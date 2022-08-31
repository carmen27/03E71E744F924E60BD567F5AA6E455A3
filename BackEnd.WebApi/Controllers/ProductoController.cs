using BackEnd.Entity;
using BackEnd.Interface.Business;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/producto")]
    public class ProductoController : ControllerBase
    {
        private readonly IBoProducto _boProducto;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ILogger<ProductoController> logger, IBoProducto boProducto)
        {
            _logger = logger;
            _boProducto = boProducto;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Producto model)
        {
            try
            {
                await _boProducto.Delete(model);
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
                var result = await _boProducto.Get(codigo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Producto model)
        {
            try
            {
                var result = await _boProducto.Save(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] Producto model)
        {
            try
            {
                var result = await _boProducto.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}