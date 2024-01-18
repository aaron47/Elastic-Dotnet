using ElasticDotnet.Domain.Models;
using MediatR;

namespace ElasticDotnet.Application.Authentication.Commands;

public record GenerateTokenCommand(AuthDTO AuthDto) : IRequest<string>;