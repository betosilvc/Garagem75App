using Garagem75.Api.Data;
using Garagem75.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly Garagem75DBContext _context;

    public DashboardController(Garagem75DBContext context)
    {
        _context = context;
    }

    [HttpGet("faturamento-dia")]
    public async Task<decimal> GetFaturamentoDia()
    {
        var hoje = DateTime.Today;

        // Buscamos as OS do dia incluindo as peças para o cálculo ser real
        var ordensDoDia = await _context.OrdemServicos
            .Include(x => x.PecasAssociadas)
            .Where(x => x.DataServico.Date == hoje)
            .ToListAsync();

        // Recalcula tudo na memória para garantir que o valor está certo
        var total = ordensDoDia.Sum(os =>
            (os.MaoDeObra + os.PecasAssociadas.Sum(p => p.Quantidade * p.PrecoUnitario)) - os.ValorDesconto
        );

        return total;
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

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboard()
    {
        var dto = new DashboardDto();

        // ===== CARDS =====
        dto.TotalPecas = await _context.Pecas.CountAsync();
        dto.TotalClientes = await _context.Clientes.CountAsync();
        dto.TotalOrdensServico = await _context.OrdemServicos.CountAsync();
        dto.TotalUsuarios = await _context.Usuarios.CountAsync();

        int diff = (7 + (int)DateTime.Now.DayOfWeek - (int)DayOfWeek.Monday) % 7;
        var inicioDaSemana = DateTime.Now.Date.AddDays(-diff);

        dto.ValorTotalOrdensServico = await _context.OrdemServicos
            .Where(o => o.DataServico >= inicioDaSemana)
            .SumAsync(o => (decimal?)o.ValorTotal) ?? 0;

        // ===== LISTAS =====

        dto.ClientesMaisAntigos = await _context.Clientes
            .OrderBy(c => c.IdCliente)
            .Take(5)
            .Select(c => new ClienteCount
            {
                IdCliente = c.IdCliente,
                Nome = c.Nome,
                Telefone = c.Telefone,
                Email = c.Email
            }).ToListAsync();

        dto.UltimosVeiculosAtendidos = await _context.Veiculos
            .OrderByDescending(v => v.IdVeiculo)
            .Take(5)
            .Select(v => new ModeloVeiculos
            {
                IdVeiculo = v.IdVeiculo,
                Fabricante = v.Fabricante,
                Modelo = v.Modelo,
                Placa = v.Placa
            }).ToListAsync();

        dto.UltimasPecas = await _context.Pecas
            .OrderByDescending(p => p.IdPeca)
            .Take(5)
            .Select(p => new PecaItem
            {
                IdPeca = p.IdPeca,
                Nome = p.Nome,
                Marca = p.Marca,
                Preco = p.Preco
            }).ToListAsync();

        dto.MarcasPecasMaisUsadas = await _context.Pecas
            .GroupBy(p => p.Marca)
            .Select(g => new MarcaQuantidadeViewModel
            {
                NomeMarca = g.Key,
                Quantidade = g.Count()
            })
            .OrderByDescending(x => x.Quantidade)
            .Take(5)
            .ToListAsync();

        dto.MarcasVeiculosMaisUsadas = await _context.Veiculos
            .GroupBy(v => v.Fabricante)
            .Select(g => new MarcaQuantidadeViewModel
            {
                NomeMarca = g.Key,
                Quantidade = g.Count()
            })
            .OrderByDescending(x => x.Quantidade)
            .Take(5)
            .ToListAsync();

        dto.PecasPorVeiculo = await _context.OrdemServicos
            .Select(o => new PecasPorVeiculo
            {
                IdOrdemServico = o.IdOrdemServico,
                Modelo = o.Veiculo.Modelo,
                QuantidadePecas = o.PecasAssociadas.Count
            })
            .OrderByDescending(o => o.QuantidadePecas)
            .Take(5)
            .ToListAsync();

        return Ok(dto);
    }
}