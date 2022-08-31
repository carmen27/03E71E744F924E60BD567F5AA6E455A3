using BackEnd.Common;
using BackEnd.Entity;
using BackEnd.Interface.Business;
using BackEnd.Interface.Data;
using BackEnd.Model;
using BackEnd.Toolkit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackEnd.Business
{
    public class BoUsuario : IBoUsuario
    {
        private readonly AppSettings _appSettings;
        private readonly IDoUsuario _doUsuario;
        private readonly ILogger<BoUsuario> _logger;

        public BoUsuario(ILogger<BoUsuario> logger,
                         IOptions<AppSettings> options,
                         IDoUsuario doUsuario)
        {
            _logger = logger;
            _appSettings = options.Value;
            _doUsuario = doUsuario;
        }

        public async Task Delete(Usuario model)
        {
            try
            {
                model.Codigo = ConvertHelper.ToNonNullString(model.Codigo);

                if (model == null)
                {
                    throw new ArgumentException("Se requieren parámetros");
                }

                if (model.Codigo == string.Empty)
                {
                    throw new ArgumentException("Se requiere código de usuario");
                }

                var usuario = await _doUsuario.GetByCodigo(model.Codigo);

                if (usuario == null)
                {
                    throw new ArgumentException("Código de usuario no existe");
                }

                usuario.Cestado = "N";
                usuario.Cusumodi = "ADMIN";
                usuario.Dfecmodi = DateTime.Now;

                if (!await _doUsuario.Update(usuario))
                {
                    throw new Exception("Ocurrió un error al grabar usuario");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Usuario?> Get(string codigo)
        {
            try
            {
                var result = await _doUsuario.GetByCodigo(codigo);

                if (result != null)
                {
                    return new Usuario()
                    {
                        Codigo = result.Ccodigo,
                        Nombres = result.Cnombres,
                        Apellidos = result.Capellidos,
                        Email = result.Cemail,
                        Telefono = result.Cnumero1,
                        TipoDocum = result.Ntipdocum,
                        NumDocum = result.Cnumdocum,
                    };
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Usuario> Save(Usuario model)
        {
            try
            {
                model.Apellidos = ConvertHelper.ToNonNullString(model.Apellidos);
                model.Email = ConvertHelper.ToNonNullString(model.Email);
                model.Nombres = ConvertHelper.ToNonNullString(model.Nombres);
                model.Username = ConvertHelper.ToNonNullString(model.Username);
                model.Password = ConvertHelper.ToNonNullString(model.Password);
                model.NumDocum = ConvertHelper.ToNonNullString(model.NumDocum);
                model.Telefono = ConvertHelper.ToNonNullString(model.Telefono);

                if (model == null)
                {
                    throw new ArgumentException("Se requieren parámetros");
                }

                if (model.TipoDocum == null)
                {
                    throw new ArgumentException("Se requiere tipo de documento");
                }

                if (model.NumDocum == null)
                {
                    throw new ArgumentException("Se requiere número de documento");
                }

                if (model.Password == string.Empty)
                {
                    throw new ArgumentException("Se requiere password");
                }

                if (model.Email == string.Empty)
                {
                    throw new ArgumentException("Se requiere correo");
                }

                if (model.Nombres == string.Empty)
                {
                    throw new ArgumentException("Se requiere nombre(s)");
                }

                if (model.Apellidos == string.Empty)
                {
                    throw new ArgumentException("Se requiere apellido(s)");
                }

                if (model.Telefono == string.Empty)
                {
                    throw new ArgumentException("Se requiere teléfono");
                }

                if (await _doUsuario.ExistsByEmail(model.Email))
                {
                    throw new ArgumentException("Correo ya fue registrado");
                }

                var usuario = new Tusuario
                {
                    Capellidos = model.Apellidos,
                    Ccodigo = "USU" + (await _doUsuario.GetLastId() + 1).ToString().PadLeft(7, '0'),
                    Cemail = model.Email,
                    Cestado = "A", //ACTIVO
                    Cguid = Guid.NewGuid().ToString("N"),
                    Cnombres = model.Nombres,
                    Cnumdocum = model.NumDocum,
                    Cnumero1 = model.Telefono,
                    Cusername = model.Username,
                    Cusucrea = "ADMIN",
                    Dfeccrea = DateTime.Now,
                    Ntipdocum = model.TipoDocum.Value,
                    Ypassword = EncryptionHelper.Encrypt(_appSettings.EncryptionSeed, model.Password)
                };

                if (!await _doUsuario.Save(usuario))
                {
                    throw new Exception("Ocurrió un error al grabar usuario");
                }

                var result = await Get(usuario.Ccodigo);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Usuario> Update(Usuario model)
        {
            try
            {
                model.Codigo = ConvertHelper.ToNonNullString(model.Codigo);

                if (model == null)
                {
                    throw new ArgumentException("Se requieren parámetros");
                }

                if (model.Codigo == string.Empty)
                {
                    throw new ArgumentException("Se requiere código de usuario");
                }

                var usuario = await _doUsuario.GetByCodigo(model.Codigo);

                if (usuario == null)
                {
                    throw new ArgumentException("Código de usuario no existe");
                }

                if (model.Apellidos != null)
                {
                    usuario.Capellidos = ConvertHelper.ToNonNullString(model.Apellidos);
                }

                if (model.Email != null)
                {
                    usuario.Cemail = ConvertHelper.ToNonNullString(model.Email);
                }

                if (model.Nombres != null)
                {
                    usuario.Cnombres = ConvertHelper.ToNonNullString(model.Nombres);
                }

                if (model.Telefono != null)
                {
                    usuario.Cnumero1 = ConvertHelper.ToNonNullString(model.Telefono);
                }

                if (model.Password != null)
                {
                    usuario.Ypassword = EncryptionHelper.Encrypt(_appSettings.EncryptionSeed, model.Password);
                }

                usuario.Cusumodi = "ADMIN";
                usuario.Dfecmodi = DateTime.Now;

                if (!await _doUsuario.Update(usuario))
                {
                    throw new Exception("Ocurrió un error al grabar usuario");
                }

                var result = await Get(usuario.Ccodigo);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}