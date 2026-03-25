using AutoMapper;
using Garagem75.Shared.Models;

namespace Garagem75.Api.Mappings
{
    public class EnderecoProfile : Profile
    {
        public EnderecoProfile() 
        {
            CreateMap<Endereco, EnderecoDto>()
                .ForMember(dest => dest.IdEndereco, opt => opt.MapFrom(src => src.IdEndereco));
            CreateMap<EnderecoDto, Endereco>();

        }
    }
}
