using System;
using System.Collections.Generic;
using System.Text;

namespace Garagem75.Shared.Dtos
{
    public class DashboardDto
    {
        // cards
        public int TotalPecas { get; set; }
        public int TotalClientes { get; set; }
        public int TotalOrdensServico { get; set; }
        public int TotalUsuarios { get; set; }
        public int PecasPorMarca { get; set; }
        public decimal ValorTotalOrdensServico { get; set; }


        // Listas
        public List<PecaItem> UltimasPecas { get; set; } = new();
        public List<ModeloVeiculos> UltimosVeiculosAtendidos { get; set; } = new();
        public List<ClienteCount> ClientesMaisAntigos { get; set; } = new();

        public List<PecasPorVeiculo> PecasPorVeiculo { get; set; } = new();

        // Gráficos
        public List<ClienteCount> ClientePorOrdemServico { get; set; } = new();
        public List<MarcaQuantidadeViewModel> MarcasPecasMaisUsadas { get; set; }
        public List<MarcaQuantidadeViewModel> MarcasVeiculosMaisUsadas { get; set; }

    }


    // ====== TIPOS DE APOIO (no MESMO namespace) ======

    public class MarcaQuantidadeViewModel
    {
        public string NomeMarca { get; set; }
        public int Quantidade { get; set; }
    }
    public class PecaItem
    {
        public int IdPeca { get; set; }
        public string Marca { get; set; } = "";
        public string Nome { get; set; } = "";
        public decimal Preco { get; set; }

    }

    public class ClienteCount
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class ModeloVeiculos
    {
        public int IdVeiculo { get; set; }
        public string Fabricante { get; set; } = "";
        public string Modelo { get; set; } = "";
        public string Placa { get; set; } = "";
    }

    public class PecasPorVeiculo
    {
        public int IdOrdemServico { get; set; }
        public string Modelo { get; set; } = "";
        public int QuantidadePecas { get; set; }
    }

    public class ValoresOrdensServico
    {
        public int IdOrdemServico { get; set; }
        public decimal ValorTotal { get; set; }
    }


}
