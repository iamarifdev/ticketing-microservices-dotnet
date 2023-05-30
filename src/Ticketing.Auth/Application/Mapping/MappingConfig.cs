using Mapster;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Domain.Entities;

namespace Ticketing.Auth.Application.Mapping;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<User, AuthResponse>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName);

        TypeAdapterConfig<SignupRequest, User>.NewConfig()
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Password, src => src.Password);
    }
}