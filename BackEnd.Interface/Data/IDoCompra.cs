using BackEnd.Model;

namespace BackEnd.Interface.Data
{
    public interface IDoCompra
    {
        void CancelarTransaccion();

        Task<bool> DeleteDetById(int id);

        bool EnTransaccion();

        void FinalizarTransaccion();

        Task<Tcompra?> GetByCodigo(string codigo);

        Task<Tcompradet?> GetDetalle(int compraId, string codProducto);

        void IniciarTransaccion();

        Task<IEnumerable<Tcompradet>> ListDetalles(int compraId);

        Task<bool> Save(Tcompra compra);

        Task<bool> SaveDetalle(Tcompradet compraDet);

        Task<bool> Update(Tcompra compra);

        Task<bool> UpdateDetalle(Tcompradet compraDet);
    }
}