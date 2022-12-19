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
        
        // GET: api/Venda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> BuscarVenda(int id)
        {

            //Busca a Venda
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            //Busca o Vendedor
            var vendedor = _context.Vendedores.FindAsync(venda.IdVendedor);
            if (vendedor == null)
            {
                return NotFound();
            }
            venda.Vendedor = vendedor.Result;

             //Busca os Itens
            var itensDaVenda = _context.ItensDaVenda.Where(x => x.idVenda==id);
            if (itensDaVenda == null)
            {
                return NotFound();
            }
            venda.ItensDaVenda = itensDaVenda.ToList();

            //Busca o Produto
            foreach (var item in venda.ItensDaVenda)
            {
                //Busca o Produto
                var produto = _context.Produtos.FindAsync(item.iProduto);
                if (produto == null)
                {
                    return NotFound();
                }

                item.Produto = produto.Result; 
                //venda.ItensDaVenda. = vendedor.Result;
            }
            
            return venda;
        }

        // PUT: api/Venda/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarVenda(int id, Venda venda)
        {
            if (id != venda.Id)
            {
                return BadRequest();
            }

            //Busca Venda do BD
            var vendaBanco = _context.Vendas.FindAsync(id).Result;
            if (vendaBanco == null)
            {
                return NotFound();
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
                break;
                case EnumStatus.Cancelada:
                    return BadRequest(new { Erro = "Esta Venda foi Cancelada, " +
                                                        "Não é possivel altera-la." });                              
                break;
            }

           
           vendaBanco.IdVendedor = venda.IdVendedor;
           vendaBanco.Vendedor = venda.Vendedor;
           vendaBanco.ItensDaVenda = venda.ItensDaVenda;
           vendaBanco.Total = venda.Total;
           vendaBanco.Status = venda.Status;
           vendaBanco.Data = venda.Data;

           _context.Vendas.Update(vendaBanco);
            //_context.Entry(venda).State = EntityState.Modified;
           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Venda
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Venda>> RegistrarVenda(Venda venda)
        {
         
            _context.Vendas.Add(venda);

            if (venda.Status != EnumStatus.AguardandoPagamento)
            {
                return BadRequest(new { Erro = "Deve ser definido 'Aguardando Pagamento' para novas Vendas." });   
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
