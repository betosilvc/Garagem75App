using AutoMapper;
using Garagem75.Api.Data;
using Garagem75.Shared.Dtos;
using Garagem75.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garagem75.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdemServicoController : ControllerBase
    {
        private readonly Garagem75DBContext _context;
        private readonly IMapper _mapper;

        public OrdemServicoController(Garagem75DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<List<OrdemServicoDto>>> GetAll()
        {
            try
            {
                var lista = await _context.OrdemServicos
                    .Include(o => o.Veiculo)
                        .ThenInclude(v => v.Cliente)
                    .Include(o => o.PecasAssociadas)
                        .ThenInclude(p => p.Peca)
                    .ToListAsync();
                lista = lista.OrderByDescending(o => o.DataServico).ToList();
                var resultado = _mapper.Map<List<OrdemServicoDto>>(lista);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar OS: {ex.Message}");
            }
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdemServicoDto>> GetById(int id)
        {
            var os = await _context.OrdemServicos
                .Include(o => o.Veiculo)
                        .ThenInclude(v => v.Cliente)
                    .Include(o => o.PecasAssociadas)
                        .ThenInclude(p => p.Peca)
                .FirstOrDefaultAsync(o => o.IdOrdemServico == id);

            if (os == null)
                return NotFound();

            return Ok(_mapper.Map<OrdemServicoDto>(os));
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> Create(OrdemServicoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 👈 AQUI
            var entity = new OrdemServico
            {
                Descricao = dto.Descricao,
                VeiculoId = dto.VeiculoId,
                ClienteId = dto.ClienteId,
                MaoDeObra = dto.MaoDeObra,
                ValorDesconto = dto.ValorDesconto,
                Status = "Aberta",
                DataServico = DateTime.Now,
                DataEntrega = DateTime.Now.AddDays(1)
            };

            // 🔥 ADICIONAR PEÇAS
            foreach (var item in dto.Pecas)
            {
                var peca = await _context.Pecas.FindAsync(item.PecaId);

                if (peca == null) continue;
                // 🔥 VALIDA ESTOQUE (ANTES DE USAR)
                if (peca.QuantidadeEstoque < item.Quantidade)
                    return BadRequest($"Estoque insuficiente para: {peca.Nome}");

                // 🔥 BAIXA ESTOQUE
                peca.QuantidadeEstoque -= item.Quantidade;


                entity.PecasAssociadas.Add(new OrdemServicoPeca
                {
                    PecaId = peca.IdPeca,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = peca.Preco
                });
            }

            // 🔥 CALCULAR TOTAL
            var totalPecas = entity.PecasAssociadas
                .Sum(p => p.Quantidade * p.PrecoUnitario);

            entity.ValorTotal = entity.MaoDeObra + totalPecas - entity.ValorDesconto;

            _context.OrdemServicos.Add(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrdemServicoDto dto)
        {
            if (id != dto.IdOrdemServico)
                return BadRequest();

            var entity = await _context.OrdemServicos.FindAsync(id);

            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            // 🔥 RECALCULA
            entity.ValorTotal = entity.MaoDeObra - entity.ValorDesconto;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // FINALIZAR
        [HttpPut("{id}/finalizar")]
        public async Task<IActionResult> Finalizar(int id)
        {
            var os = await _context.OrdemServicos.FindAsync(id);

            if (os == null)
                return NotFound();

            os.Status = "Finalizada";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.OrdemServicos.FindAsync(id);

            if (entity == null)
                return NotFound();

            _context.OrdemServicos.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}