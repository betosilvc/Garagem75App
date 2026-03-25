using AutoMapper;
using Garagem75.Shared.Models;
using Garagem75.Shared.Dtos;

public class PecaProfile : Profile
{
    public PecaProfile()
    {
        // ENTITY -> DTO
        CreateMap<Peca, PecaDto>();

        // DTO -> ENTITY
        CreateMap<PecaDto, Peca>()
            // 👇 NÃO deixa sobrescrever datas
            .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
            .ForMember(dest => dest.DataUltimaAtualizacao, opt => opt.Ignore())

            // 👇 NÃO mexe na relação com OS
            .ForMember(dest => dest.PecasAssociadas, opt => opt.Ignore());
    }
}