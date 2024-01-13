using elastic_dotnet.Data;
using elastic_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace elastic_dotnet.Repository;

public class UserRepository(DatabaseContext dbContext) : IUserRepository
{
	private readonly DatabaseContext _dbContext = dbContext;

	public async Task RegisterUser(User user)
	{
		await _dbContext.Users.AddAsync(user);
		await _dbContext.SaveChangesAsync();
	}
	public async Task<User?> FindByEmail(string email)
	{
		return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
	}
}