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

        public async Task<bool> ExistsByEmail(string email)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("cemail", email);
                var query = "SELECT TOP 1 1 WHERE cemail = @cemail AND cestado != 'N'";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteScalarAsync<bool>(query, parameters, null, null, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Tusuario?> GetByCodigo(string codigo)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("codigo", codigo);
                var query = "SELECT TOP 1 nid, ccodigo, cnombre FROM tusuario WHERE ccodigo = @codigo";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.QueryFirstOrDefaultAsync<Tusuario>(query, parameters, null, null, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Tusuario?> GetByDocum(string numDocum)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("cnumdocum", numDocum);
                var query = "SELECT TOP 1 nid, ccodigo, cnombre FROM tusuario WHERE cnumdocum = @cnumdocum";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.QueryFirstOrDefaultAsync<Tusuario>(query, parameters, null, null, CommandType.Text);
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
                var parameters = new DynamicParameters();
                parameters.Add("nid", id);
                var query = "SELECT TOP 1 nid, ccodigo, cnombre FROM tusuario WHERE nid = @nid";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.QueryFirstOrDefaultAsync<Tusuario>(query, parameters, null, null, CommandType.Text);
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
                var query = "SELECT ISNULL(MAX(nid), 0) FROM tusuario";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteScalarAsync<int>(query);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Save(Tusuario usuario)
        {
            try
            {
                var query = "INSERT INTO tusuario (cguid, ccodigo, cnombres, capellidos, cusername, ypassword, ntipdocum,";
                query += " cnumdocum, cemail, cnumero1, cnumero2, cnumero3, cestado, cusucrea, dfeccrea, cusumodi, dfecmodi)";
                query += " VALUES (@cguid, @ccodigo, @cnombres, @capellidos, @cusername, @ypassword, @ntipdocum, @cnumdocum,";
                query += " @cemail, @cnumero1, @cnumero2, @cnumero3, @cestado, @cusucrea, @dfeccrea, @cusumodi, @dfecmodi);";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteAsync(query, usuario);
                return result == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Update(Tusuario usuario)
        {
            try
            {
                var query = "UPDATE tusuario SET";
                query += " cnombres = @cnombres,";
                query += " capellidos = @capellidos,";
                query += " ypassword = @ypassword,";
                query += " cemail = @cemail,";
                query += " cnumero1 = @cnumero1,";
                query += " cestado = @cestado,";
                query += " cusumodi = @cusumodi,";
                query += " dfecmodi = @dfecmodi";
                query += " WHERE nid = @nid;";
                using var connection = _connectionFactory.GetConnection();
                var result = await connection.ExecuteAsync(query, usuario);
                return result == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}