using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Garagem75.Shared.Models;

public class BlogPost
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Titulo { get; set; }

    [StringLength(300)]
    public string Resumo { get; set; }

    [Required]
    public string Conteudo { get; set; }

    [StringLength(200)]
    public string ImagemUrl { get; set; }

    public DateTime DataPublicacao { get; set; } = DateTime.Now;

    public bool Ativo { get; set; } = true;
}
