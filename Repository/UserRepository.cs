using elastic_dotnet.Data;
using elastic_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace elastic_dotnet.Repository;

public class UserRepository(DatabaseContext dbContext) : IUserRepository
{
	public async Task RegisterUser(User user)
	{
		await dbContext.Users.AddAsync(user);
		await dbContext.SaveChangesAsync();
	}
	public async Task<User?> FindByEmail(string email)
	{
		return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
	}
}