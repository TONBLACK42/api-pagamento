using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_pagamento.Pagamento.Api.Models;

using Microsoft.AspNetCore.JsonPatch; //Adicionei para usar HttpPatch
using api_pagamento.Pagamento.Api.Repository.Interfaces; //Adicionada para Padrão Repositorio
using Newtonsoft.Json;

namespace api_pagamento.Pagamento.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        private readonly IVendaRepository _repository;

        public VendaController(IVendaRepository repository)
        {
            _repository = repository;
        }
        
        ///<summary>
        /// Busca Venda por ID.
        ///</summary>
        ///<param name="id"></param>
        /// <returns>A Venda Completa com seus Itens e dados do Vendedor.</returns>
        /// <response code="200">Retorna Venda e seus Items cadastrados.</response>
        /// <response code="404">Quando a Venda ou seus Itens não são encontrados.</response>
        [HttpGet("buscarvenda/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Venda>> BuscarVenda(int id)
        {

            var venda = await _repository.GetByIdAsync(id);
            if (venda == null)
            {
                return NotFound(new {Erro = $"Não foi encontrada nenhuma venda com id {id}."});
            }   
            
            return Ok(venda);
        }

        /// <summary>
        /// Permite alterar somente o Status da Venda informada pelo ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="venda">Aqui representada somente pelo campo a ser alterado.</param>
        /// <returns>A Venda Completa com seus Itens e dados do Vendedor.</returns>
        /// <remarks>
        ///
        /// Exemplo de Preenchimento:
        ///
        ///     PATCH /Venda
        ///         [
        ///             {
        ///                 "op": "Replace",    
        ///                 "path": "/status",
        ///                 "value": 2 ou "PagamentoAprovado"
        ///             }
        ///         ]
        ///
        /// </remarks>
        /// <response code="204">Quando a Venda é atualizada com Sucesso.</response>
        /// <response code="404">Quando a Venda ou seus Itens não são encontrados.</response>
        /// <response code="400">Quando ocorre algum problema ao Alterar a Venda ou seus Itens ou alguma Regra de Negócio é infrigida.</response>
        /// <response code="500">Quando ocorre algum problema no lado do Servidor</response>
        [HttpPatch("atualizarvenda/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AttualizarVenda(int id,[FromBody] JsonPatchDocument<Venda> venda)
        {
            if (venda == null)
            {
                return BadRequest(new {Erro = "Modelo de Venda inválido."});
            }

            var bancoVenda = await _repository.GetByIdAsync(id);
            
            if (bancoVenda == null)
            {
                return NotFound(new {Erro = $"Venda não encontrada para o Id {id}."});
            }

            int statusInteiro = Convert.ToInt32(venda.Operations[0].value);
            EnumStatus status = (EnumStatus)statusInteiro;
                                  
            //Verifica o Status Informado.
            switch (bancoVenda.Status)
            {
                case EnumStatus.AguardandoPagamento:
                     if ( status != EnumStatus.PagamentoAprovado && status != EnumStatus.Cancelada)
                        return BadRequest(new { Erro = "Para vendas que estão Aguardando Pagamento, " + 
                                                        "só é possível Aprovar o pagamento ou Cancelar a venda." });
                break;
                case EnumStatus.PagamentoAprovado:
                     if (status != EnumStatus.EnviandoParaTransportadora && status != EnumStatus.Cancelada)
                        return BadRequest(new { Erro = "Para vendas que tiveram o Pagamento Aprovado, " +
                                                        "só é possível Enviar para Transportadora ou Cancelar a venda." });                              
                break;
                case EnumStatus.EnviandoParaTransportadora:
                     if (status != EnumStatus.Entregue)
                        return BadRequest(new { Erro = "Para vendas que foram Enviadas para Transportadora, " +
                                                        "só é possível Entregar a venda." });                              
                break;
                case EnumStatus.Entregue:
                    return BadRequest(new { Erro = "Os itens dessa Venda já foram entregues e a Venda finalizada, " +
                                                        "Não é possivel altera-la." });                              

                case EnumStatus.Cancelada:
                    return BadRequest(new { Erro = "Esta Venda foi Cancelada, Não é possivel altera-la." });                              
            }

             venda.ApplyTo(bancoVenda, ModelState);

             var isValid = TryValidateModel(bancoVenda);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await _repository.SaveChangesAsync();
            
            return NoContent();

        }
     
        /// <summary>
        /// Inclui uma nova venda, com informações do Vendedor e Itens da Venda e seu produto.
        /// </summary>
        /// <param name="venda"></param>
        /// <returns>A Venda em si, com sesu dados associados de Vendedor, Itens e Produto.</returns>
        /// <remarks>
        /// Exemplo de Preenchimento:
        ///
        ///     POST /Venda
        ///      {
        ///          "vendedores": 
        ///          {
        ///              "nome": "Vendedor 01",
        ///              "cpf": "111.222.333-44",
        ///              "email": "user@pottencial.com.br",
        ///              "telefone": "5454-5445"
        ///          },
        ///          "itensDaVenda": 
        ///          [
        ///              {
        ///                  "quantidade": 2,
        ///                  "valor": 1250.00,
        ///                  "produto": 
        ///                  {
        ///                      "nome": "Seguro Residencial",
        ///                      "quantidade": 100,
        ///                      "valor": 1250.00
        ///                  }
        ///               }
        ///          ],
        ///          "total": 2500.00
        ///      }
        ///
        /// </remarks>
        /// <response code="201">Retorna Venda e seus Items que acabaram de ser cadastrados.</response>
        /// <response code="400">Quando Ocorre algum erro com a Venda ou seus Itens ou alguma regra de negócio é infrigida.</response>
       
        [HttpPost("registrarvenda/")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Venda>> RegistrarVenda(Venda venda)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
         
            if (venda.Status != EnumStatus.AguardandoPagamento)
            {
                return BadRequest(new { Erro = "Novas Vendas devem ser definidas como, 'Aguardando Pagamento'." });   
            }

            _repository.Add(venda);

           
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarVenda), new { id = venda.Id }, venda);
        }
       
    }
}
