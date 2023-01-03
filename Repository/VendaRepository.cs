using tech_test_payment_api.Context;
using tech_test_payment_api.Models;
using tech_test_payment_api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace tech_test_payment_api.Repository
{
    public class VendaRepository : BaseRepository, IVendaRepository
    {
        private readonly VendaContext _context;

        public VendaRepository(VendaContext context) : base(context)
        {
            _context = context;
        }

        // public IEnumerable<Venda> Get()
        // {
        //     return _context.Vendas.ToList();
        // }

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