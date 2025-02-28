using Microsoft.AspNetCore.Mvc;
using PedidoApi.Data;
using PedidoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace PedidoApi.Controllers {
    [ApiController]
    [Route("pedidos")]
    public class PedidoController : ControllerBase {
        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPedido([FromBody] Pedido pedido)
        {
            if (pedido == null || pedido.Itens == null || !pedido.Itens.Any()) {
                return BadRequest("Dados do pedido inválidos.");
            }

            if (pedido.Total == 0) {
                pedido.Total = pedido.Itens.Sum(i => i.Quantidade * i.Preco);
            }

            if (string.IsNullOrEmpty(pedido.Status)) {
                pedido.Status = "PENDENTE";
            }

            pedido.DataCriacao = DateTime.UtcNow;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var resultado = new
            {
                id = pedido.Id,
                status = pedido.Status,
                total = pedido.Total,
                data_criacao = pedido.DataCriacao
            };

            return CreatedAtAction(nameof(CadastrarPedido), new { id = pedido.Id }, resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodosPedidos()
        {
            var pedidos = await _context.Pedidos.Include(p => p.Itens).ToListAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterDetalhesPedido(Guid id)
        {
            var pedido = await _context.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null) {
                return NotFound("Pedido não encontrado.");
            }

            return Ok(pedido);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> AtualizarStatusPedido(Guid id, [FromBody] Pedido pedido)
        {
            var pedidoExistente = await _context.Pedidos.FindAsync(id);

            if (pedidoExistente == null) {
                return NotFound("Pedido não encontrado.");
            }

            pedidoExistente.Status = pedido.Status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pedido atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPedido(Guid id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null) {
                return NotFound("Pedido não encontrado.");
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
