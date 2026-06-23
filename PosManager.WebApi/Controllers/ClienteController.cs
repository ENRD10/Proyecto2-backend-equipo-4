using Microsoft.AspNetCore.Mvc;
using PosManager.Business;
using PosManager.Data;
using PosManager.Domain;

namespace PosManager.WebApi.Controllers
{
    [ApiController]
    [Route("cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteBusiness _clienteBusiness;

        public ClienteController(IConfiguration configuration)
        {
            //            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection is not configured.");
            var clienteData = new ClienteData(connectionString);
            _clienteBusiness = new ClienteBusiness(clienteData);
        }

        // GET /cliente
        [HttpGet]
        public ActionResult<List<Cliente>> GetClientes()
        {
            var clientes = _clienteBusiness.GetClientes();
            return Ok(clientes);
        }

        // GET /cliente/id
        [HttpGet("id")]
        public ActionResult<Cliente> GetCliente(int id)
        {
            var cliente = _clienteBusiness.GetCliente(id);
            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }
    }
}
