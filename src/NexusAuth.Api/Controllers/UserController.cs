using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NexusAuth.Api.Models.Request;
using NexusAuth.Application.Features.Users;
using NexusAuth.Application.Features.Users.Authentication;
using NexusAuth.Application.Features.Users.RecoveryAccess;
using NexusAuth.Application.Features.Users.Registration;
using NexusAuth.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NexusAuth.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public sealed class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public UserController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var command = new RegisterCommand(request.Login, request.UserName, request.Password, request.Email, request.Phone, request.IdGender, request.IdCountry);

            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(new { Message = "Регистрация прошла успешно!" }),
                onFailure: errors =>
                {
                    if (errors.Any(e => e.Code == ErrorCode.Save))
                    {
                        var serverError = errors.FirstOrDefault(e => e.Code == ErrorCode.Save);
                        return StatusCode(StatusCodes.Status500InternalServerError, new
                        {
                            Tittle = "Внутренняя ошибка сервера",
                            Details = serverError?.ClientMessage,
                        });
                    }

                    if (errors.Any(e => e.Code == ErrorCode.Conflict))
                    {
                        var conflictError = errors.FirstOrDefault(e => e.Code == ErrorCode.Conflict);
                        return Conflict(new
                        {
                            Title = "Конфликт данных",
                            Detail = conflictError?.ClientMessage
                        });
                    }

                    return BadRequest(new
                    {
                        Title = "Произошла ошибка",
                        Errors = errors.Select(e => new
                        {
                            e.Code,
                            e.ClientMessage
                        })
                    });
                });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var command = new AuthCommand(request.Password, request.Login, request.Email);

            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(
                onSuccess: user =>
                {
                    var token = GenerateJwtToken(user);
                    return Ok(token);
                },
                onFailure: errors =>
                {
                    return Unauthorized(new
                    {
                        Title = "Не валидные данные",
                        Message = result.ClientStringMessage
                    });

                });
        }

        [HttpPost("recovery-access")]
        public async Task<IActionResult> RecoveryAccess([FromBody] RecoveryAccessRequest request)
        {
            var command = new RecoveryAccessCommand(request.Login, request.Email, request.NewPassword);

            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(
                onSuccess: Ok,
                onFailure: errors =>
                {
                    if (errors.Any(e => e.Code == ErrorCode.Server))
                    {
                        var serverError = errors.FirstOrDefault(e => e.Code == ErrorCode.Save);
                        return StatusCode(StatusCodes.Status500InternalServerError, new
                        {
                            Tittle = "Внутренняя ошибка сервера",
                            Details = serverError?.ClientMessage,
                        });
                    }
                    return BadRequest(result.ClientStringMessage);
                });
        }

        private string GenerateJwtToken(UserDto user)
        {
            var role = (Roles)user.IdRole;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName), 
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}