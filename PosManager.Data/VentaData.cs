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

                // 2. Insertar cabecera en pos_orden
                int numeroOrden;
                const string sqlCabecera = @"
                    INSERT INTO pos_orden 
                        (fk_cliente, fk_empleado, fecha_orden, total_orden, impuesto)
                    OUTPUT INSERTED.numero_orden
                    VALUES 
                        (@fk_cliente, @fk_empleado, @fecha_orden, @total_orden, @impuesto)";

                using (var cmdCabecera = new SqlCommand(sqlCabecera, connection, transaction))
                {
                    cmdCabecera.Parameters.AddWithValue("@fk_cliente", request.IdCliente);
                    cmdCabecera.Parameters.AddWithValue("@fk_empleado", request.IdEmpleado);
                    cmdCabecera.Parameters.AddWithValue("@fecha_orden", request.FechaOrden);
                    cmdCabecera.Parameters.AddWithValue("@total_orden", totalOrden);
                    cmdCabecera.Parameters.AddWithValue("@impuesto", request.Impuesto);

                    numeroOrden = (int)cmdCabecera.ExecuteScalar();
                }

                // 3. Insertar cada línea en pos_orden_detalle
                const string sqlDetalle = @"
                    INSERT INTO pos_orden_detalle 
                        (fk_pos_orden, fk_producto, cantidad, precio_unitario, 
                         descuento, impuesto, precio_subtotal, precio_total)
                    VALUES 
                        (@fk_pos_orden, @fk_producto, @cantidad, @precio_unitario,
                         @descuento, @impuesto, @precio_subtotal, @precio_total)";

                foreach (var detalle in request.Detalles)
                {
                    decimal subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    decimal descuento = subtotal * (detalle.Descuento / 100);
                    decimal precioSubtotal = subtotal - descuento;
                    decimal precioTotal = precioSubtotal * (1 + detalle.Impuesto / 100);

                    using var cmdDetalle = new SqlCommand(sqlDetalle, connection, transaction);
                    cmdDetalle.Parameters.AddWithValue("@fk_pos_orden", numeroOrden);
                    cmdDetalle.Parameters.AddWithValue("@fk_producto", detalle.IdProducto);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@precio_unitario", detalle.PrecioUnitario);
                    cmdDetalle.Parameters.AddWithValue("@descuento", detalle.Descuento);
                    cmdDetalle.Parameters.AddWithValue("@impuesto", detalle.Impuesto);
                    cmdDetalle.Parameters.AddWithValue("@precio_subtotal", precioSubtotal);
                    cmdDetalle.Parameters.AddWithValue("@precio_total", precioTotal);
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
                                    ov.numero_orden, 
                                    ov.fk_cliente, 
                                    ov.fk_empleado,
                                    ov.fecha_orden, 
                                    ov.total_orden, 
                                    ov.impuesto,
                                    CONCAT(c.nombre, ' ', c.apellidos) AS NombreCliente,
                                    CONCAT(e.nombre, ' ', e.apellidos) AS NombreEmpleado
                                FROM Pos_Orden ov
                                INNER JOIN Cliente c ON ov.fk_cliente = c.id
                                INNER JOIN Empleado e ON ov.fk_empleado = e.id
                                ORDER BY ov.fecha_orden DESC";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var cmd = new SqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ventas.Add(new VentaResponse
                {
                    NumeroOrden = reader.GetInt32(reader.GetOrdinal("numero_orden")),
                    IdCliente = reader.GetInt32(reader.GetOrdinal("fk_cliente")),
                    NombreCliente = reader.GetString(reader.GetOrdinal("NombreCliente")),
                    IdEmpleado = reader.GetInt32(reader.GetOrdinal("fk_empleado")),
                    NombreEmpleado = reader.GetString(reader.GetOrdinal("NombreEmpleado")),
                    FechaOrden = reader.GetDateTime(reader.GetOrdinal("fecha_orden")),
                    TotalOrden = reader.GetDecimal(reader.GetOrdinal("total_orden")),
                    Impuesto = reader.GetDecimal(reader.GetOrdinal("impuesto"))
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
                                            ov.numero_orden, 
                                            ov.fk_cliente, 
                                            ov.fk_empleado,
                                            ov.fecha_orden, 
                                            ov.total_orden, 
                                            ov.impuesto,
                                            CONCAT(c.nombre, ' ', c.apellidos) AS NombreCliente,
                                            CONCAT(e.nombre, ' ', e.apellidos) AS NombreEmpleado
                                        FROM Pos_Orden ov
                                        INNER JOIN Cliente c ON ov.fk_cliente = c.id
                                        INNER JOIN Empleado e ON ov.fk_empleado = e.id
                                        WHERE ov.numero_orden = @numero_orden";

            const string sqlDetalles = @"
                                        SELECT 
                                            d.fk_producto, 
                                            pr.nombre_etiqueta AS NombreProducto,
                                            d.cantidad, 
                                            d.precio_unitario, 
                                            d.descuento,
                                            d.impuesto, 
                                            d.precio_subtotal, 
                                            d.precio_total
                                        FROM Pos_Orden_Detalle d
                                        INNER JOIN Producto pr ON d.fk_producto = pr.product_id
                                        WHERE d.fk_pos_orden = @numero_orden";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            // 1. Leer Cabecera
            using (var cmd = new SqlCommand(sqlCabecera, connection))
            {
                cmd.Parameters.AddWithValue("@numero_orden", numeroOrden);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    venta = new VentaResponse
                    {
                        NumeroOrden = reader.GetInt32(reader.GetOrdinal("numero_orden")),
                        IdCliente = reader.GetInt32(reader.GetOrdinal("fk_cliente")),
                        NombreCliente = reader.GetString(reader.GetOrdinal("NombreCliente")),
                        IdEmpleado = reader.GetInt32(reader.GetOrdinal("fk_empleado")),
                        NombreEmpleado = reader.GetString(reader.GetOrdinal("NombreEmpleado")),
                        FechaOrden = reader.GetDateTime(reader.GetOrdinal("fecha_orden")),
                        TotalOrden = reader.GetDecimal(reader.GetOrdinal("total_orden")),
                        Impuesto = reader.GetDecimal(reader.GetOrdinal("impuesto")),
                        // Asegúrate de inicializar la lista de detalles si tu clase no lo hace en el constructor
                        Detalles = new List<DetalleVentaResponse>()
                    };
                }
            }

            // Si no se encontró la cabecera, salimos temprano
            if (venta == null) return null;

            // 2. Leer Detalles
            using (var cmd = new SqlCommand(sqlDetalles, connection))
            {
                cmd.Parameters.AddWithValue("@numero_orden", numeroOrden);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    venta.Detalles.Add(new DetalleVentaResponse
                    {
                        IdProducto = reader.GetInt32(reader.GetOrdinal("fk_producto")),
                        NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("cantidad")),
                        PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("precio_unitario")),
                        Descuento = reader.GetDecimal(reader.GetOrdinal("descuento")),
                        Impuesto = reader.GetDecimal(reader.GetOrdinal("impuesto")),
                        PrecioSubtotal = reader.GetDecimal(reader.GetOrdinal("precio_subtotal")),
                        PrecioTotal = reader.GetDecimal(reader.GetOrdinal("precio_total"))
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
                UPDATE pos_orden SET
                    fk_cliente  = @fk_cliente,
                    fk_empleado = @fk_empleado,
                    fecha_orden = @fecha_orden,
                    impuesto   = @impuesto
                WHERE numero_orden = @numero_orden";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@numero_orden", numeroOrden);
            cmd.Parameters.AddWithValue("@fk_cliente", request.IdCliente);
            cmd.Parameters.AddWithValue("@fk_empleado", request.IdEmpleado);
            cmd.Parameters.AddWithValue("@fecha_orden", request.FechaOrden);
            cmd.Parameters.AddWithValue("@impuesto", request.Impuesto);

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
                    "DELETE FROM pos_orden_detalle WHERE fk_pos_orden = @numero_orden",
                    connection, transaction))
                {
                    cmdDetalle.Parameters.AddWithValue("@numero_orden", numeroOrden);
                    cmdDetalle.ExecuteNonQuery();
                }

                // Luego eliminar cabecera
                int rows;
                using (var cmdCab = new SqlCommand(
                    "DELETE FROM pos_orden WHERE numero_orden = @numero_orden",
                    connection, transaction))
                {
                    cmdCab.Parameters.AddWithValue("@numero_orden", numeroOrden);
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