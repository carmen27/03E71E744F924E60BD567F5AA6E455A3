using BackEnd.Interface.Business;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IBoUsuario _boUsuario;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger, IBoUsuario boUsuario)
        {
            _logger = logger;
            _boUsuario = boUsuario;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string codigo)
        {
            var result = await _boUsuario.Get(codigo);
            return Ok(result);
        }
    }
}