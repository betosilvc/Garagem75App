using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Garagem75.Shared.Models;
[Table("Cliente")]
public class Cliente
{
    [Key]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "Campo obrigatório!")]
    [Display(Name = "Nome")]
    [StringLength(75)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Campo obrigatório!")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter exatamente 11 números.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números.")]
    [Display(Name = "CPF")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "Campo obrigatório!")]
    [Display(Name = "Telefone")]
    [StringLength(15)]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "Campo obrigatório!")]
    [Display(Name = "Email")]
    [StringLength(75)]
    public string Email { get; set; }

    
    public ICollection<Endereco> Enderecos { get; set; }
    
    public ICollection<Veiculo> Veiculos { get; set; }

}


