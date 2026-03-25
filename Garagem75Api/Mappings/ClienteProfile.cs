using AutoMapper;
using Garagem75.Shared;
using Garagem75.Shared.Dtos;
using Garagem75.Shared.Models;
using static System.Net.Mime.MediaTypeNames;

public class ClienteProfile : Profile
{
    public ClienteProfile()
    {
        CreateMap<Cliente, ClienteDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCliente)); ;
        CreateMap<ClienteDto, Cliente>();
    }
}
