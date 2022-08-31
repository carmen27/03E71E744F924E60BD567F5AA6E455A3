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

                _doCompra.IniciarTransaccion();

                if (!await _doCompra.ClearDetalles(compra.Nid))
                {
                    throw new Exception("Ocurrió un error al eliminar factura");
                }

                if (!await _doCompra.Delete(compra))
                {
                    throw new Exception("Ocurrió un error al eliminar factura");
                }

                _doCompra.FinalizarTransaccion();
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
                    return new Compra()
                    {

                    };
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
                model.Tipo = ConvertHelper.ToNonNullString(model.Tipo).ToUpper();
                model.RucCliente = ConvertHelper.ToNonNullString(model.RucCliente).ToUpper();
                model.Moneda = ConvertHelper.ToNonNullString(model.Moneda).ToUpper();
                model.Fecha = ConvertHelper.ToNonNullString(model.Fecha).ToUpper();

                if (model.Tipo == string.Empty || (model.Tipo != "01" && model.Tipo != "02"))
                {
                    throw new Exception("Tipo de factura inválida");
                }

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

                var fechaDocumento = ConvertHelper.ToNullDateTimeWithFormat(model.Fecha, "dd/MM/yyyy");

                if (fechaDocumento == null)
                {
                    throw new Exception("Fecha tiene formato inválido");
                }

                if (model.Detalles == null || model.Detalles.Count == 0)
                {
                    throw new Exception("Factura no contiene detalles");
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
                    throw new Exception("Total a facturar no puede ser 0");
                }

                var compra = new Tcompra()
                {
                    Cguid = Guid.NewGuid().ToString("N"),
                    Ccodigo = "COM",
                    Ctipo = model.Tipo,
                    Dfecha = fechaDocumento.Value,
                    Ccliruc = cliente.Cnumdocum,
                    Cclirazon = $"{cliente.Capellidos}, {cliente.Cnombres}",
                    Nvaligv = model.TasaIgv ?? 0M,
                    Ntotaligv = cabecTotalIgv,
                    Nimport = cabecTotal,
                    Nimportigv = cabecTotal + cabecTotalIgv,
                    Cmoneda = model.Moneda,
                    Cestado = "R",
                    Cusucrea = "ADMIN",
                    Dfeccrea = DateTime.Now
                };

                _doCompra.IniciarTransaccion();

                if (!await _doCompra.Save(compra))
                {
                    throw new Exception("Ocurrió un error al registar factura");
                }

                foreach (var item in model.Detalles)
                {
                    if (string.IsNullOrEmpty(item.CodArticulo))
                    {
                        throw new Exception("Código de artículo inválido");
                    }

                    var articulo = await _doProducto.GetByCodigo(item.CodArticulo);

                    if (articulo == null)
                    {
                        throw new Exception("Código de artículo inválido");
                    }

                    var newFacturaDet = new Tcompradet()
                    {
                        Cprodcod = item.CodArticulo,
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

                    if (!await _doCompra.SaveDetalle(newFacturaDet))
                    {
                        throw new Exception("Ocurrió un error al registar detalle de factura");
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
                //model.Tipo = ConvertHelper.ToNonNullString(model.Tipo).ToUpper();
                //model.RucCliente = ConvertHelper.ToNonNullString(model.RucCliente).ToUpper();
                //model.Moneda = ConvertHelper.ToNonNullString(model.Moneda).ToUpper();
                //model.Glosa1 = ConvertHelper.ToNonNullString(model.Glosa1).ToUpper();
                //model.Fecha = ConvertHelper.ToNonNullString(model.Fecha).ToUpper();

                //if (model.Tipo == string.Empty || (model.Tipo != "01" && model.Tipo != "02"))
                //{
                //    throw new Exception("Tipo de factura inválida");
                //}

                //if (model.NumSerie == string.Empty)
                //{
                //    throw new Exception("Número de serie inválido");
                //}

                //if (model.NumDocumento == string.Empty)
                //{
                //    throw new Exception("Número de documento inválido");
                //}

                model.Codigo = ConvertHelper.ToNonNullString(model.Codigo);

                var compra = await _doCompra.GetByCodigo(model.Codigo);

                if (compra == null)
                {
                    throw new Exception("Factura no existe");
                }

                if (model.Moneda == string.Empty)
                {
                    throw new Exception("Código de moneda inválido");
                }

                if (model.RucCliente == string.Empty)
                {
                    throw new Exception("RUC de cliente inválido");
                }

                //var cliente = await _doCliente.Get(model.RucCliente);

                //if (cliente == null)
                //{
                //    throw new Exception("Cliente no existe");
                //}

                //var fechaDocumento = ConvertHelper.ToNullDateTimeWithFormat(model.Fecha, "dd/MM/yyyy");

                //if (fechaDocumento == null)
                //{
                //    throw new Exception("Fecha tiene formato inválido");
                //}

                if (model.Detalles == null || model.Detalles.Count == 0)
                {
                    throw new Exception("Factura no contiene detalles");
                }

                //var usuario = await _doUsuario.GetByGuid(usuGuid);

                //if (usuario == null)
                //{
                //    throw new Exception("Usuario no existe");
                //}

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
                    throw new Exception("Total a facturar no puede ser 0");
                }

                compra.Cmoneda = model.Moneda;
                compra.Ntotaligv = cabecTotalIgv;
                compra.Nimport = cabecTotal;
                compra.Nimportigv = cabecTotal + cabecTotalIgv;
                compra.Cusumodi = "ADMIN";
                compra.Dfecmodi = DateTime.Now;

                _doCompra.IniciarTransaccion();

                if (!await _doCompra.Update(compra))
                {
                    throw new Exception("Ocurrió un error al registar factura");
                }

                //await _doFactura.ClearDetalles(oldFactura.Nid);

                foreach (var item in model.Detalles)
                {
                    var articulo = await _doProducto.Get(item.CodArticulo);

                    if (articulo == null)
                    {
                        throw new Exception("Códogp de artículo inválido");
                    }

                    var facturaDet = await _doCompra.GetDetalle(compra.Cguid, item.CodArticulo);

                    if (facturaDet == null)
                    {
                        facturaDet = new Tfactdet()
                        {
                            Cartcod = item.CodArticulo,
                            Cartdesc = articulo.Cdescripcion,
                            Cartmarca = articulo.Cmarca,
                            Cartunidades = articulo.Cunidades,
                            Nprecio = item.Precio,
                            Ncantidad = item.Cantidad,
                            Nimport = item.Total,
                            Cestado = "R",
                            Nfacturaid = compra.Nid,
                            Cusucrea = usuario.Ccodigo,
                            Dfeccrea = DateTime.Now
                        };

                        if (!await _doCompra.SaveDetalle(facturaDet))
                        {
                            throw new Exception("Ocurrió un error al registar detalle de factura");
                        }
                    }
                    else
                    {
                        facturaDet.Nprecio = item.Precio;
                        facturaDet.Ncantidad = item.Cantidad;
                        facturaDet.Nimport = item.Total;
                        facturaDet.Cusumodi = usuario.Ccodigo;
                        facturaDet.Dfecmodi = DateTime.Now;

                        if (!await _doCompra.UpdateDetalle(facturaDet))
                        {
                            throw new Exception("Ocurrió un error al actualizar detalle de factura");
                        }
                    }
                }

                var detallesBD = await _doCompra.ListDetalles(compra.Cguid);

                foreach (var detalle in detallesBD)
                {
                    var facturaDet = model.Detalles.FirstOrDefault(x => x.CodArticulo == detalle.Cartcod);

                    if (facturaDet == null)
                    {
                        if (!await _doCompra.DeleteDetalle(detalle))
                        {
                            throw new Exception("Ocurrió un error al eliminar detalle de factura");
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