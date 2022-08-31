using BackEnd.Entity;

namespace BackEnd.Interface.Business
{
    public interface IBoCompra
    {
        Task Delete(Compra model);

  

        Task<Compra?> Get(string codigo);

        Task<Compra> Save(Compra model);

        Task<Compra> Update(Compra model);
    }
}