using BackEnd.Model;

namespace BackEnd.Interface.Data
{
    public interface IDoCompra
    {
        void CancelarTransaccion();

        Task<bool> ClearDetalles(int nid);

        //Task<bool> Delete(Tcompra compra);

        Task<bool> DeleteDetalle(object detalle);

        bool EnTransaccion();

        void FinalizarTransaccion();

        Task<Tcompra?> GetByCodigo(string codigo);

        Task<Tcompradet?> GetDetalle(string cguid, string codProducto);

        void IniciarTransaccion();

        Task<IEnumerable<Tcompradet>> ListDetalles(int compraId);

        Task<bool> Save(Tcompra compra);

        Task<bool> SaveDetalle(Tcompradet compraDet);

        Task<bool> Update(Tcompra compra);

        Task<bool> UpdateDetalle(Tcompradet compraDet);
    }
}