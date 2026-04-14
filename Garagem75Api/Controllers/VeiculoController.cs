using AutoMapper;
using Garagem75.Api.Data;
using Garagem75.Shared; // <-- IMPORTANTE
using Garagem75.Shared.Dtos;
using Garagem75.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garagem75.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Administrador,Mecanico")]
    public class VeiculoController : ControllerBase
    {
        private readonly Garagem75DBContext _context;

        private readonly IMapper _mapper;

        public VeiculoController(Garagem75DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeiculoDto>>> GetAll(
            string? searchPlaca,
            string? searchCliente)
        {
            var query = _context.Veiculos
                .Include(v => v.Cliente)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchPlaca))
                query = query.Where(v => v.Placa.Contains(searchPlaca));

            if (!string.IsNullOrEmpty(searchCliente))
                query = query.Where(v => v.Cliente.Nome.Contains(searchCliente));

            var lista = await query.ToListAsync();

            return Ok(_mapper.Map<List<VeiculoDto>>(lista));
            
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<VeiculoDto>> GetById(int id)
        {
            var v = await _context.Veiculos
                .Include(x => x.Cliente)
                .FirstOrDefaultAsync(x => x.IdVeiculo == id);

            if (v == null)
                return NotFound();

            return Ok(_mapper.Map<VeiculoDto>(v));
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> Create(VeiculoDto dto)
        {
            // ✅ Verifica placa duplicada
            bool placaExiste = await _context.Veiculos
                .AnyAsync(v => v.Placa == dto.Placa);

            if (placaExiste)
                return BadRequest(new { mensagem = "Placa já cadastrada." });
            var veiculo = _mapper.Map<Veiculo>(dto);

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<VeiculoDto>(veiculo);

            return CreatedAtAction(nameof(GetById),
                new { id = veiculo.IdVeiculo }, dto);
        }

        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadFoto(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido");

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound();

            // 📁 pasta
            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/veiculos");

            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            // 🔥 nome único
            var nomeArquivo = $"{id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var caminhoCompleto = Path.Combine(pasta, nomeArquivo);

            // 💾 salva arquivo
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 🌐 URL pública
            var url = $"/uploads/veiculos/{nomeArquivo}";

            veiculo.FotoUrl = url;
            await _context.SaveChangesAsync();

            return Ok(new { fotoUrl = url });
        }
        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VeiculoDto dto)
        {
            if (id != dto.IdVeiculo)
                return BadRequest();

            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                return NotFound();
            // ✅ Verifica placa duplicada, ignorando o próprio veículo
            bool placaExiste = await _context.Veiculos
                .AnyAsync(v => v.Placa == dto.Placa && v.IdVeiculo != id);

            if (placaExiste)
                return BadRequest(new { mensagem = "Placa já cadastrada." });

            // Atualiza campos
            _mapper.Map(dto, veiculo);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                return NotFound();

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetByCliente(int clienteId)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.ClienteId == clienteId)
                .ToListAsync();

            return Ok(veiculos);
        }
    }
}