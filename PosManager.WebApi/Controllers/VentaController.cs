=====
using Microsoft.AspNetCore.Mvc;
using PosManager.Business;
using PosManager.Domain.DTOs;
 
namespace PosManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/venta")]
    public class VentaController : ControllerBase
    {
        private readonly VentaBusiness _ventaBusiness;

        public VentaController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Lenguajes_Equipo4")
                ?? throw new InvalidOperationException("Connection string 'Lenguajes_Equipo4' not found.");

            _ventaBusiness = new VentaBusiness(connectionString);
        }

        
        [HttpPost]
        public IActionResult CreateVenta([FromBody] CreateVentaRequest request)
        {
            try
            {
                int numeroOrden = _ventaBusiness.CreateVenta(request);

                return CreatedAtAction(
                    nameof(GetVentaById),
                    new { numeroOrden },
                    new { NumeroOrden = numeroOrden, Mensaje = "Venta creada exitosamente." }
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error interno del servidor.", Detalle = ex.Message });
            }
        }

        // GET api/venta
        [HttpGet]
        public IActionResult GetAllVentas()
        {
            try
            {
                var ventas = _ventaBusiness.GetAllVentas();
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error interno del servidor.", Detalle = ex.Message });
            }
        }

        // GET api/venta/{numeroOrden}
        [HttpGet("{numeroOrden}")]
        public IActionResult GetVentaById(int numeroOrden)
        {
            try
            {
                var venta = _ventaBusiness.GetVentaById(numeroOrden);

                if (venta == null)
                    return NotFound(new { Error = $"No se encontró la venta con NumeroOrden {numeroOrden}." });

                return Ok(venta);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error interno del servidor.", Detalle = ex.Message });
            }
        }

        // PUT api/venta/{numeroOrden}
        [HttpPut("{numeroOrden}")]
        public IActionResult UpdateVenta(int numeroOrden, [FromBody] CreateVentaRequest request)
        {
            try
            {
                bool actualizado = _ventaBusiness.UpdateVenta(numeroOrden, request);

                if (!actualizado)
                    return NotFound(new { Error = $"No se encontró la venta con NumeroOrden {numeroOrden}." });

                return Ok(new { Mensaje = "Venta actualizada exitosamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error interno del servidor.", Detalle = ex.Message });
            }
        }

        // DELETE api/venta/{numeroOrden}
        [HttpDelete("{numeroOrden}")]
        public IActionResult DeleteVenta(int numeroOrden)
        {
            try
            {
                bool eliminado = _ventaBusiness.DeleteVenta(numeroOrden);

                if (!eliminado)
                    return NotFound(new { Error = $"No se encontró la venta con NumeroOrden {numeroOrden}." });

                return Ok(new { Mensaje = "Venta eliminada exitosamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error interno del servidor.", Detalle = ex.Message });
            }
        }
    }
}