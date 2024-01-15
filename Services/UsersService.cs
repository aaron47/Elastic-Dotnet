using elastic_dotnet.Models;
using elastic_dotnet.Repository;
using elastic_dotnet.Utils;
using elastic_dotnet.Validators;

namespace elastic_dotnet.Services;

public class UsersService(IUserRepository userRepository, IJwtService jwtService) : IUsersService
{
    public async Task<ServiceResult<string>> Register(AuthDTO authDto)
    {
        var authValidator = new AuthValidator();
        var result = await authValidator.ValidateAsync(authDto);
        var dbUser = await userRepository.FindByEmail(authDto.Email);

        if (!result.IsValid)
        {
            return ServiceResult<string>.FailureResult(result.Errors.Select(e => e.ErrorMessage));
        }

        if (dbUser is not null)
        {
            var errors = new[] { "Email already in use." };
            return ServiceResult<string>.FailureResult(errors);
        }


        var user = new User
        {
            Email = authDto.Email,
            Password = HashPassword(authDto.Password)
        };

        await userRepository.RegisterUser(user);
        return ServiceResult<string>.SuccessResult(authDto.Email, "User registered successfully.");
    }

    public async Task<ServiceResult<string>> Login(AuthDTO authDto)
    {
        var user = await userRepository.FindByEmail(authDto.Email);

        if (user is null)
        {
            var errors = new[] { "User does not exist." };
            return ServiceResult<string>.FailureResult(errors);
        }

        var token = jwtService.GenerateToken(authDto);
        return ServiceResult<string>.SuccessResult(token, "Logged in successfully");
    }

    private static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
}