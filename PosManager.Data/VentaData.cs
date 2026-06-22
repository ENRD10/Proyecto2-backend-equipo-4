
using Microsoft.Data.SqlClient;
using PosManager.Domain;
using PosManager.Domain.DTOs;

namespace PosManager.Data
{
    public class VentaData
    {
        private readonly string _connectionString;

        public VentaData(string connectionString)
        {
            _connectionString = connectionString;
        }

        // -----------------------------------------------------------
        // CREATE - Inserta cabecera + detalles en una transacción
        // -----------------------------------------------------------
        public int CreateVenta(CreateVentaRequest request)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Calcular total de la orden a partir de los detalles
                decimal totalOrden = 0;
                foreach (var d in request.Detalles)
                {
                    decimal subtotal = d.Cantidad * d.PrecioUnitario;
                    decimal descuento = subtotal * (d.Descuento / 100);
                    decimal precioConImpuesto = (subtotal - descuento) * (1 + d.Impuesto / 100);
                    totalOrden += precioConImpuesto;
                }

                // 2. Insertar cabecera en OrdenVenta
                int numeroOrden;
                const string sqlCabecera = @"
                    INSERT INTO OrdenVenta 
                        (IdCliente, IdEmpleado, FechaOrden, TotalOrden, Impuesto)
                    OUTPUT INSERTED.NumeroOrden
                    VALUES 
                        (@IdCliente, @IdEmpleado, @FechaOrden, @TotalOrden, @Impuesto)";

                using (var cmdCabecera = new SqlCommand(sqlCabecera, connection, transaction))
                {
                    cmdCabecera.Parameters.AddWithValue("@IdCliente", request.IdCliente);
                    cmdCabecera.Parameters.AddWithValue("@IdEmpleado", request.IdEmpleado);
                    cmdCabecera.Parameters.AddWithValue("@FechaOrden", request.FechaOrden);
                    cmdCabecera.Parameters.AddWithValue("@TotalOrden", totalOrden);
                    cmdCabecera.Parameters.AddWithValue("@Impuesto", request.Impuesto);

                    numeroOrden = (int)cmdCabecera.ExecuteScalar();
                }

                // 3. Insertar cada línea en DetalleOrdenVenta
                const string sqlDetalle = @"
                    INSERT INTO DetalleOrdenVenta 
                        (NumeroOrden, IdProducto, Cantidad, PrecioUnitario, 
                         Descuento, Impuesto, PrecioSubtotal, PrecioTotal)
                    VALUES 
                        (@NumeroOrden, @IdProducto, @Cantidad, @PrecioUnitario,
                         @Descuento, @Impuesto, @PrecioSubtotal, @PrecioTotal)";

                foreach (var detalle in request.Detalles)
                {
                    decimal subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    decimal descuento = subtotal * (detalle.Descuento / 100);
                    decimal precioSubtotal = subtotal - descuento;
                    decimal precioTotal = precioSubtotal * (1 + detalle.Impuesto / 100);

                    using var cmdDetalle = new SqlCommand(sqlDetalle, connection, transaction);
                    cmdDetalle.Parameters.AddWithValue("@NumeroOrden", numeroOrden);
                    cmdDetalle.Parameters.AddWithValue("@IdProducto", detalle.IdProducto);
                    cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                    cmdDetalle.Parameters.AddWithValue("@Descuento", detalle.Descuento);
                    cmdDetalle.Parameters.AddWithValue("@Impuesto", detalle.Impuesto);
                    cmdDetalle.Parameters.AddWithValue("@PrecioSubtotal", precioSubtotal);
                    cmdDetalle.Parameters.AddWithValue("@PrecioTotal", precioTotal);
                    cmdDetalle.ExecuteNonQuery();
                }

                transaction.Commit();
                return numeroOrden;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // -----------------------------------------------------------
        // GET ALL - Lista todas las órdenes de venta
        // -----------------------------------------------------------
        public List<VentaResponse> GetAllVentas()
        {
            var ventas = new List<VentaResponse>();

            const string sql = @"
                SELECT 
                    ov.NumeroOrden, ov.IdCliente, ov.IdEmpleado,
                    ov.FechaOrden, ov.TotalOrden, ov.Impuesto,
                    p_cli.Nombre + ' ' + p_cli.Apellidos AS NombreCliente,
                    p_emp.Nombre + ' ' + p_emp.Apellidos AS NombreEmpleado
                FROM OrdenVenta ov
                INNER JOIN Cliente c ON ov.IdCliente = c.Id
                INNER JOIN Persona p_cli ON c.Id = p_cli.Id
                INNER JOIN Empleado e ON ov.IdEmpleado = e.Id
                INNER JOIN Persona p_emp ON e.Id = p_emp.Id
                ORDER BY ov.FechaOrden DESC";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var cmd = new SqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ventas.Add(new VentaResponse
                {
                    NumeroOrden = reader.GetInt32(reader.GetOrdinal("NumeroOrden")),
                    IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                    NombreCliente = reader.GetString(reader.GetOrdinal("NombreCliente")),
                    IdEmpleado = reader.GetInt32(reader.GetOrdinal("IdEmpleado")),
                    NombreEmpleado = reader.GetString(reader.GetOrdinal("NombreEmpleado")),
                    FechaOrden = reader.GetDateTime(reader.GetOrdinal("FechaOrden")),
                    TotalOrden = reader.GetDecimal(reader.GetOrdinal("TotalOrden")),
                    Impuesto = reader.GetDecimal(reader.GetOrdinal("Impuesto"))
                });
            }

