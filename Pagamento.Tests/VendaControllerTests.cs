using System.Threading.Tasks;
using Xunit;
using Moq; // Usar o Mock para teste.
using api_pagamento.Pagamento.Api.Repository.Interfaces;
using api_pagamento.Pagamento.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using api_pagamento.Pagamento.Api.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

namespace tech_test_payment_api.Payment.Pagamento.Tests;

public class VendaControllerTests
{
    #region BuscaVenda_RetornaHttpNotFound_ComIdInvalido

    [Fact(DisplayName = "BuscaVenda_RetornaHttpNotFound_ComIdInvalido")]
    public async Task BuscaVenda_RetornaHttpNotFound_ComIdInvalido()
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

    [Fact(DisplayName = "BuscaVenda_RetornaHttpOk_ComIdValido")]
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
        var returnValue = Assert.IsType<Venda>(okResult.Value);

        Assert.Equal(EnumStatus.AguardandoPagamento, returnValue.Status);
    }
    #endregion

    #region RegistrarVenda_RetornaBadRequest_PassandoModeloInvalido

    [Fact(DisplayName = "RegistrarVenda_RetornaBadRequest_PassandoModeloInvalido")]
    public async Task RegistrarVenda_RetornaBadRequest_PassandoModeloInvalido()
    {
        // Arrange & Act
        var mockRepo = new Mock<IVendaRepository>();
        var controller = new VendaController(mockRepo.Object);
        controller.ModelState.AddModelError("Erro", "Algum erro");

        // Act
        var result = await controller.RegistrarVenda(venda: null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
    #endregion

    #region RegistraVenda_RetornaBadRequest_PassandoStatusInvalido

    [Fact(DisplayName = "RegistraVenda_RetornaBadRequest_PassandoStatusInvalido")]
    public async Task RegistraVenda_RetornaBadRequest_PassandoStatusInvalido()
    {
        // Arrange
        EnumStatus statusVendaTese = EnumStatus.Entregue;
        var mockRepo = new Mock<IVendaRepository>();
        var controller = new VendaController(mockRepo.Object);
        var venda = new Venda()
        {
            Status = statusVendaTese
        };

        // Act
        var result = await controller.RegistrarVenda(venda);

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(venda, badRequestObjectResult.Value);
    }
    #endregion

    #region RegistraVenda_RetornaCreated_PassandoVendaValida

    [Fact(DisplayName = "RegistraVenda_RetornaCreated_PassandoVendaValida")]
    public async Task RegistraVenda_RetornaCreated_PassandoVendaValida()
    {
        // Arrange
        EnumStatus statusVendaTese = EnumStatus.AguardandoPagamento;

        var mockRepo = new Mock<IVendaRepository>();
        var controller = new VendaController(mockRepo.Object);

        var venda = BuscaVendaDeTest();

        // Act
        var result = await controller.RegistrarVenda(venda);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnVenda = Assert.IsType<Venda>(createdAtActionResult.Value);
        mockRepo.Verify();

        Assert.Equal(venda, createdAtActionResult.Value);
        Assert.Equal(statusVendaTese, returnVenda.Status);
    }
    #endregion

    #region AttualizaStatusDaVenda_RetornaBadRequest_PassandoStatusInvalido

    [Theory(DisplayName = "AttualizaStatusDaVenda_RetornaBadRequest_PassandoStatusInvalido")]
    [MemberData(nameof(DadosParaTesteDeStatusInvalido))]
    public async Task AttualizaStatusDaVenda_RetornaBadRequest_PassandoStatusInvalido(EnumStatus statusAtualVendaTest, EnumStatus statusNovoVendaTest)
    {
        // Arrange
        int idVendaTest = 1;

        //Simula a Criação de uma Venda e Define o seu Status Atual.
        var venda = BuscaVendaDeTest();
        venda.Status = statusAtualVendaTest;
        //Cria o Repositorio Mok da venda com o Status atual.
        var mockRepo = new Mock<IVendaRepository>();
        mockRepo.Setup(repo => repo.GetByIdAsync(idVendaTest))
            .ReturnsAsync(venda);
        var controller = new VendaController(mockRepo.Object);

        //Cria um JsonPatchDocument simulando a Requisição HttpPatch com a alteração do Status.
        JsonPatchDocument<Venda> vendaJSonPatchDocument = new JsonPatchDocument<Venda>();
        vendaJSonPatchDocument.Replace(v => v.Status, statusNovoVendaTest);

        // Act
        var result = await controller.AttualizarVenda(idVendaTest, vendaJSonPatchDocument);

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(venda, badRequestObjectResult.Value);
    }
    #endregion
        
    private Venda BuscaVendaDeTest()
    {
        var venda = new Venda()
        {
            Id = 1,
            Vendedores = new Vendedor()
            {
                Id = 1,
                Nome = "Maria Eduarda",
                CPF = "123.456.789-11",
                Email = "maria@eduarda.com.br",
                Telefone = "1234-5678",
                VendaID = 1
            },
            ItensDaVenda = new List<ItemDaVenda>()
            {

                new ItemDaVenda()
                {
                    Id = 1,
                    Quantidade = 1,
                    Valor = 4,
                    VendaID = 1,
                    Produto = new Produto()
                    {
                        Id = 1,
                        Nome = "Seguro Veicular",
                        Quantidade = 2,
                        Valor = 4
                    }
                }
            },
            Total = 8,
            Status = EnumStatus.AguardandoPagamento
            //Data = System.DateTime.Now
        };

        return venda;
    }

    public static IEnumerable<object[]> DadosParaTesteDeStatusInvalido()
    {
        //Status Aguardando Pagamento
        yield return new object[] {EnumStatus.AguardandoPagamento, EnumStatus.AguardandoPagamento};
        yield return new object[] {EnumStatus.AguardandoPagamento, EnumStatus.EnviandoParaTransportadora};
        yield return new object[] {EnumStatus.AguardandoPagamento, EnumStatus.Entregue};
        //Status Pagamento Aprovado
        yield return new object[] {EnumStatus.PagamentoAprovado,EnumStatus.AguardandoPagamento};
        yield return new object[] {EnumStatus.PagamentoAprovado,EnumStatus.PagamentoAprovado};
        yield return new object[] {EnumStatus.PagamentoAprovado,EnumStatus.Entregue};
        //Status Enviado Para Transportadora
        yield return new object[] {EnumStatus.EnviandoParaTransportadora,EnumStatus.AguardandoPagamento};
        yield return new object[] {EnumStatus.EnviandoParaTransportadora,EnumStatus.PagamentoAprovado};
        yield return new object[] {EnumStatus.EnviandoParaTransportadora,EnumStatus.EnviandoParaTransportadora};
        //Status Entregue
        yield return new object[] {EnumStatus.Entregue,EnumStatus.AguardandoPagamento};
        yield return new object[] {EnumStatus.Entregue,EnumStatus.PagamentoAprovado};
        yield return new object[] {EnumStatus.Entregue,EnumStatus.EnviandoParaTransportadora};
        yield return new object[] {EnumStatus.Entregue,EnumStatus.Entregue};
        yield return new object[] {EnumStatus.Entregue,EnumStatus.Cancelada};
        //Status Cancelada.
        yield return new object[] {EnumStatus.Cancelada,EnumStatus.AguardandoPagamento};
        yield return new object[] {EnumStatus.Cancelada,EnumStatus.PagamentoAprovado};
        yield return new object[] {EnumStatus.Cancelada,EnumStatus.EnviandoParaTransportadora};
        yield return new object[] {EnumStatus.Cancelada,EnumStatus.Entregue};
        yield return new object[] {EnumStatus.Cancelada,EnumStatus.Cancelada};
    }

}