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
    public class ClienteController : ControllerBase
    {
        private readonly Garagem75DBContext _context;
        private readonly IMapper _mapper;

        public ClienteController(Garagem75DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
        {
            var lista = await _context.Clientes.ToListAsync();
            return Ok(_mapper.Map<List<ClienteDto>>(lista));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetById(int id)
        {
            var cliente = await _context.Clientes
        .Include(c => c.Enderecos) // 👈 ESSA LINHA
        .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null)
                return NotFound();

            return Ok(_mapper.Map<ClienteDto>(cliente));
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClienteDto dto)
        {
            // ✅ Verifica CPF duplicado
            bool cpfExiste = await _context.Clientes
                .AnyAsync(c => c.Cpf == dto.Cpf);

            if (cpfExiste)
                return BadRequest(new { mensagem = "CPF já cadastrado." });

            // ✅ Verifica Email duplicado
            bool emailExiste = await _context.Clientes
                .AnyAsync(c => c.Email == dto.Email);

            if (emailExiste)
                return BadRequest(new { mensagem = "E-mail já cadastrado." });
            var entity = _mapper.Map<Cliente>(dto);

            // 👇 GARANTE RELACIONAMENTO
            if (entity.Enderecos != null)
            {
                foreach (var e in entity.Enderecos)
                {
                    e.Cliente = entity;
                }
            }

            _context.Clientes.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = entity.IdCliente },
                _mapper.Map<ClienteDto>(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClienteDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var entity = await _context.Clientes.FindAsync(id);

            if (entity == null)
                return NotFound();

            // ✅ Verifica CPF duplicado, ignorando o próprio cliente
            bool cpfExiste = await _context.Clientes
                .AnyAsync(c => c.Cpf == dto.Cpf && c.IdCliente != id);

            if (cpfExiste)
                return BadRequest(new { mensagem = "CPF já cadastrado." });

            // ✅ Verifica Email duplicado, ignorando o próprio cliente
            bool emailExiste = await _context.Clientes
                .AnyAsync(c => c.Email == dto.Email && c.IdCliente != id);

            if (emailExiste)
                return BadRequest(new { mensagem = "E-mail já cadastrado." });

            _mapper.Map(dto, entity);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Carrega o cliente incluindo os endereços (e veículos se houver)
            var entity = await _context.Clientes
                .Include(c => c.Enderecos)
                // .Include(c => c.Veiculos) // Se houver veículos, inclua aqui também
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (entity == null)
                return NotFound();

            // Remove os endereços vinculados primeiro
            if (entity.Enderecos != null && entity.Enderecos.Any())
            {
                _context.Enderecos.RemoveRange(entity.Enderecos);
            }

            // Agora remove o cliente
            _context.Clientes.Remove(entity);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("endereco/{id}")]
        public async Task<IActionResult> DeleteEndereco(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);

            if (endereco == null)
                return NotFound();

            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}