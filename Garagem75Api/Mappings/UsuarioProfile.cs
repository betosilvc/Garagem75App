using AutoMapper;
using Garagem75.Shared.Dtos;
using Garagem75.Shared.Models;

namespace Garagem75.Api.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>();

            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.TipoUsuario, opt => opt.Ignore());
        }
    }
}
