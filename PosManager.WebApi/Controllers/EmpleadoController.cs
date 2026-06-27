using Microsoft.AspNetCore.Mvc;
using PosManager.Business;
using PosManager.Data;
using PosManager.Domain;

namespace PosManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/empleado")]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoBusiness _empleadoBusiness;

        public EmpleadoController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Lenguajes_Equipo4")
                ?? throw new InvalidOperationException("Connection string 'Lenguajes_Equipo4' not found.");
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
