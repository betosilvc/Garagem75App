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

        public TipoUsuarioDto? TipoUsuario { get; set; } // ✅ CORRETO

        public bool Ativo { get; set; } = true;
    }
}