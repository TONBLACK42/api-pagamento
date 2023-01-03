using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tech_test_payment_api.Models;

namespace tech_test_payment_api.Repository.Interfaces
{
    public interface IVendaRepository : IBaseRepository
    {
        //IEnumerable<Venda> Get();

       Task<Venda> GetByIdAsync(int id);
    }
}