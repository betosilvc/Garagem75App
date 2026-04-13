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
    public class PecaController : ControllerBase
    {
        private readonly Garagem75DBContext _context;
        private readonly IMapper _mapper;

        public PecaController(Garagem75DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PecaDto>>> GetAll()
        {
            var listaEntidade = await _context.Pecas
                .OrderBy(p => p.Nome)
                .ToListAsync();

            // 1. Primeiro mapeia para DTO
            var listaDto = _mapper.Map<List<PecaDto>>(listaEntidade);

            // 2. Obtém a base do túnel de forma dinâmica
            // Se estiver no túnel, o Host será o endereço .devtunnels.ms
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            foreach (var dto in listaDto)
            {
                if (!string.IsNullOrEmpty(dto.Imagem))
                {
                    // Se a URL gravada no banco contiver localhost
                    if (dto.Imagem.Contains("localhost"))
                    {
                        // Extraímos apenas o caminho do ficheiro (ex: /uploads/pecas/foto.jpg)
                        // O Uri ajuda a separar a porta do caminho real
                        var uri = new Uri(dto.Imagem);
                        dto.Imagem = $"{baseUrl}{uri.PathAndQuery}";
                    }
                    // Se no banco estiver apenas "uploads/pecas/foto.jpg" (sem http)
                    else if (!dto.Imagem.StartsWith("http"))
                    {
                        dto.Imagem = $"{baseUrl}/{dto.Imagem.TrimStart('/')}";
                    }
                }
            }

            return Ok(listaDto);
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<PecaDto>>> Buscar(string termo)
        {
            var lista = await _context.Pecas
                .Where(p => p.Nome.Contains(termo) || p.Marca.Contains(termo))
                .ToListAsync();

            return Ok(_mapper.Map<List<PecaDto>>(lista));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PecaDto>> GetById(int id)
        {
            var item = await _context.Pecas.FindAsync(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<PecaDto>(item));
        }

        [HttpPost]
        public async Task<ActionResult> Create(PecaDto dto)
        {
            Console.WriteLine(dto.Imagem); // 👈 AQUI

            var entity = _mapper.Map<Peca>(dto);

            entity.DataCadastro = DateTime.Now;
            entity.DataUltimaAtualizacao = DateTime.Now;

            _context.Pecas.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<PecaDto>(entity);

            return CreatedAtAction(nameof(GetById),
                new { id = result.IdPeca }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PecaDto dto)
        {
            if (id != dto.IdPeca)
                return BadRequest();

            var entity = await _context.Pecas.FindAsync(id);

            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            entity.DataUltimaAtualizacao = DateTime.Now; // 👈 importante

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Pecas.FindAsync(id);

            if (entity == null)
                return NotFound();

            _context.Pecas.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadImagem(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido");

            var peca = await _context.Pecas.FindAsync(id);
            if (peca == null)
                return NotFound();

            // 📁 pasta
            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/pecas");

            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            // 🔥 nome único
            var nomeArquivo = $"{id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var caminhoCompleto = Path.Combine(pasta, nomeArquivo);
            var caminhoRelativo = $"/uploads/pecas/{nomeArquivo}";

            // 💾 salva arquivo
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var url = $"{baseUrl}{caminhoRelativo}";
            // 💾 salva no banco
            peca.Imagem = url;
            await _context.SaveChangesAsync();

            return Ok(new { imagem = url });
        }
    }
}