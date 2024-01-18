using ElasticDotnet.Domain.Entities;

namespace ElasticDotnet.Domain.Interfaces;


public interface IUserRepository
{
	public Task RegisterUser(User user);
	public Task<User?> FindByEmail(string email);
}