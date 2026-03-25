using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Garagem75.Shared.Models;

[Table("TipoUsuario")]
public class TipoUsuario
{
    [Key]
    public int IdTipoUsuario { get; set; }

    [Required(ErrorMessage = "Campo obrigatório!")]
    [Display(Name = "Tipo de Usuário")]
    [StringLength(50)]
    public string DescricaoTipoUsuario { get; set; }
    public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
