using BackEnd.Entity;
using BackEnd.Interface.Business;
using BackEnd.Interface.Data;

namespace BackEnd.Business
{
    public class BoUsuario : IBoUsuario
    {
        private readonly IDoUsuario _doUsuario;

        public BoUsuario(IDoUsuario doUsuario)
        {
            _doUsuario = doUsuario;
        }

        public async Task<Usuario?> Get(string codigo)
        {
            var result = await _doUsuario.GetByCodigo(codigo);

            if (result != null)
            {
                return new Usuario()
                {
                    Codigo = result.Ccodigo,
                    Nombre = result.Cnombre
                };
            }

            return null;
        }
    }
}