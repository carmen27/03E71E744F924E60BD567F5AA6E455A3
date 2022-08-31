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

        public Task<bool> ClearDetalles(int nid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Tcompra compra)
        {
            throw new NotImplementedException();
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
    }
}