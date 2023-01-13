using System.Threading.Tasks;
using Xunit;
using Moq; // Usar o Mock para teste.
using tech_test_payment_api.Repository.Interfaces;
using tech_test_payment_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Models;
using System.Collections.Generic;

namespace Payment.Tests;

public class VendaControllerTests
{
    #region BuscarVenda_RetornaHttpNotFound_ComIdInvalido
    [Fact]
    public async Task BuscarVenda_RetornaHttpNotFound_ComIdInvalido()
    {
        // Arrange
        int idVendaTest = 1;
        var mockRepo = new Mock<IVendaRepository>();
        mockRepo.Setup(repo => repo.GetByIdAsync(idVendaTest))
            .ReturnsAsync((Venda)null);
        var controller = new VendaController(mockRepo.Object);

        // Act
        var result = await controller.BuscarVenda(idVendaTest);

        // Assert
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(idVendaTest, notFoundObjectResult.Value);
    }
    #endregion

    #region BuscaVenda_RetornaHttpOk_ComIdValido
    [Fact]
    public async Task BuscaVenda_RetornaHttpOk_ComIdValido()
    {
        // Arrange
        int idVendaTest = 1;
        var mockRepo = new Mock<IVendaRepository>();
        mockRepo.Setup(repo => repo.GetByIdAsync(idVendaTest))
            .ReturnsAsync(BuscaVendaDeTest());
        var controller = new VendaController(mockRepo.Object);

        // Act
        var result = await controller.BuscarVenda(idVendaTest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        //var returnValue = Assert.IsType<List<Venda>>(okResult.Value);
        var returnValue = Assert.IsType<Venda>(okResult.Value);
        
        
        //var venda = returnValue[0]; //FirstOrDefault();
       Assert.Equal(EnumStatus.AguardandoPagamento, returnValue.Status);
        
        //Assert.Equal(idVendaTest, okResult.Value);
    }
    #endregion


    private Venda BuscaVendaDeTest()
    {
        var venda = new Venda();
        venda.Id = 1;

        var vendedor = new Vendedor();
        vendedor.Id = 1;
        vendedor.Nome = "Maria Eduarda";
        vendedor.CPF = "123.456.789-11";
        vendedor.Email = "maria@eduarda.com.br";
        vendedor.Telefone = "1234-5678";
        vendedor.VendaID = 1;

        venda.Vendedores = vendedor;

        var itensDaVenda = new List<ItemDaVenda>();

        var itemDaVenda = new ItemDaVenda();
        itemDaVenda.Id = 1;
        itemDaVenda.Quantidade = 1;
        itemDaVenda.Valor = 4;
        itemDaVenda.VendaID = 1;

        var produto = new Produto();
        produto.Id = 1;
        produto.Nome = "Seguro Veicular";
        produto.Quantidade = 2;
        produto.Valor = 4;

        itemDaVenda.Produto = produto;

        itensDaVenda.Add(itemDaVenda);

        venda.ItensDaVenda = itensDaVenda;

        venda.Total = 8;
        venda.Status = EnumStatus.AguardandoPagamento;
        venda.Data = System.DateTime.Now;



        return venda;
        // var venda = new Venda()
        // {
        //     Vendedores = new Vendedor()
        //     {
        //         Nome = "Maria Eduarda",
        //         CPF = "123.456.789-11",
        //         Email = "maria@eduarda.com.br",
        //         Telefone = "1234-5678",
        //         VendaID = 1

        //     },
        //     ItensDaVenda  = new List<ItemDaVenda>{
        //         new ItemDaVenda
        //         {
        //             Quantidade = 1,
        //             Valor = 4,
        //             VendaID =1,
        //             Produto = new Produto()
        //             {
        //                 Nome = "Seguro Veicular",
        //                 Quantidade =2,
        //                 Valor = 4
        //             }
        //         }

        //     }
        // };

        //return venda;
    }



}