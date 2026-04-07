using System;
using System.Collections.Generic;
using System.Text;

namespace Garagem75.Shared.Dtos
{

    public class EnderecoDto
    {
        public int IdEndereco { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
    }
}