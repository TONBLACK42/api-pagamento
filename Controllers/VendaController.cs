using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Models;

namespace tech_test_payment_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        private readonly VendaContext _context;

        public VendaController(VendaContext context)
        {
            _context = context;
        }
        
        ///<summary>
        /// Busca Venda por ID.
        ///</summary>
        ///<param name="id"></param>
        /// <returns>A Venda Completa com seus Itens e dados do Vendedor.</returns>
        /// <response code="200">Retorna Venda e seus Items cadastrados.</response>
        /// <response code="404">Quando a Venda ou seus Itens não são encontrados.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> BuscarVenda(int id)
        {

            //Busca a Venda
            //Utilizando EF com carregamento Ansioso de Vários níveis
            var venda = _context.Vendas
                                .Where(v => v.Id == id)
                                .Include(vendedor => vendedor.Vendedores)
                                .Include("ItensDaVenda.Produto")
                                .FirstOrDefault();
            if (venda == null)
            {
                return NotFound(new {Erro = $"Não foi encontrada nenhuma venda com id {id}. Favor verificar se o mesmo esta correto."});
            }   
            
            return venda;
        }

        ///<summary>
        /// Altera a  Venda pelo ID indicado.
        ///</summary>
        ///<param name="id"></param>
        /// <returns>Altera a Venda Completa com seus Itens e dados do Vendedor.</returns>
        /// <response code="204">Quando a Venda é atualizada com Sucesso.</response>
        /// <response code="404">Quando a Venda ou seus Itens não são encontrados.</response>
        /// <response code="400">Quando ocorre algum problema ao Alterar a Venda ou seus Itens ou alguma Regra de Negócio é infrigida.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarVenda(int id, Venda venda)
        {
            if (id != venda.Id)
            {
                return BadRequest(new {Erro = $"O Id {id} informado não é válido!"});
            }

            //Busca Venda do BD
            var vendaBanco = _context.Vendas.FindAsync(id).Result;
            if (vendaBanco == null)
            {
                return NotFound(new {Erro = $"Venda não encontrada para o Id {id}."});
            }

            //Verifica o Status Informado.
            switch (vendaBanco.Status)
            {
                case EnumStatus.AguardandoPagamento:
                     if (venda.Status != EnumStatus.PagamentoAprovado && venda.Status != EnumStatus.Cancelada)
                        return BadRequest(new { Erro = "Para vendas que estão Aguardando Pagamento, " + 
                                                        "só é possível Aprovar o pagamento ou Cancelar a venda." });
                break;
                case EnumStatus.PagamentoAprovado:
                     if (venda.Status != EnumStatus.EnviandoParaTransportadora && venda.Status != EnumStatus.Cancelada)
                        return BadRequest(new { Erro = "Para vendas que tiveram o Pagamento Aprovado, " +
                                                        "só é possível Enviar para Transportadora ou Cancelar a venda." });                              
                break;
                case EnumStatus.EnviandoParaTransportadora:
                     if (venda.Status != EnumStatus.Entregue)
                        return BadRequest(new { Erro = "Para vendas que foram Enviadas para Transportadora, " +
                                                        "só é possível Enviar para Transportadora ou Cancelar a venda." });                              
                break;
                case EnumStatus.Entregue:
                    return BadRequest(new { Erro = "Os itens dessa Venda já foram entregues e a Venda finalizada, " +
                                                        "Não é possivel altera-la." });                              
                //break;
                case EnumStatus.Cancelada:
                    return BadRequest(new { Erro = "Esta Venda foi Cancelada, Não é possivel altera-la." });                              
               // break;
            }

           //Caso O Status esteja correto, atualiza a Venda com o novo Status.
           vendaBanco.Vendedores = venda.Vendedores;
           vendaBanco.ItensDaVenda = venda.ItensDaVenda;
           vendaBanco.Total = venda.Total;
           vendaBanco.Status = venda.Status;
           vendaBanco.Data = venda.Data;

            //Atualiza a Venda.
           _context.Vendas.Update(vendaBanco);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExists(id))
                {
                    return NotFound(new {Erro = $"Venda não encontrada para o Id {id}."});
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        ///<summary>
        /// Inclui uma nova venda, com informações do Vendedor e Itens da Venda e seu produto.
        ///</summary>
        ///<param name="venda"></param>
        /// <returns>A Venda em si, com sesu dados associados de Vendedor, Itens e Produto.</returns>
        /// <remarks>
        /// Exemplo de Preenchimento:
        ///
        ///     POST /Venda
        ///      {
        ///          "id": 0,
        ///          "vendedores": 
        ///          {
        ///              "id": 0,
        ///              "nome": "Vendedor 01",
        ///              "cpf": "111.222.333-44",
        ///              "email": "user@example.com",
        ///              "telefone": "5454-5445",
        ///              "vendaID": 0
        ///          },
        ///          "itensDaVenda": 
        ///          [
        ///              {
        ///                  "id": 0,
        ///                  "quantidade": 2,
        ///                  "valor": 12600.00,
        ///                  "vendaID": 0,
        ///                  "produto": 
        ///                  {
        ///                      "id": 0,
        ///                      "nome": "Seguro Residencial",
        ///                      "quantidade": 100,
        ///                      "valor": 6300.00
        ///                  }
        ///               }
        ///          ],
        ///          "total": 12300.00,
        ///          "status": "AguardandoPagamento",
        ///          "data": "2022-12-28T20:21:38.819Z"
        ///      }
        ///
        /// </remarks>
        /// <response code="201">Retorna Venda e seus Items que acabaram de ser cadastrados.</response>
        /// <response code="400">Quando Ocorre algum erro com a Venda ou seus Itens ou alguma regra de negócio é infrigida.</response>
       
        [HttpPost]
        public async Task<ActionResult<Venda>> RegistrarVenda(Venda venda)
        {
         
            _context.Vendas.Add(venda);

            if (venda.Status != EnumStatus.AguardandoPagamento)
            {
                return BadRequest(new { Erro = "Novas Vendas devem ser definidas como, 'Aguardando Pagamento'." });   
            }
           
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarVenda), new { id = venda.Id }, venda);
        }


        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.Id == id);
        }
    }
}
