using System;
using System.Collections.Generic;
using System.Text;

namespace PosManager.Domain
{
    public class Producto
    {
        private int productId;
        private string nombreEtiqueta;
        private string descripcion;
        private int existenciaEnStock;
        private int existenciaLimiteParaAlmacenar;
        private decimal precioNeto;
        private decimal precioMinimo;
        private decimal precioMinimoConImpuesto;
        private DateTime fechaCreacion;
        private bool puedeVenderse;
        private bool puedeComprarse;
        private decimal impuestoValorAgregado;
        private decimal impuestoLocal1;
        private decimal impuestoLocal2;
        private bool tieneImpuesto;
        private string codigoBarra;
        private string imagen;
        private string codigoContableVentas;
        private string codigoContableCompras;
        private decimal peso;
        private string unidadPeso;
        private decimal longitud;
        private string unidadLongitud;
        private decimal superficie;
        private string unidadSuperficie;
        private decimal volumen;
        private List<Categoria> categorias;

        public Producto()
        {

        }

        public Producto(
            int productId,
            string nombreEtiqueta,
            string descripcion,
            int existenciaEnStock,
            int existenciaLimiteParaAlmacenar,
            decimal precioNeto,
            decimal precioMinimo,
            decimal precioMinimoConImpuesto,
            DateTime fechaCreacion,
            bool puedeVenderse,
            bool puedeComprarse,
            decimal impuestoValorAgregado,
            decimal impuestoLocal1,
            decimal impuestoLocal2,
            bool tieneImpuesto,
            string codigoBarra,
            string imagen,
            string codigoContableVentas,
            string codigoContableCompras,
            decimal peso,
            string unidadPeso,
            decimal longitud,
            string unidadLongitud,
            decimal superficie,
            string unidadSuperficie,
            decimal volumen,
            List<Categoria> categorias)
        {
            this.productId = productId;
            this.nombreEtiqueta = nombreEtiqueta;
            this.descripcion = descripcion;
            this.existenciaEnStock = existenciaEnStock;
            this.existenciaLimiteParaAlmacenar = existenciaLimiteParaAlmacenar;
            this.precioNeto = precioNeto;
            this.precioMinimo = precioMinimo;
            this.precioMinimoConImpuesto = precioMinimoConImpuesto;
            this.fechaCreacion = fechaCreacion;
            this.puedeVenderse = puedeVenderse;
            this.puedeComprarse = puedeComprarse;
            this.impuestoValorAgregado = impuestoValorAgregado;
            this.impuestoLocal1 = impuestoLocal1;
            this.impuestoLocal2 = impuestoLocal2;
            this.tieneImpuesto = tieneImpuesto;
            this.codigoBarra = codigoBarra;
            this.imagen = imagen;
            this.codigoContableVentas = codigoContableVentas;
            this.codigoContableCompras = codigoContableCompras;
            this.peso = peso;
            this.unidadPeso = unidadPeso;
            this.longitud = longitud;
            this.unidadLongitud = unidadLongitud;
            this.superficie = superficie;
            this.unidadSuperficie = unidadSuperficie;
            this.volumen = volumen;
            this.categorias = categorias;
        }

        public int ProductId
        {
            get => productId;
            set => productId = value;
        }

        public string NombreEtiqueta
        {
            get => nombreEtiqueta;
            set => nombreEtiqueta = value;
        }

        public string Descripcion
        {
            get => descripcion;
            set => descripcion = value;
        }

        public int ExistenciaEnStock
        {
            get => existenciaEnStock;
            set => existenciaEnStock = value;
        }

        public int ExistenciaLimiteParaAlmacenar
        {
            get => existenciaLimiteParaAlmacenar;
            set => existenciaLimiteParaAlmacenar = value;
        }

        public decimal PrecioNeto
        {
            get => precioNeto;
            set => precioNeto = value;
        }

        public decimal PrecioMinimo
        {
            get => precioMinimo;
            set => precioMinimo = value;
        }

        public decimal PrecioMinimoConImpuesto
        {
            get => precioMinimoConImpuesto;
            set => precioMinimoConImpuesto = value;
        }

        public DateTime FechaCreacion
        {
            get => fechaCreacion;
            set => fechaCreacion = value;
        }

        public bool PuedeVenderse
        {
            get => puedeVenderse;
            set => puedeVenderse = value;
        }

        public bool PuedeComprarse
        {
            get => puedeComprarse;
            set => puedeComprarse = value;
        }

        public decimal ImpuestoValorAgregado
        {
            get => impuestoValorAgregado;
            set => impuestoValorAgregado = value;
        }

        public decimal ImpuestoLocal1
        {
            get => impuestoLocal1;
            set => impuestoLocal1 = value;
        }

        public decimal ImpuestoLocal2
        {
            get => impuestoLocal2;
            set => impuestoLocal2 = value;
        }

        public bool TieneImpuesto
        {
            get => tieneImpuesto;
            set => tieneImpuesto = value;
        }

        public string CodigoBarra
        {
            get => codigoBarra;
            set => codigoBarra = value;
        }

        public string Imagen
        {
            get => imagen;
            set => imagen = value;
        }

        public string CodigoContableVentas
        {
            get => codigoContableVentas;
            set => codigoContableVentas = value;
        }

        public string CodigoContableCompras
        {
            get => codigoContableCompras;
            set => codigoContableCompras = value;
        }

        public decimal Peso
        {
            get => peso;
            set => peso = value;
        }

        public string UnidadPeso
        {
            get => unidadPeso;
            set => unidadPeso = value;
        }

        public decimal Longitud
        {
            get => longitud;
            set => longitud = value;
        }

        public string UnidadLongitud
        {
            get => unidadLongitud;
            set => unidadLongitud = value;
        }

        public decimal Superficie
        {
            get => superficie;
            set => superficie = value;
        }

        public string UnidadSuperficie
        {
            get => unidadSuperficie;
            set => unidadSuperficie = value;
        }

        public decimal Volumen
        {
            get => volumen;
            set => volumen = value;
        }

        public List<Categoria> Categorias
        {
            get => categorias;
            set => categorias = value;
        }
    }
}
