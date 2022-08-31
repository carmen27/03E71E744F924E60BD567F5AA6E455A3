using BackEnd.Model;

namespace BackEnd.Interface.Data
{
    public interface IDoProducto
    {
        Task<bool> ExistsByCodigo(string codigo);

        Task<Tproducto?> GetByCodigo(string codigo);

        Task<int> GetLastId();

        Task<bool> Save(Tproducto producto);

        Task<bool> Update(Tproducto producto);
    }
}