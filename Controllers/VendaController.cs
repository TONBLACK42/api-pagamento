using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        // GET: api/Venda
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venda>>> GetVendas()
        {

            return await _context.Vendas.ToListAsync();
        }

        // GET: api/Venda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> GetVenda(int id)
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
        public async Task<IActionResult> PutVenda(int id, Venda venda)
        {
            if (id != venda.Id)
            {
                return BadRequest();
            }

            _context.Entry(venda).State = EntityState.Modified;

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
        public async Task<ActionResult<Venda>> PostVenda(Venda venda)
        {
         
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenda", new { id = venda.Id }, venda);
        }

        // DELETE: api/Venda/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenda(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.Id == id);
        }
    }
}
