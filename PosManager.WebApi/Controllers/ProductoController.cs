using Microsoft.AspNetCore.Mvc;
using PosManager.Business;
using PosManager.Data;
using PosManager.Domain;

namespace PosManager.WebApi.Controllers
{
    [ApiController]
    [Route("producto")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoBusiness _productoBusiness;

        public ProductoController(IConfiguration configuration)
        {

            // Obtener la cadena de conexión desde la configuración
            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection not found");
            var productoData = new ProductoData(connectionString);
            _productoBusiness = new ProductoBusiness(productoData);
        }

        // GET /producto
        [HttpGet]
        public ActionResult<List<Producto>> GetProductos()
        {
            var productos = _productoBusiness.GetProductos();
            return Ok(productos);
        }
    }
}