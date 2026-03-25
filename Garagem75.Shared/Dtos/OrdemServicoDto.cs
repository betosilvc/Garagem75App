public class OrdemServicoDto
{
    public int IdOrdemServico { get; set; }
    public string Descricao { get; set; }
    public DateTime DataServico { get; set; }
    public decimal MaoDeObra { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal ValorTotal { get; set; } // calculado
    public string Status { get; set; }
    public DateTime DataEntrega { get; set; }
    public int VeiculoId { get; set; }

    public List<OrdemServicoPecaDto> Pecas { get; set; } = new();
}