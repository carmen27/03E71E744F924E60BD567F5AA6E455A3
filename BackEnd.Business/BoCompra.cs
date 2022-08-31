using BackEnd.Entity;
using BackEnd.Interface.Business;
using BackEnd.Interface.Data;
using BackEnd.Model;
using BackEnd.Toolkit;
using Microsoft.Extensions.Logging;

namespace BackEnd.Business
{
    public class BoCompra : IBoCompra
    {
        private readonly IDoCompra _doCompra;
        private readonly IDoProducto _doProducto;
        private readonly IDoUsuario _doUsuario;

        private readonly ILogger<BoCompra> _logger;

        public BoCompra(ILogger<BoCompra> logger,
                         IDoCompra doCompra,
                         IDoProducto doProducto,
                         IDoUsuario doUsuario)
        {
            _logger = logger;
            _doCompra = doCompra;
            _doProducto = doProducto;
            _doUsuario = doUsuario;
        }

        public async Task Delete(Compra model)
        {
            try
            {
                model.Codigo = ConvertHelper.ToNonNullString(model.Codigo).ToUpper();

                var compra = await _doCompra.GetByCodigo(model.Codigo);

                if (compra == null)
                {
                    throw new Exception("Compra no existe");
                }

                if (compra.Cestado != "R")
                {
                    throw new Exception("Solo es posible eliminar compras en estado REGISTRADO");
                }

                compra.Cestado = "N";

                await _doCompra.Update(compra);
            }
            catch (Exception ex)
            {
                if (_doCompra.EnTransaccion())
                {
                    _doCompra.CancelarTransaccion();
                }
                _logger.LogError($"Ocurrió un error: {ex}");
                throw;
            }
        }

