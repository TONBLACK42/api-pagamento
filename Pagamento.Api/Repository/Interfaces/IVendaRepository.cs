using api_pagamento.Pagamento.Api.Models;

namespace api_pagamento.Pagamento.Api.Repository.Interfaces
{
    public interface IVendaRepository : IBaseRepository
    {
       Task<Venda> GetByIdAsync(int id);
    }
}