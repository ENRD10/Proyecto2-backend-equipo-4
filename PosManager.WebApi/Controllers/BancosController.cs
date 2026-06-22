using Microsoft.AspNetCore.Mvc;
using PosManager.Business;
using PosManager.Domain;
using System;
using System.Collections.Generic;

namespace PosManager.WebApi.Controllers
{
    [ApiController]
    [Route("banco")]
    public class BancosController : ControllerBase
    {
        private readonly BancoBusiness _business;

        public BancosController(BancoBusiness business)
        {
            _business = business;
        }

        [HttpGet]
        public ActionResult<List<Banco>> GetBancos()
        {
            try
            {
                return Ok(_business.GetBancos());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Banco> GetBanco(int id)
        {
            try
            {
                var banco = _business.GetBanco(id);
                if (banco == null)
                    return NotFound($"No se encontró el banco con ID {id}");

                return Ok(banco);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<Banco> CreateBanco([FromBody] Banco banco)
        {
            try
            {
                var nuevo = _business.CreateBanco(banco);
                return CreatedAtAction(nameof(GetBanco), new { id = nuevo.Id }, nuevo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Banco> EditBanco(int id, [FromBody] Banco banco)
        {
            try
            {
                if (id != banco.Id)
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la petición.");

                var editado = _business.EditBanco(banco);
                return Ok(editado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBanco(int id)
        {
            try
            {
                _business.DeleteBanco(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}