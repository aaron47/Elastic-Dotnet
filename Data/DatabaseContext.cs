using elastic_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace elastic_dotnet.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
}