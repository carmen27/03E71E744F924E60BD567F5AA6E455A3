using BackEnd.Entity;

namespace BackEnd.Interface.Business
{
    public interface IBoUsuario
    {
        Task Delete(Usuario model);

        Task<Usuario?> Get(string codigo);

        Task<Usuario?> Save(Usuario model);

        Task<Usuario?> Update(Usuario model);
    }
}