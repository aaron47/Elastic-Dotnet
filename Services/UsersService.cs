using elastic_dotnet.Models;
using elastic_dotnet.Repository;
using elastic_dotnet.Services;
using elastic_dotnet.Utils;
using elastic_dotnet.Validators;

namespace elastic_dotnet;

public class UsersService(IUserRepository userRepository, IJwtService jwtService) : IUsersService
{
	private readonly IUserRepository _userRepository = userRepository;
	private readonly IJwtService _jwtService = jwtService;

	public async Task<ServiceResult<string>> Register(AuthDTO authDto)
	{
		var authValidator = new AuthValidator();
		var result = authValidator.Validate(authDto);
		var dbUser = await _userRepository.FindByEmail(authDto.Email);

		if (!result.IsValid)
		{
			return ServiceResult<string>.FailureResult(result.Errors.Select(e => e.ErrorMessage));
		}

		if (dbUser is not null)
		{
			return ServiceResult<string>.FailureResult(["Email already in use."]);
		}


		var user = new User
		{
			Email = authDto.Email,
			Password = HashPassword(authDto.Password)
		};

		await _userRepository.RegisterUser(user);
		return ServiceResult<string>.SuccessResult(authDto.Email, "User registered successfully.");
	}

	public async Task<ServiceResult<string>> Login(AuthDTO authDto)
	{
		var user = await _userRepository.FindByEmail(authDto.Email);

		if (user is null)
		{
			return ServiceResult<string>.FailureResult(["User does not exist."]);
		}

		var token = _jwtService.GenerateToken(authDto);
		return ServiceResult<string>.SuccessResult(token, "Logged in successfully");
	}

	private static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
}