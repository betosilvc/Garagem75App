using Garagem75.Shared.Models;

namespace Garagem75.Shared.Dtos
{
    public class OrdemServicoDto
    {
        public int IdOrdemServico { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataServico { get; set; }
        public decimal MaoDeObra { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorTotal { get; set; } // calculado
        public string? Status { get; set; }
        public DateTime DataEntrega { get; set; }
        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }
        public string? Observacao { get; set; }
        public string? NomeCliente { get; set; }
        public string? PlacaVeiculo { get; set; }
        public string? VeiculoModelo { get; set; }

        public List<OrdemServicoPecaDto> PecasAssociadas { get; set; } = new();

        

    }
}