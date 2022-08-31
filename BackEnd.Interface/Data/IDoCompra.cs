using BackEnd.Model;

namespace BackEnd.Interface.Data
{
    public interface IDoCompra
    {
        void CancelarTransaccion();

        Task<bool> ClearDetalles(int nid);

        Task<bool> Delete(Tcompra compra);

        Task<bool> DeleteDetalle(object detalle);

        bool EnTransaccion();

        void FinalizarTransaccion();

        Task<Tcompra?> GetByCodigo(string codigo);

        Task<Tcompradet?> GetDetalle(string cguid, string codProducto);

        void IniciarTransaccion();

        Task<List<Tcompradet>> ListDetalles(string cguid);

        Task<bool> Save(Tcompra compra);

        Task<bool> SaveDetalle(Tcompradet newFacturaDet);

        Task<bool> Update(Tcompra compra);

        Task<bool> UpdateDetalle(object facturaDet);
    }
}