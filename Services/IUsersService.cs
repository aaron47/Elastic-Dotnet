using elastic_dotnet.Models;
using elastic_dotnet.Utils;

namespace elastic_dotnet.Services;

public interface IUsersService
{
	public Task<ServiceResult<string>> Register(AuthDTO authDto);
	public Task<ServiceResult<string>> Login(AuthDTO authDto);
}