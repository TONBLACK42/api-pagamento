namespace tech_test_payment_api.Payment.Api.Repository.Interfaces
{
    public interface IBaseRepository
    {
        public void Add<T>(T entity) where T: class;
        public void Update<T>(T entity) where T: class;
        Task<bool> SaveChangesAsync();
    }
}