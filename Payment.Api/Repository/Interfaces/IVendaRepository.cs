using tech_test_payment_api.Payment.Api.Models;

namespace tech_test_payment_api.Payment.Api.Repository.Interfaces
{
    public interface IVendaRepository : IBaseRepository
    {
       Task<Venda> GetByIdAsync(int id);
    }
}