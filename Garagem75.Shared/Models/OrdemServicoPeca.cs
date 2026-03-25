using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Garagem75.Shared.Models;

public class OrdemServicoPeca
{
    // Chave Composta: Entity Framework Core usará a combinação 
    // de OrdemServicoId e PecaId como chave primária composta
    public int OrdemServicoId { get; set; }
    public int PecaId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    [Display(Name = "Preço Unitário na OS")]
    // É importante armazenar o preço unitário aqui para que, 
    // se o preço da Peca for alterado no futuro, o histórico da OS 
    // não seja afetado.
    public decimal PrecoUnitario { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    [Display(Name = "Subtotal")]
    public decimal Subtotal => Quantidade * PrecoUnitario;

    // Propriedades de Navegação
    public OrdemServico OrdemServico { get; set; }
    public Peca Peca { get; set; }
}
