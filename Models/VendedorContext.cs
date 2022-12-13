using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tech_test_payment_api.Models
{

    //O contexto de banco de dados é a classe principal que coordena a 
    //funcionalidade do Entity Framework para um modelo de dados. 
    //Essa classe é criada derivando-a da classe Microsoft.EntityFrameworkCore.DbContext.
    public class VendedorContext : DbContext
    {

        public VendedorContext(DbContextOptions<VendedorContext> options)
            : base(options)
        {   
        }
        
        public DbSet<Vendedor> Vendedores {get; set;} = null!;
    }
}