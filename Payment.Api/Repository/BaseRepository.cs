using tech_test_payment_api.Payment.Api.Context;
using tech_test_payment_api.Payment.Api.Repository.Interfaces;

namespace tech_test_payment_api.Payment.Api.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly VendaContext _context;

        public BaseRepository(VendaContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        
    }
}