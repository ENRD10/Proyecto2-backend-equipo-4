using System;
using System.Collections.Generic;
using System.Text;

namespace PosManager.Domain
{
    public class OrdenVenta
    {
        private int numeroOrden;
        private Cliente cliente;
        private Empleado empleado;
        private DateTime fechaOrden;
        private decimal totalOrden;
        private decimal impuesto;

        public OrdenVenta()
        {

        }

        public OrdenVenta(int numeroOrden, Cliente cliente,
            Empleado empleado, DateTime fechaOrden,
            decimal totalOrden, decimal impuesto)
        {
            this.numeroOrden = numeroOrden;
            this.cliente = cliente;
            this.empleado = empleado;
            this.fechaOrden = fechaOrden;
            this.totalOrden = totalOrden;
            this.impuesto = impuesto;
        }

        public int NumeroOrden { get => numeroOrden; set => numeroOrden = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }
        public Empleado Empleado { get => empleado; set => empleado = value; }
        public DateTime FechaOrden { get => fechaOrden; set => fechaOrden = value; }
        public decimal TotalOrden { get => totalOrden; set => totalOrden = value; }
        public decimal Impuesto { get => impuesto; set => impuesto = value; }
    }
}
