using ElasticDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElasticDotnet.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}