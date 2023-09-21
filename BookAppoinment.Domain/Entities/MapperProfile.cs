using System;
using AutoMapper;
using BookAppoinment.Adapters.Repositories.Dtos;
using BookAppoinment.Domain.Entities.Agencies.Queries;

namespace BookAppoinment.Domain.Entities;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AgenciesDto, AgencyDetailResponse>();
    }
}