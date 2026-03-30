using AutoMapper;
using Garagem75.Shared.Dtos;
using Garagem75.Shared.Models;

namespace Garagem75.Api.Mappings
{
    public class TipoUsuarioProfile : Profile
    {
        public TipoUsuarioProfile()
        {
            CreateMap<TipoUsuario, TipoUsuarioDto>().ReverseMap();
        }
    }
}