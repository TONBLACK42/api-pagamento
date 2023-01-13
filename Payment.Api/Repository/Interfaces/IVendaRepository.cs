using tech_test_payment_api.Models;

namespace tech_test_payment_api.Repository.Interfaces
{
    public interface IVendaRepository : IBaseRepository
    {
       Task<Venda> GetByIdAsync(int id);
    }
}