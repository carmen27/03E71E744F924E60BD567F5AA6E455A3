using BackEnd.Interface.Data;
using BackEnd.Model;
using Dapper;
using System.Data;

namespace BackEnd.Data
{
    public class DoProducto : IDoProducto
    {
        private readonly IConnectionFactory _connectionFactory;

        public DoProducto(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> ExistsByCodigo(string codigo)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("ccodigo", codigo);
                var query = "SELECT TOP 1 1 WHERE ccodigo = @ccodigo AND cestado != 'N'";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteScalarAsync<bool>(query, parameters, null, null, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Tproducto?> GetByCodigo(string codigo)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("codigo", codigo);
                var query = "SELECT TOP 1 nid, ccodigo FROM tproducto WHERE ccodigo = @codigo";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.QueryFirstOrDefaultAsync<Tproducto>(query, parameters, null, null, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetLastId()
        {
            try
            {
                var query = "SELECT ISNULL(MAX(nid), 0) FROM tproducto";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteScalarAsync<int>(query);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Save(Tproducto producto)
        {
            try
            {
                var query = "INSERT INTO tproducto (cguid, ccodigo, cdescripcion, cmarca, cunidades, nprecio, cestado, cusucrea, dfeccrea)";
                query += " VALUES (@cguid, @ccodigo, @cdescripcion, @cmarca, @cunidades, @nprecio, @cestado, @cusucrea, @dfeccrea);";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteAsync(query, producto);
                return result == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Update(Tproducto producto)
        {
            try
            {
                var query = "UPDATE tproducto SET";
                query += " cdescripcion = @cdescripcion,";
                query += " nprecio = @nprecio,";
                query += " cestado = @cestado,";
                query += " cusumodi = @cusumodi,";
                query += " dfecmodi = @dfecmodi";
                query += " WHERE nid = @nid;";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteAsync(query, producto);
                return result == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}