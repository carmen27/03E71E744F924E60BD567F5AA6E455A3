using BackEnd.Model;

namespace BackEnd.Interface.Data
{
    public interface IDoCompra
    {
        void CancelarTransaccion();

        void FinalizarTransaccion();

        Task<Tcompra?> GetByCodigo(string codigo);

        void IniciarTransaccion();

        bool EnTransaccion();
        Task<bool> ClearDetalles(int nid);
        Task<bool> Delete(Tcompra compra);
    }
}