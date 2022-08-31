using BackEnd.Entity;
using BackEnd.Interface.Business;
using BackEnd.Interface.Data;
using BackEnd.Model;
using BackEnd.Toolkit;
using Microsoft.Extensions.Logging;

namespace BackEnd.Business
{
    public class BoProducto : IBoProducto
    {
        private readonly IDoProducto _doProducto;
        private readonly IDoUsuario _doUsuario;

        private readonly ILogger<BoProducto> _logger;

        public BoProducto(ILogger<BoProducto> logger,
                         IDoProducto doArticulo,
                         IDoUsuario doUsuario)
        {
            _logger = logger;
            _doProducto = doArticulo;
            _doUsuario = doUsuario;
        }

        public async Task Delete(Producto model)
        {
            try
            {
                model.Codigo = ConvertHelper.ToNonNullString(model.Codigo).ToUpper();

                if (model.Codigo == string.Empty)
                {
                    throw new Exception("Código de producto inválido");
                }

                var producto = await _doProducto.GetByCodigo(model.Codigo);

                if (producto == null)
                {
                    throw new Exception("Producto no registrado");
                }

                producto.Cestado = "N";
                producto.Cusumodi = "ADMIN";
                producto.Dfecmodi = DateTime.Now;

                if (!await _doProducto.Update(producto))
                {
                    throw new Exception("Ocurrió un error al eliminar producto");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error: {ex}");
                throw;
            }
        }

        public async Task<Producto?> Get(string codigo)
        {
            try
            {
                var result = await _doProducto.GetByCodigo(codigo);

                if (result != null)
                {
                    return new Producto()
                    {
                        Codigo = result.Ccodigo,
                        Descripcion = result.Cdescripcion,
                        Marca = result.Cmarca,
                        Precio = result.Nprecio,
                        Unidades = result.Cunidades
                    };
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Producto> Save(Producto model)
        {
            try
            {
                model.Codigo = ConvertHelper.ToNonNullString(model.Codigo).ToUpper();
                model.Descripcion = ConvertHelper.ToNonNullString(model.Descripcion).ToUpper();
                model.Marca = ConvertHelper.ToNonNullString(model.Marca).ToUpper();
                model.Unidades = ConvertHelper.ToNonNullString(model.Unidades).ToUpper();

                if (model.Codigo == string.Empty)
                {
                    var lastId = await _doProducto.GetLastId();

                    if (lastId == -1)
                    {
                        throw new Exception("Ocurrió un error al obtener último id");
                    }

                    model.Codigo = $"PRD{(lastId + 1).ToString().PadLeft(7, '0')}";
                }
                else
                {
                    if (model.Codigo.Length > 10)
                    {
                        throw new Exception("Longitud de código debe ser diez o menos");
                    }
                }

                if (await _doProducto.ExistsByCodigo(model.Codigo))
                {
                    throw new Exception("Producto ya fue registrado");
                }

                if (model.Descripcion == string.Empty)
                {
                    throw new Exception("Se requiere descripción de producto");
                }

                var producto = new Tproducto()
                {
                    Cguid = Guid.NewGuid().ToString("N"),
                    Ccodigo = model.Codigo,
                    Cdescripcion = model.Descripcion,
                    Cmarca = model.Marca,
                    Cunidades = model.Unidades,
                    Nprecio = model.Precio ?? 1M,
                    Cestado = "A",
                    Cusucrea = "ADMIN",
                    Dfeccrea = DateTime.Now
                };

                if (!await _doProducto.Save(producto))
                {
                    throw new Exception("Ocurrió un error al registar producto");
                }

                var result = await Get(producto.Ccodigo);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error: {ex}");
                throw;
            }
        }

        public async Task<Producto> Update(Producto model)
        {
            try
            {
                model.Codigo = ConvertHelper.ToNonNullString(model.Codigo).ToUpper();

                if (model.Codigo == string.Empty)
                {
                    throw new Exception("Código de producto inválido");
                }

                var producto = await _doProducto.GetByCodigo(model.Codigo);

                if (producto == null)
                {
                    throw new Exception("producto no registrado");
                }

                if (model.Descripcion != null)
                {
                    producto.Cdescripcion = ConvertHelper.ToNonNullString(model.Descripcion).ToUpper();
                }

                if (model.Marca != null)
                {
                    producto.Cmarca = ConvertHelper.ToNonNullString(model.Marca).ToUpper();
                }

                if (model.Unidades != null)
                {
                    producto.Cunidades = ConvertHelper.ToNonNullString(model.Unidades).ToUpper();
                }

                if (model.Precio != null)
                {
                    producto.Nprecio = model.Precio ?? 0M;
                }

                producto.Cusumodi = "ADMIN";
                producto.Dfecmodi = DateTime.Now;

                if (!await _doProducto.Update(producto))
                {
                    throw new Exception("Ocurrió un error al actualizar producto");
                }

                var result = await Get(producto.Ccodigo);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error: {ex}");
                throw;
            }
        }
    }
}