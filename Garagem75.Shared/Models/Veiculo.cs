using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Garagem75.Shared.Models;

[Table("Veiculo")]
public class Veiculo
{
    [Key]
    public int IdVeiculo { get; set; }

    [Required(ErrorMessage = "Campo Obrigatório!")]
    [StringLength(30)]
    public string Fabricante { get; set; }

    [Required(ErrorMessage = "Campo Obrigatório!")]
    [StringLength(75)]
    public string Modelo { get; set; }

    [Required(ErrorMessage = "Campo Obrigatório!")]
    [Range(1900, 2100, ErrorMessage = "Ano inválido!")]
    public int Ano { get; set; }

    [Required(ErrorMessage = "Campo Obrigatório!")]
    [StringLength(7)]
    public string Placa { get; set; }

    [StringLength(30)]
    public string Cor { get; set; }
    public string? FotoUrl { get; set; } // 🔥 NOVO

    public int? ClienteId { get; set; }
    public virtual Cliente? Cliente { get; set; }
}