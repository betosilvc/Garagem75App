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
        public async Task<ActionResult<IEnumerable<OrdemServicoDto>>> GetAll()
        {
            var lista = await _context.OrdemServicos
                .Include(o => o.Veiculo)
                .Include(o => o.PecasAssociadas)
                    .ThenInclude(p => p.Peca)
                .ToListAsync();

            return Ok(_mapper.Map<List<OrdemServicoDto>>(lista));
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdemServicoDto>> GetById(int id)
        {
            var os = await _context.OrdemServicos
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
            var entity = _mapper.Map<OrdemServico>(dto);

            entity.Status = "Aberta";

            // 🔥 CALCULAR TOTAL
            entity.ValorTotal = entity.MaoDeObra - entity.ValorDesconto;

            _context.OrdemServicos.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = entity.IdOrdemServico },
                _mapper.Map<OrdemServicoDto>(entity));
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