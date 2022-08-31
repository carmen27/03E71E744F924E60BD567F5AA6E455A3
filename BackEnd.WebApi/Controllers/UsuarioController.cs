using BackEnd.Entity;
using BackEnd.Interface.Business;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IBoUsuario _boUsuario;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger, IBoUsuario boUsuario)
        {
            _logger = logger;
            _boUsuario = boUsuario;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Usuario model)
        {
            try
            {
                await _boUsuario.Delete(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string codigo)
        {
            try
            {
                var result = await _boUsuario.Get(codigo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Usuario model)
        {
            try
            {
                var result = await _boUsuario.Save(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] Usuario model)
        {
            try
            {
                var result = await _boUsuario.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}