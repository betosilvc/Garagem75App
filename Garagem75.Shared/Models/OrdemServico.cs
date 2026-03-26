using Garagem75.Shared.Models;
using System.ComponentModel.DataAnnotations;

public class OrdemServico
{
    [Key]
    public int IdOrdemServico { get; set; }

    [Required]
    public string Descricao { get; set; }

    public DateTime DataServico { get; set; } = DateTime.Now;

    public decimal MaoDeObra { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal ValorTotal { get; set; }

    public string Status { get; set; } = "Aberta";

    public DateTime DataEntrega { get; set; }

    // 🔥 RELAÇÕES
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    public int VeiculoId { get; set; }
    public Veiculo? Veiculo { get; set; }

    public ICollection<OrdemServicoPeca> PecasAssociadas { get; set; }
        = new List<OrdemServicoPeca>();
}