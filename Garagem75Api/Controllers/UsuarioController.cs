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
    public class UsuarioController : ControllerBase
    {
        private readonly Garagem75DBContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(Garagem75DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
        {
            var lista = await _context.Usuarios.ToListAsync();
            return Ok(_mapper.Map<List<UsuarioDto>>(lista));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var item = await _context.Usuarios.FindAsync(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<UsuarioDto>(item));
        }

        [HttpPost]
        public async Task<ActionResult> Create(UsuarioDto dto)
        {
            var entity = _mapper.Map<Usuario>(dto);

            _context.Usuarios.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UsuarioDto>(entity);

            return CreatedAtAction(nameof(GetById),
                new { id = result.IdUsuario }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UsuarioDto dto)
        {
            if (id != dto.IdUsuario)
                return BadRequest();

            var entity = await _context.Usuarios.FindAsync(id);

            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Usuarios.FindAsync(id);

            if (entity == null)
                return NotFound();

            _context.Usuarios.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}