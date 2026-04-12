using AutoMapper;
using Garagem75.Shared.Models;
using Garagem75.Shared.Dtos;

namespace Garagem75.Api.Mappings
{
    public class OrdemServicoProfile : Profile
    {
        public OrdemServicoProfile()
        {
            // 🔁 ENTITY → DTO
            CreateMap<OrdemServico, OrdemServicoDto>()
                .ForMember(dest => dest.PecasAssociadas,
                    opt => opt.MapFrom(src => src.PecasAssociadas))
                .ForMember(dest => dest.NomeCliente,
                    opt => opt.MapFrom(src => src.Veiculo != null && src.Veiculo.Cliente != null
                        ? src.Veiculo.Cliente.Nome
                        : ""))
                .ForMember(dest => dest.PlacaVeiculo,
                    opt => opt.MapFrom(src => src.Veiculo != null
                        ? src.Veiculo.Placa
                        : ""));

            // 🔁 PEÇA DA OS → DTO
            CreateMap<OrdemServicoPeca, OrdemServicoPecaDto>()
                .ForMember(dest => dest.NomePeca,
                    opt => opt.MapFrom(src => src.Peca != null
                        ? src.Peca.Nome
                        : ""))
                .ForMember(dest => dest.MarcaPeca, opt => opt.MapFrom(src => src.Peca.Marca)) // 🔥 Adicione esta linha
                .ForMember(dest => dest.PrecoUnitario,
                    opt => opt.MapFrom(src => src.PrecoUnitario));

            // 🔁 DTO → ENTITY (IMPORTANTE)
            CreateMap<OrdemServicoDto, OrdemServico>()
                .ForMember(dest => dest.PecasAssociadas, opt => opt.Ignore()); // 🔥 você controla manualmente

            CreateMap<OrdemServicoPecaDto, OrdemServicoPeca>();
        }
    }
}