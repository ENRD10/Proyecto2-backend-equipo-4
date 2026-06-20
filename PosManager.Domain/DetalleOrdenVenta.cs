using System;
using System.Collections.Generic;
using System.Text;

namespace PosManager.Domain
{
    public class DetalleOrdenVenta
    {
        private OrdenVenta ordenVenta;
        private Producto producto;
        private decimal impuesto;
        private int cantidad;
        private decimal descuento;
        private decimal precioUnitario;
        private decimal precioSubtotal;
        private decimal precioTotal;

        public DetalleOrdenVenta()
        {

        }

        public DetalleOrdenVenta(
            OrdenVenta ordenVenta,
            Producto producto,
            decimal impuesto,
            int cantidad,
            decimal descuento,
            decimal precioUnitario,
            decimal precioSubtotal,
            decimal precioTotal)
        {
            this.ordenVenta = ordenVenta;
            this.producto = producto;
            this.impuesto = impuesto;
            this.cantidad = cantidad;
            this.descuento = descuento;
            this.precioUnitario = precioUnitario;
            this.precioSubtotal = precioSubtotal;
            this.precioTotal = precioTotal;
        }

        public OrdenVenta OrdenVenta { get => ordenVenta; set => ordenVenta = value; }
        public Producto Producto { get => producto; set => producto = value; }
        public decimal Impuesto { get => impuesto; set => impuesto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public decimal Descuento { get => descuento; set => descuento = value; }
        public decimal PrecioUnitario { get => precioUnitario; set => precioUnitario = value; }
        public decimal PrecioSubtotal { get => precioSubtotal; set => precioSubtotal = value; }
        public decimal PrecioTotal { get => precioTotal; set => precioTotal = value; }
    }
}
