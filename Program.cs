// No ASP.NET Core, serviços como o contexto de BD precisam ser registrados
// no contêiner de DI (injeção de dependência). 
// O contêiner fornece o serviço aos controladores.

using Microsoft.EntityFrameworkCore; //Adicionei para Registro do Contexto
using tech_test_payment_api.Models; //Adicionei para Registro do Contexto

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Adiciona o contexto de banco de dados ao contêiner de DI.
//Especifica que o contexto de banco de dados usará um banco de dados 
//em memória.
//A String passada na Option é o nome que dei ao BD em Memória.
//Serviço para Venda.
builder.Services.AddDbContext<VendaContext>(opt =>
    opt.UseInMemoryDatabase("TechTestPaymentApi"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
