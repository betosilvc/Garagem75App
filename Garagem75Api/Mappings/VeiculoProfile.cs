using AutoMapper;
using Garagem75.Shared;
using Garagem75.Shared.Dtos;
using Garagem75.Shared.Models;

public class VeiculoProfile : Profile
{
    public VeiculoProfile()
    {
        // Entity -> DTO
        CreateMap<Veiculo, VeiculoDto>()
    .ForMember(dest => dest.ClienteNome,
        opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : "Sem cliente"));

        // DTO -> Entity
        CreateMap<VeiculoDto, Veiculo>();
    }
}