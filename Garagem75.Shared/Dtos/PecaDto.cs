using System;
using System.Collections.Generic;
using System.Text;

namespace Garagem75.Shared.Dtos
{
    public class PecaDto
    {
        public int IdPeca { get; set; }
        public int CodPeca { get; set; }
        public string Marca { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Fornecedor { get; set; }
        public int QuantidadeEstoque { get; set; }

        public string? Imagem { get; set; } // 👈 importante pro app
    }
}
