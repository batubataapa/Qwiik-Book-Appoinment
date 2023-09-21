using System;
using BookAppoinment.Adapters.Repositories.Dtos;
using BookAppoinment.Adapters.Repositories.Interfaces;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace BookAppoinment.Adapters.Repositories;

public class AgencyRepository<T> : IAgencyRepository where T : QwiikDataContext
{
    private readonly QwiikDataContext _context;

    public AgencyRepository(T context)
    {
        _context = context;
    }

    //Queries
    public async Task<Option<AgenciesDto>> GetAgencyByAgencyIdAsync(int agencyId)
    {
        return Optional(await _context.Agencies.Where(Dtos => Dtos.Id == agencyId).FirstOrDefaultAsync());
    }

    //Commands
}

