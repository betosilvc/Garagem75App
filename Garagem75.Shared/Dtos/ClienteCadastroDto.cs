using Garagem75.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garagem75.Shared.Dtos
{
     class ClienteCadastroDto
    {
        public Cliente Cliente { get; set; } = new Cliente();
        public Endereco Endereco { get; set; } = new Endereco();
        public Veiculo Veiculo { get; set; } = new Veiculo();
    }
}
