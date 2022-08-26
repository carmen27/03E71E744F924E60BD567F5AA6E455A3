using BackEnd.Entity;

namespace BackEnd.Interface.Business
{
    public interface IBoUsuario
    {
        Task<Usuario?> Get(string codigo);
    }
}