            return ventas;
        }

        // -----------------------------------------------------------
        // GET BY ID - Obtiene una venta con sus detalles
        // -----------------------------------------------------------
        public VentaResponse? GetVentaById(int numeroOrden)
        {
            VentaResponse? venta = null;

            const string sqlCabecera = @"
                SELECT 
                    ov.NumeroOrden, ov.IdCliente, ov.IdEmpleado,
                    ov.FechaOrden, ov.TotalOrden, ov.Impuesto,
                    p_cli.Nombre + ' ' + p_cli.Apellidos AS NombreCliente,
                    p_emp.Nombre + ' ' + p_emp.Apellidos AS NombreEmpleado
                FROM OrdenVenta ov
                INNER JOIN Cliente c ON ov.IdCliente = c.Id
                INNER JOIN Persona p_cli ON c.Id = p_cli.Id
                INNER JOIN Empleado e ON ov.IdEmpleado = e.Id
                INNER JOIN Persona p_emp ON e.Id = p_emp.Id
                WHERE ov.NumeroOrden = @NumeroOrden";

            const string sqlDetalles = @"
                SELECT 
                    d.IdProducto, pr.NombreEtiqueta AS NombreProducto,
                    d.Cantidad, d.PrecioUnitario, d.Descuento,
                    d.Impuesto, d.PrecioSubtotal, d.PrecioTotal
                FROM DetalleOrdenVenta d
                INNER JOIN Producto pr ON d.IdProducto = pr.ProductId
                WHERE d.NumeroOrden = @NumeroOrden";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using (var cmd = new SqlCommand(sqlCabecera, connection))
            {
                cmd.Parameters.AddWithValue("@NumeroOrden", numeroOrden);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    venta = new VentaResponse
                    {
                        NumeroOrden = reader.GetInt32(reader.GetOrdinal("NumeroOrden")),
                        IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                        NombreCliente = reader.GetString(reader.GetOrdinal("NombreCliente")),
                        IdEmpleado = reader.GetInt32(reader.GetOrdinal("IdEmpleado")),
                        NombreEmpleado = reader.GetString(reader.GetOrdinal("NombreEmpleado")),
                        FechaOrden = reader.GetDateTime(reader.GetOrdinal("FechaOrden")),
                        TotalOrden = reader.GetDecimal(reader.GetOrdinal("TotalOrden")),
                        Impuesto = reader.GetDecimal(reader.GetOrdinal("Impuesto"))
                    };
                }
            }

            if (venta == null) return null;

            using (var cmd = new SqlCommand(sqlDetalles, connection))
            {
                cmd.Parameters.AddWithValue("@NumeroOrden", numeroOrden);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    venta.Detalles.Add(new DetalleVentaResponse
                    {
                        IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                        NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                        PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                        Descuento = reader.GetDecimal(reader.GetOrdinal("Descuento")),
                        Impuesto = reader.GetDecimal(reader.GetOrdinal("Impuesto")),
                        PrecioSubtotal = reader.GetDecimal(reader.GetOrdinal("PrecioSubtotal")),
                        PrecioTotal = reader.GetDecimal(reader.GetOrdinal("PrecioTotal"))
                    });
                }
            }

            return venta;
        }

        // -----------------------------------------------------------
        // UPDATE - Actualiza cabecera de la orden (no los detalles)
        // -----------------------------------------------------------
        public bool UpdateVenta(int numeroOrden, CreateVentaRequest request)
        {
            const string sql = @"
                UPDATE OrdenVenta SET
                    IdCliente  = @IdCliente,
                    IdEmpleado = @IdEmpleado,
                    FechaOrden = @FechaOrden,
                    Impuesto   = @Impuesto
                WHERE NumeroOrden = @NumeroOrden";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@NumeroOrden", numeroOrden);
            cmd.Parameters.AddWithValue("@IdCliente", request.IdCliente);
            cmd.Parameters.AddWithValue("@IdEmpleado", request.IdEmpleado);
            cmd.Parameters.AddWithValue("@FechaOrden", request.FechaOrden);
            cmd.Parameters.AddWithValue("@Impuesto", request.Impuesto);

            return cmd.ExecuteNonQuery() > 0;
        }

        // -----------------------------------------------------------
        // DELETE - Elimina una orden y sus detalles en cascada
        // -----------------------------------------------------------
        public bool DeleteVenta(int numeroOrden)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Primero eliminar detalles (FK)
                using (var cmdDetalle = new SqlCommand(
                    "DELETE FROM DetalleOrdenVenta WHERE NumeroOrden = @NumeroOrden",
                    connection, transaction))
                {
                    cmdDetalle.Parameters.AddWithValue("@NumeroOrden", numeroOrden);
                    cmdDetalle.ExecuteNonQuery();
                }

                // Luego eliminar cabecera
                int rows;
                using (var cmdCab = new SqlCommand(
                    "DELETE FROM OrdenVenta WHERE NumeroOrden = @NumeroOrden",
                    connection, transaction))
                {
                    cmdCab.Parameters.AddWithValue("@NumeroOrden", numeroOrden);
                    rows = cmdCab.ExecuteNonQuery();
                }

                transaction.Commit();
                return rows > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}