        public async Task<Compra?> Get(string codigo)
        {
            try
            {
                var result = await _doCompra.GetByCodigo(codigo);
                if (result != null)
                {
                    var compra = new Compra()
                    {
                        Codigo = result.Ccodigo,
                        RucCliente = result.Ccliruc,
                        RazonCliente = result.Cclirazon,
                        TotalIgv = result.Ntotaligv,
                        ImporteSinIgv = result.Nimport,
                        ImporteConIgv = result.Nimportigv
                    };

                    compra.Detalles = (await _doCompra.ListDetalles(result.Nid)).Select(x => new CompraDetalle()
                    {
                        CodProducto = x.Cprodcod,
                        DescProducto = x.Cproddesc,
                        Marca = x.Cprodmarca,
                        Cantidad = x.Ncantidad,
                        Precio = x.Nprecio,
                        Total = x.Nimport
                    }).ToList();

                    return compra;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Compra> Save(Compra model)
        {
            try
            {
                model.RucCliente = ConvertHelper.ToNonNullString(model.RucCliente).ToUpper();
                model.Moneda = ConvertHelper.ToNonNullString(model.Moneda).ToUpper();

                if (model.Moneda == string.Empty)
                {
                    throw new Exception("Código de moneda inválido");
                }

                if (model.RucCliente == string.Empty)
                {
                    throw new Exception("RUC de cliente inválido");
                }

                var cliente = await _doUsuario.GetByDocum(model.RucCliente);

                if (cliente == null)
                {
                    throw new Exception("Cliente no existe");
                }

                if (model.Detalles == null || model.Detalles.Count == 0)
                {
                    throw new Exception("Compra no contiene detalles");
                }

                var cabecTotal = 0M;
                var cabecTotalIgv = 0M;

                foreach (var item in model.Detalles)
                {
                    item.Total = item.Cantidad * item.Precio;
                    cabecTotal += item.Total ?? 0M;
                    cabecTotalIgv += (item.Total ?? 0M) * (model.TasaIgv ?? 0M);
                }

                model.Detalles = model.Detalles.Where(x => x.Total != 0M).ToList();

                if (cabecTotal == 0M)
                {
                    throw new Exception("Total no puede ser 0");
                }

                var compra = new Tcompra()
                {
                    Cguid = Guid.NewGuid().ToString("N"),
                    Ccodigo = "COM",
                    Ctipo = "C",
                    Dfecha = DateTime.Now,
                    Ccliruc = cliente.Cnumdocum,
                    Cclirazon = $"{cliente.Capellidos}, {cliente.Cnombres}",
                    Nvaligv = model.TasaIgv ?? 0M,
                    Ntotaligv = cabecTotalIgv,
                    Nimport = cabecTotal,
                    Nimportigv = cabecTotal + cabecTotalIgv,
                    Cmoneda = model.Moneda,
                    Cestado = "R",
                    Cusucrea = "ADMIN",
                    Dfeccrea = DateTime.Now,
                };

                _doCompra.IniciarTransaccion();

                if (!await _doCompra.Save(compra))
                {
                    throw new Exception("Ocurrió un error al registar compra");
                }

                foreach (var item in model.Detalles)
                {
                    if (string.IsNullOrEmpty(item.CodProducto))
                    {
                        throw new Exception("Código de producto inválido");
                    }

                    var articulo = await _doProducto.GetByCodigo(item.CodProducto);

                    if (articulo == null)
                    {
                        throw new Exception("Código de producto inválido");
                    }

                    var compraDet = new Tcompradet()
                    {
                        Cprodcod = item.CodProducto,
                        Cproddesc = articulo.Cdescripcion,
                        Cprodmarca = articulo.Cmarca,
                        Cprodunid = articulo.Cunidades,
                        Nprecio = item.Precio ?? 0M,
                        Ncantidad = item.Cantidad ?? 0M,
                        Nimport = item.Total ?? 0M,
                        Cestado = "R",
                        Ncompraid = compra.Nid,
                        Cusucrea = "ADMIN",
                        Dfeccrea = DateTime.Now
                    };

                    if (!await _doCompra.SaveDetalle(compraDet))
                    {
                        throw new Exception("Ocurrió un error al registar detalle de compra");
                    }
                }

                _doCompra.FinalizarTransaccion();

                var result = await Get(compra.Ccodigo);
                return result;
            }
            catch (Exception ex)
            {
                if (_doCompra.EnTransaccion())
                {
                    _doCompra.CancelarTransaccion();
                }
                _logger.LogError($"Ocurrió un error: {ex}");
                throw;
            }
        }

        public async Task<Compra> Update(Compra model)
        {
            try
            {

                model.Codigo = ConvertHelper.ToNonNullString(model.Codigo);

                var compra = await _doCompra.GetByCodigo(model.Codigo);

                if (compra == null)
                {
                    throw new Exception("Compra no existe");
                }

                if (model.Detalles == null || model.Detalles.Count == 0)
                {
                    throw new Exception("Compra no contiene detalles");
                }

                var cabecTotal = 0M;
                var cabecTotalIgv = 0M;

                foreach (var item in model.Detalles)
                {
                    item.Total = item.Cantidad * item.Precio;
                    cabecTotal += item.Total ?? 0M;
                    cabecTotalIgv += (item.Total ?? 0M) * (model.TasaIgv ?? 0M);
                }

                model.Detalles = model.Detalles.Where(x => x.Total != 0M).ToList();

                if (cabecTotal == 0M)
                {
                    throw new Exception("Total no puede ser 0");
                }

                if (model.Moneda != null)
                {
                    compra.Cmoneda = ConvertHelper.ToNonNullString(model.Moneda);
                }

                compra.Ntotaligv = cabecTotalIgv;
                compra.Nimport = cabecTotal;
                compra.Nimportigv = cabecTotal + cabecTotalIgv;
                compra.Cusumodi = "ADMIN";
                compra.Dfecmodi = DateTime.Now;

                _doCompra.IniciarTransaccion();

                if (!await _doCompra.Update(compra))
                {
                    throw new Exception("Ocurrió un error al registar compra");
                }

                foreach (var item in model.Detalles)
                {
                    if (string.IsNullOrEmpty(item.CodProducto))
                    {
                        throw new Exception("Código de producto inválido");
                    }

                    var producto = await _doProducto.GetByCodigo(item.CodProducto);

                    if (producto == null)
                    {
                        throw new Exception("Código de producto inválido");
                    }

                    var compraDet = await _doCompra.GetDetalle(compra.Nid, item.CodProducto);

                    if (compraDet == null)
                    {
                        compraDet = new Tcompradet()
                        {
                            Cprodcod = item.CodProducto,
                            Cproddesc = producto.Cdescripcion,
                            Cprodmarca = producto.Cmarca,
                            Cprodunid = producto.Cunidades,
                            Nprecio = item.Precio ?? 0M,
                            Ncantidad = item.Cantidad ?? 0M,
                            Nimport = item.Total ?? 0M,
                            Cestado = "R",
                            Ncompraid = compra.Nid,
                            Cusucrea = "ADMIN",
                            Dfeccrea = DateTime.Now
                        };

                        if (!await _doCompra.SaveDetalle(compraDet))
                        {
                            throw new Exception("Ocurrió un error al registar detalle de compra");
                        }
                    }
                    else
                    {
                        compraDet.Nprecio = item.Precio ?? 0M;
                        compraDet.Ncantidad = item.Cantidad ?? 0M;
                        compraDet.Nimport = item.Total ?? 0M;
                        compraDet.Cusumodi = "ADMIN";
                        compraDet.Dfecmodi = DateTime.Now;

                        if (!await _doCompra.UpdateDetalle(compraDet))
                        {
                            throw new Exception("Ocurrió un error al actualizar detalle de compra");
                        }
                    }
                }

                var detallesBD = await _doCompra.ListDetalles(compra.Nid);

                foreach (var detalle in detallesBD)
                {
                    var compraDet = model.Detalles.FirstOrDefault(x => x.CodProducto == detalle.Cprodcod);

                    if (compraDet == null)
                    {
                        if (!await _doCompra.DeleteDetById(detalle.Nid))
                        {
                            throw new Exception("Ocurrió un error al eliminar detalle de compra");
                        }
                    }
                }

                _doCompra.FinalizarTransaccion();

                var result = await Get(compra.Ccodigo);
                return result;
            }
            catch (Exception ex)
            {
                if (_doCompra.EnTransaccion())
                {
                    _doCompra.CancelarTransaccion();
                }
                _logger.LogError($"Ocurrió un error: {ex}");
                throw;
            }
        }
    }
}