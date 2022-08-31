using BackEnd.Interface.Data;
using BackEnd.Model;
using Dapper;
using System.Data;

namespace BackEnd.Data
{
    public class DoCompra : IDoCompra
    {
        private readonly IConnectionFactory _connectionFactory;
        private IDbTransaction? _transaction;

        public DoCompra(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void CancelarTransaccion()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteDetById(int id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("nid", id);
                var query = "DELETE FROM tcompradet";
                query += " WHERE nid = @nid;";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteAsync(query, parameters, _transaction);
                return result == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool EnTransaccion() => _transaction != null;

        public void FinalizarTransaccion()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tcompra?> GetByCodigo(string codigo)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("codigo", codigo);
                var query = "SELECT TOP 1 nid, ccodigo FROM tcompra WHERE ccodigo = @codigo";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.QueryFirstOrDefaultAsync<Tcompra>(query, parameters, null, null, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Tcompradet?> GetDetalle(int compraId, string codProducto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("ncompraid", compraId);
                parameters.Add("ccodigo", codProducto);
                var query = "SELECT TOP 1 nid, ccodigo FROM tcompradet WHERE ncompraid = @ncompraid && ccodigo = @ccodigo";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.QueryFirstOrDefaultAsync<Tcompradet>(query, parameters, null, null, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void IniciarTransaccion()
        {
            try
            {
                using var connection = _connectionFactory.GetConnection();
                _transaction = connection.BeginTransaction();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Tcompradet>> ListDetalles(int compraId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("ncompraid", compraId);
                var query = "SELECT nid, ccodigo FROM tcompradet WHERE ncompraid = @ncompraid";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.QueryAsync<Tcompradet>(query, parameters, null, null, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Save(Tcompra compra)
        {
            try
            {
                var query = "INSERT INTO tcompra (cguid, ccodigo, ctipo, dfecha, ccliruc, cclirazon, nvaligv,";
                query += " ntotaligv, nimport, nimportigv, cmoneda, cobserv, cestado, cusucrea, dfeccrea)";
                query += " VALUES (@cguid, @ccodigo, @ctipo, @dfecha, @ccliruc, @cclirazon, @nvaligv, @ntotaligv,";
                query += " @nimport, @nimportigv, @cmoneda, @cobserv, @cestado, @cusucrea, @dfeccrea);";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteAsync(query, compra, _transaction);
                return result == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SaveDetalle(Tcompradet compraDet)
        {
            try
            {
                var query = "INSERT INTO tcompradet (cprodcod, cproddesc, cprodmarca, cprodunid, nprecio, ncantidad, nimport, cestado, ncompraid, cusucrea, dfeccrea)";
                query += " VALUES (@cprodcod, @cproddesc, @cprodmarca, @cprodunid, @nprecio, @ncantidad, @nimport, @cestado, @ncompraid, @cusucrea, @dfeccrea);";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteAsync(query, compraDet, _transaction);
                return result == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Update(Tcompra compra)
        {
            try
            {
                var query = "UPDATE tcompra SET";
                query += " nvaligv = @nvaligv,";
                query += " ntotaligv = @ntotaligv,";
                query += " nimport = @nimport,";
                query += " nimportigv = @nimportigv,";
                query += " cmoneda = @cmoneda,";
                query += " cobserv = @cobserv,";
                query += " cestado = @cestado,";
                query += " cusumodi = @cusumodi,";
                query += " dfecmodi = @dfecmodi";
                query += " WHERE nid = @nid;";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteAsync(query, compra, _transaction);
                return result == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateDetalle(Tcompradet compraDet)
        {
            try
            {
                var query = "UPDATE tcompradet SET";
                query += " nprecio = @nprecio,";
                query += " ncantidad = @ncantidad,";
                query += " nimport = @nimport,";
                query += " cestado = @cestado";
                query += " cusumodi = @cusumodi,";
                query += " dfecmodi = @dfecmodi";
                query += " WHERE nid = @nid;";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteAsync(query, compraDet, _transaction);
                return result == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}