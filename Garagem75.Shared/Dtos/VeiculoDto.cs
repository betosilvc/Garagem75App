using System;
using System.Collections.Generic;
using System.Text;

namespace Garagem75.Shared.Dtos
{
    public class VeiculoDto
    {
        public int IdVeiculo { get; set; }
        public string Fabricante { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public string Placa { get; set; }
        public string Cor { get; set; }

        public string? FotoUrl { get; set; }
        public int ClienteId { get; set; }
        public string? ClienteNome { get; set; }
    }
}
