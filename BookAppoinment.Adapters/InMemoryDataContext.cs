using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookAppoinment.Adapters;

public class InMemoryDataContext : QwiikDataContext
{
    private readonly IConfiguration _configuration;

    public InMemoryDataContext(DbContextOptions<InMemoryDataContext> options, IConfiguration configuration) : base(options) =>
        _configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(_configuration["ApiDatabase"]);
        optionsBuilder.EnableDetailedErrors();
    }
}