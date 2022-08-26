using BackEnd.Interface.Data;
using BackEnd.Model;
using Dapper;
using System.Data;

namespace BackEnd.Data
{
    public class DoUsuario : IDoUsuario
    {
        private readonly IConnectionFactory _connectionFactory;

        public DoUsuario(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Tusuario?> GetByCodigo(string codigo)
        {
            try
            {
                using var connection = _connectionFactory.GetConnection();
                var query = "SELECT TOP 1 nid, ccodigo, cnombre FROM tusuario WHERE ccodigo = @codigo";
                var parameters = new DynamicParameters();
                parameters.Add("codigo", codigo);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<Tusuario>(connection, query, parameters, null, null, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Tusuario?> GetById(int id)
        {
            try
            {
                using var connection = _connectionFactory.GetConnection();
                var query = "SELECT nid, ccodigo, cnombre FROM tusuario WHERE nid = @nid LIMIT 1";
                var parameters = new DynamicParameters();
                parameters.Add("nid", id);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<Tusuario>(connection, query, parameters, null, null, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}