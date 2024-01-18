using ElasticDotnet.Domain.Models;
using MediatR;

namespace ElasticDotnet.Application.User.Commands;

public record LoginUserCommand(AuthDTO AuthDto) : IRequest<ServiceResult<string>>;