using System;
using BookAppoinment.Adapters.Repositories.Dtos;
using LanguageExt;

namespace BookAppoinment.Adapters.Repositories.Interfaces;

public interface IAgencyRepository
{
    //Queries
    Task<Option<AgenciesDto>> GetAgencyByAgencyIdAsync(int agencyId);

    //Commands
}

