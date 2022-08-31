using BackEnd.Model;

namespace BackEnd.Interface.Data
{
    public interface IDoUsuario
    {
        Task<bool> ExistsByEmail(string email);

        Task<Tusuario?> GetByCodigo(string codigo);

        Task<Tusuario?> GetByDocum(string numDocum);

        Task<Tusuario?> GetById(int id);

        Task<int> GetLastId();

        Task<bool> Save(Tusuario usuario);

        Task<bool> Update(Tusuario usuario);
    }
}