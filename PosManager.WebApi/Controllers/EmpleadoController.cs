using Microsoft.AspNetCore.Mvc;
using PosManager.Business;
using PosManager.Data;
using PosManager.Domain;

namespace PosManager.WebApi.Controllers
{
    [ApiController]
    [Route("empleado")]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoBusiness _empleadoBusiness;

        public EmpleadoController(IConfiguration configuration)
        {
            //            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection is not configured.");
            var empleadoData = new EmpleadoData(connectionString);
            _empleadoBusiness = new EmpleadoBusiness(empleadoData);
        }

        // GET /empleado
        [HttpGet]
        public ActionResult<List<Empleado>> GetEmpleados()
        {
            var empleados = _empleadoBusiness.GetEmpleados();
            return Ok(empleados);
        }
    }
}
