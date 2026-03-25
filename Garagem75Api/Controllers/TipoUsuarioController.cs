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
    public class TipoUsuarioController : ControllerBase
    {
        private readonly Garagem75DBContext _context;
        private readonly IMapper _mapper;

        public TipoUsuarioController(Garagem75DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoUsuarioDto>>> GetAll()
        {
            var lista = await _context.TipoUsuarios.ToListAsync();
            return Ok(_mapper.Map<List<TipoUsuarioDto>>(lista));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoUsuarioDto>> GetById(int id)
        {
            var item = await _context.TipoUsuarios.FindAsync(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<TipoUsuarioDto>(item));
        }

        [HttpPost]
        public async Task<ActionResult> Create(TipoUsuarioDto dto)
        {
            var entity = _mapper.Map<TipoUsuario>(dto);

            _context.TipoUsuarios.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = entity.IdTipoUsuario }, _mapper.Map<TipoUsuarioDto>(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TipoUsuarioDto dto)
        {
            if (id != dto.IdTipoUsuario)
                return BadRequest();

            var entity = await _context.TipoUsuarios.FindAsync(id);

            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.TipoUsuarios.FindAsync(id);

            if (entity == null)
                return NotFound();

            _context.TipoUsuarios.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}