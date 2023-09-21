using Microsoft.EntityFrameworkCore;

namespace BookAppoinment.Adapters;

public class MySqlDbContext : QwiikDataContext
{
    private readonly IConfiguration _configuration;

    public MySqlDbContext(DbContextOptions<MySqlDbContext> options, IConfiguration configuration) : base(options) =>
        _configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var useRetryPolicy = _configuration.GetSection("DbConnectionPolicy").GetValue<bool>("RetryOnFailure");
        var timeout = _configuration.GetSection("DbConnectionPolicy").GetValue<int>("Timeout");
        var maxRetryCount = _configuration.GetSection("DbConnectionPolicy").GetValue<int>("MaxRetryCount");

        optionsBuilder.UseMySql(
                _configuration.GetConnectionString("ApiDatabase"),
                ServerVersion.AutoDetect(_configuration.GetConnectionString("ApiDatabase")),
                options =>
                {
                    if (useRetryPolicy)
                        options.EnableRetryOnFailure(maxRetryCount);
                    if (timeout > 0)
                        options.CommandTimeout(timeout);
                })
            ;
        optionsBuilder.EnableSensitiveDataLogging();
    }
}