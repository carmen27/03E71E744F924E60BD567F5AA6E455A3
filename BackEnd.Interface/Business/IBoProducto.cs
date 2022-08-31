using BackEnd.Entity;

namespace BackEnd.Interface.Business
{
    public interface IBoProducto
    {
        Task Delete(Producto model);

        Task<Producto?> Get(string codigo);

        Task<Producto> Save(Producto model);

        Task<Producto> Update(Producto model);
    }
}