namespace elastic_dotnet.Repository;
using elastic_dotnet.Models;


public interface IUserRepository
{
	public Task RegisterUser(User user);
	public Task<User?> FindByEmail(string email);
}