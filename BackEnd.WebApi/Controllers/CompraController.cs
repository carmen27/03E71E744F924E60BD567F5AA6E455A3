using BackEnd.Entity;
using BackEnd.Interface.Business;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/usuario")]
    public class CompraController : ControllerBase
    {
        private readonly IBoCompra _boCompra;
        private readonly ILogger<CompraController> _logger;

        public CompraController(ILogger<CompraController> logger, IBoCompra boCompra)
        {
            _logger = logger;
            _boCompra = boCompra;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Compra model)
        {
            try
            {
                await _boCompra.Delete(model);
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
                var result = await _boCompra.Get(codigo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Compra model)
        {
            try
            {
                var result = await _boCompra.Save(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Compra model)
        {
            try
            {
                var result = await _boCompra.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}