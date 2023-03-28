// No ASP.NET Core, serviços como o contexto de BD precisam ser registrados
// no contêiner de DI (injeção de dependência). 
// O contêiner fornece o serviço aos controladores.

using Microsoft.EntityFrameworkCore; //Adicionei para Registro do Contexto
using api_pagamento.Pagamento.Api.Context; //Adicionei para Registro do Contexto
using Microsoft.OpenApi.Models; //Para Personalizar e Estender o Swagger.
using System.Text.Json.Serialization;// Para Converter Enum Inteiro em String.
using System.Reflection; // Usa Reflections para Configure o Swagger para usar o arquivo XML
using api_pagamento.Pagamento.Api.Repository.Interfaces;
using api_pagamento.Pagamento.Api.Repository;
using api_pagamento.Pagamento.Api.Helpers;//Permite usar a Classe AddShemaExample para alterar modelo esquema do request.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(opt => 
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); //Convert Enum Iteiro para String no Swagger.
    
})
.AddNewtonsoftJson(); //substitui os formatadores de entrada e saída baseados em padrão System.Text.Jsonusados.

//Adiciona e Configura DI para implementação de padrão Repositório
builder.Services.AddScoped<IBaseRepository, BaseRepository>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();

//Adiciona o contexto de banco de dados ao contêiner de DI.
//Especifica que o contexto de banco de dados usará um banco de dados 
//em memória.
//A String passada na Option é o nome que dei ao BD em Memória.
//Serviço para Venda.
builder.Services.AddDbContext<VendaContext>(opt =>
    opt.UseInMemoryDatabase("TechTestPaymentApi"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt => 
{
    //Documenta Cabeçalho da API no Swagger.
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
            Version = "v1",
            Title = "API DE PAGAMENTO",
            Description = "Web API ASP .NET 6 para gerenciar Vendas.",
            Contact = new OpenApiContact
            {
                Name = "Ailton Alves da Silva",
                Url = new Uri("https://www.linkedin.com/in/ailton-alves-da-silva"),
                Email = "ailton_as@hotmail.com",
                
            }
    }
    );

    //Habilita Anotações para Swagger inclusive Schema Filters.
    opt.EnableAnnotations();

    //Define Schema de Exemplo Customizado.
    opt.SchemaFilter<AddSchemaExample>();

    // Usa Reflections para Configure o Swagger para usar Documentação XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	
	if (File.Exists(xmlPath))
	{
			opt.IncludeXmlComments(xmlPath);
	}

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => 
    {
        options.RouteTemplate = "api-docs/swagger/{documentName}/swagger.json"; //Definindo Rota para Documentação Swagger. Altera Link abaixo do titulo na UI.
    });
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "api-docs/swagger"; //Definindo Rota para Documentação Swagger.

        options.SwaggerEndpoint("/api-docs/swagger/v1/swagger.json", "API de Vendas");

    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
