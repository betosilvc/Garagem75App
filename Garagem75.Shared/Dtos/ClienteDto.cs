using System;
using System.Collections.Generic;
using System.Text;

namespace Garagem75.Shared.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        public List<EnderecoDto> Enderecos { get; set; } = new();


    }
}
