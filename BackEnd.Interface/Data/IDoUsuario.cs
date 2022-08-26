using BackEnd.Model;

namespace BackEnd.Interface.Data
{
    public interface IDoUsuario
    {
        Task<Tusuario?> GetByCodigo(string codigo);

        Task<Tusuario?> GetById(int id);
    }
}