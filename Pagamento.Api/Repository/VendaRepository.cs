using api_pagamento.Pagamento.Api.Context;
using api_pagamento.Pagamento.Api.Models;
using api_pagamento.Pagamento.Api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_pagamento.Pagamento.Api.Repository
{
    public class VendaRepository : BaseRepository, IVendaRepository
    {
        private readonly VendaContext _context;

        public VendaRepository(VendaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Venda> GetByIdAsync(int id)
        {
            return await _context.Vendas
                    .Where(v => v.Id == id)
                    .Include(vendedor => vendedor.Vendedores)
                    .Include("ItensDaVenda.Produto")
                    .FirstOrDefaultAsync();
        }
    }
}