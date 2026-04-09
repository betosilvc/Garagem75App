using Garagem75.Api.Data;
using Garagem75.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garagem75.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdemServicoPecaController : ControllerBase
    {
        private readonly Garagem75DBContext _context;

        public OrdemServicoPecaController(Garagem75DBContext context)
        {
            _context = context;
        }

        // 🔍 LISTAR PEÇAS DA OS
        [HttpGet("{ordemId}")]
        public async Task<IActionResult> GetPecas(int ordemId)
        {
            var itens = await _context.OrdemServicoPecas
                .Include(x => x.Peca)
                .Where(x => x.OrdemServicoId == ordemId)
                .ToListAsync();

            return Ok(itens);
        }

        // ➕ ADICIONAR PEÇA
        [HttpPost]
        public async Task<IActionResult> AddPeca([FromQuery] int ordemId, [FromQuery] int pecaId, [FromQuery] int quantidade)
        {
            var os = await _context.OrdemServicos
                .Include(o => o.PecasAssociadas)
                .FirstOrDefaultAsync(o => o.IdOrdemServico == ordemId);

            var peca = await _context.Pecas.FindAsync(pecaId);

            if (os == null || peca == null)
                return NotFound();

            var existente = os.PecasAssociadas
                .FirstOrDefault(p => p.PecaId == pecaId);

            if (existente != null)
            {
                existente.Quantidade += quantidade;
            }
            else
            {
                var item = new OrdemServicoPeca
                {
                    OrdemServicoId = ordemId,
                    PecaId = pecaId,
                    Quantidade = quantidade,
                    PrecoUnitario = peca.Preco // 🔥 correto agora
                };

                os.PecasAssociadas.Add(item);
            }

            RecalcularTotal(os);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // ✏️ ATUALIZAR QUANTIDADE
        [HttpPut]
        public async Task<IActionResult> UpdateQuantidade(int ordemId, int pecaId, int quantidade)
        {
            var item = await _context.OrdemServicoPecas
                .FirstOrDefaultAsync(x =>
                    x.OrdemServicoId == ordemId &&
                    x.PecaId == pecaId);

            if (item == null)
                return NotFound();

            item.Quantidade = quantidade;

            var os = await _context.OrdemServicos
                .Include(o => o.PecasAssociadas)
                .FirstAsync(o => o.IdOrdemServico == ordemId);

            RecalcularTotal(os);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ❌ REMOVER PEÇA
        [HttpDelete]
        public async Task<IActionResult> RemovePeca([FromQuery] int ordemId, [FromQuery] int pecaId)
        {
            var item = await _context.OrdemServicoPecas
                .FirstOrDefaultAsync(x =>
                    x.OrdemServicoId == ordemId &&
                    x.PecaId == pecaId);

            if (item == null)
                return NotFound();

            _context.OrdemServicoPecas.Remove(item);

            var os = await _context.OrdemServicos
                .Include(o => o.PecasAssociadas)
                .FirstAsync(o => o.IdOrdemServico == ordemId);

            RecalcularTotal(os);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔥 REGRA DE NEGÓCIO CENTRAL
        private void RecalcularTotal(OrdemServico os)
        {
            var totalPecas = os.PecasAssociadas
                .Sum(p => p.Subtotal); // 🔥 usa propriedade do model

            os.ValorTotal = os.MaoDeObra + totalPecas - os.ValorDesconto;
        }
    }
}