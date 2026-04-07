using Garagem75.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garagem75.Shared.Dtos
{
     public class ClienteCadastroDto
    {
        public ClienteDto Cliente { get; set; } = new();
        public EnderecoDto Endereco { get; set; } = new();
       // public VeiculoDto Veiculo { get; set; } = new();
    }
}
