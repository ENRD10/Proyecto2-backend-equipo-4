using Microsoft.AspNetCore.Mvc;
using PosManager.Business;
using PosManager.Data;
using PosManager.Domain;

namespace PosManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoBusiness _productoBusiness;

        public ProductoController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Lenguajes_Equipo4")
                ?? throw new InvalidOperationException("Connection string 'Lenguajes_Equipo4' not found.");
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