using System.Collections.Generic;
using PosManager.Data;
using PosManager.Domain;

namespace PosManager.Business
{
    public class ProductoBusiness
    {
        private readonly ProductoData _productoData;

        public ProductoBusiness(ProductoData productoData)
        {
            _productoData = productoData;
        }

        public List<Producto> GetProductos()
        {
            return _productoData.GetProductos();
        }
    }
}