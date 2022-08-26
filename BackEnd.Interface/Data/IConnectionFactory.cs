using System.Data;

namespace BackEnd.Interface.Data
{
    public interface IConnectionFactory
    {
        public IDbConnection GetConnection();
    }
}