using Garagem75.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garagem75.Shared.Dtos
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int TipoUsuarioId { get; set; }
        public virtual TipoUsuario? TipoUsuario { get; set; }

        //Propriedade para softdelete
        public bool Ativo { get; set; } = true;
    }
}
