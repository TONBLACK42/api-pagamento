using Microsoft.EntityFrameworkCore;
using api_pagamento.Pagamento.Api.Models;

namespace api_pagamento.Pagamento.Api.Context
{
    //O contexto de banco de dados é a classe principal que coordena a 
    //funcionalidade do Entity Framework para um modelo de dados. 
    //Essa classe é criada derivando-a da classe Microsoft.EntityFrameworkCore.DbContext.
    public class VendaContext : DbContext
    {
        public VendaContext(DbContextOptions<VendaContext> options)
            : base(options)
        {   
        }
        
        //Representa a Tabela de Vendas no BD.
        public DbSet<Venda> Vendas {get; set;} = null!;

        //Representa a Tabela de Vendedores no BD.
        public DbSet<Vendedor> Vendedores {get; set;} = null!;

        //Representa a Tabela de Produtos no BD.
        public DbSet<Produto> Produtos {get; set;} = null!;

        //Representa a Tabela de Itens da Venda no BD.
        public DbSet<ItemDaVenda> ItensDaVenda {get; set;} = null!;


    }
}