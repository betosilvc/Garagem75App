using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garagem75.Shared.Models
{
    public class ViaCepResponse
    {
        public string? logradouro { get; set; }
        public string? bairro { get; set; }
        public string? localidade { get; set; }
        public string? uf { get; set; }
        public bool erro { get; set; }
    }
}
