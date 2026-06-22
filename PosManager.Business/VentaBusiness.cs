
using PosManager.Data;
using PosManager.Domain.DTOs;

namespace PosManager.Business
{
    public class VentaBusiness
    {
        private readonly VentaData _ventaData;

        public VentaBusiness(string connectionString)
        {
            _ventaData = new VentaData(connectionString);
        }

        public int CreateVenta(CreateVentaRequest request)
        {
            // Validaciones de negocio
            if (request.IdCliente <= 0)
                throw new ArgumentException("El IdCliente es inválido.");

            if (request.IdEmpleado <= 0)
                throw new ArgumentException("El IdEmpleado es inválido.");

            if (request.Detalles == null || request.Detalles.Count == 0)
                throw new ArgumentException("La venta debe tener al menos un detalle.");

            foreach (var d in request.Detalles)
            {
                if (d.Cantidad <= 0)
                    throw new ArgumentException($"La cantidad del producto {d.IdProducto} debe ser mayor a 0.");

                if (d.PrecioUnitario <= 0)
                    throw new ArgumentException($"El precio unitario del producto {d.IdProducto} debe ser mayor a 0.");

                if (d.Descuento < 0 || d.Descuento > 100)
                    throw new ArgumentException($"El descuento del producto {d.IdProducto} debe estar entre 0 y 100.");
            }

            return _ventaData.CreateVenta(request);
        }

        public List<VentaResponse> GetAllVentas() =>
            _ventaData.GetAllVentas();

        public VentaResponse? GetVentaById(int numeroOrden)
        {
            if (numeroOrden <= 0)
                throw new ArgumentException("El NumeroOrden es inválido.");

            return _ventaData.GetVentaById(numeroOrden);
        }

        public bool UpdateVenta(int numeroOrden, CreateVentaRequest request)
        {
            if (numeroOrden <= 0)
                throw new ArgumentException("El NumeroOrden es inválido.");

            if (request.IdCliente <= 0)
                throw new ArgumentException("El IdCliente es inválido.");

            if (request.IdEmpleado <= 0)
                throw new ArgumentException("El IdEmpleado es inválido.");

            return _ventaData.UpdateVenta(numeroOrden, request);
        }

        public bool DeleteVenta(int numeroOrden)
        {
            if (numeroOrden <= 0)
                throw new ArgumentException("El NumeroOrden es inválido.");

            return _ventaData.DeleteVenta(numeroOrden);
        }
    }
}