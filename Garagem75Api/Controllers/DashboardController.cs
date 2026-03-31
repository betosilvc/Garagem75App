using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garagem75.Api.Data;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly Garagem75DBContext _context;

    public DashboardController(Garagem75DBContext context)
    {
        _context = context;
    }

    // 💰 FATURAMENTO DO DIA
    [HttpGet("faturamento-dia")]
    public async Task<decimal> GetFaturamentoDia()
    {
        var hoje = DateTime.Today;

        return await _context.OrdemServicos
            .Where(x => x.DataServico.Date == hoje)
            .SumAsync(x => (decimal?)x.ValorTotal) ?? 0;
    }

    // 🚗 FABRICANTES
    [HttpGet("fabricantes")]
    public async Task<IActionResult> GetFabricantes()
    {
        var dados = await _context.Veiculos
            .GroupBy(v => v.Fabricante)
            .Select(g => new {
                nome = g.Key,
                total = g.Count()
            })
            .ToListAsync();

        return Ok(dados);
    }

    // 🔧 MARCAS DE PEÇAS
    [HttpGet("marcas-pecas")]
    public async Task<IActionResult> GetMarcasPecas()
    {
        var dados = await _context.Pecas
            .GroupBy(p => p.Marca)
            .Select(g => new {
                nome = g.Key,
                total = g.Count()
            })
            .ToListAsync();

        return Ok(dados);
    }
}