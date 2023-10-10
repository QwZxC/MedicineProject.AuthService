using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicineProject.AuthService.Domain.Services;
using MedicineProject.AuthService.Domain.Dtos.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MedicineProject.AuthService.Domain.Models;

namespace MedicineProject.AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private const string BAD_PASSWORD = "Пароль не соответствует ограничениям";
        private readonly IAccountService _accountService;

        public AccountsController(ITokenService tokenService, IAccountService accountService)   
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _accountService.TrimProperties(request);

            Patient? managedUser = await _accountService.FindUserByEmailAsync(request.Email);

            if (managedUser == null)
            {
                return BadRequest("Нет пользователя с данной почтой");
            }

            bool isPasswordValid = await _accountService.CheckPasswordAsync(managedUser, request.Password);

            if (!isPasswordValid)
            {
                return BadRequest("Не верный пароль");
            }

            Patient? user = await _accountService.FindByEmailInDbAsync(request.Email);

            if (user is null)
            {
                return Unauthorized();
            }

            List<long> roleIds = await _accountService.FindRoleIdsAsync(user.Id);
            List<IdentityRole<long>> roles = await _accountService.FindRolesAsync(roleIds);

            string accessToken = _tokenService.CreateToken(user, roles);
            user.RefreshToken = _accountService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = _accountService.GetRefreshTokenExpiryTime();

            await _accountService.SaveChangedAsync();

            return Ok(new AuthResponse
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = accessToken,
                RefreshToken = user.RefreshToken,
                Role = request.Role
            });
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(request);
            }

            _accountService.TrimProperties(request);

            IdentityRole<long> role = await _accountService.GetRoleByNameAsync(request.Role);

            if (role == null)
            {
                return BadRequest("Неверная роль");
            }

            Patient user = _accountService.CreateUser(request);
            IdentityResult result = await _accountService.GetResultAsync(user, request.Password);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            if (!result.Succeeded) 
            {
                return BadRequest(BAD_PASSWORD);
            }

            Patient? findUser = await _accountService.FindUserByEmailAsync(request.Email);
            if (findUser == null) 
            {
                NotFound($"Пользователь {request.Email} не найден");
            } 

            await _accountService.LinkUserRole(findUser, request.Role);

            return await Authenticate(new AuthRequest
            {
                Email = request.Email,
                Password = request.Password,
                Role = request.Role
            });
        }

        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken(TokenModel? tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Не правильный токен");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;
            ClaimsPrincipal? principal = _accountService.GetPrincipal(accessToken);

            if (principal == null)
            {
                return BadRequest("неверный токен доступа или обновления");
            }

            string? username = principal.Identity!.Name;
            Patient? user = await _accountService.FindUserByNameAsync(username!);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime >= DateTime.UtcNow)
            {
                return BadRequest("неверный токен доступа или обновления");
            }

            JwtSecurityToken newAccessToken = _accountService.CreateToken(principal);
            string? newRefreshToken = _accountService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _accountService.UpdateUserAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Revoke(string username)
        {
            Patient? user = await _accountService.FindUserByNameAsync(username);
            if (user == null) 
            {
                return NotFound("Invalid user name");
            }

            user.RefreshToken = null;
            await _accountService.UpdateUserAsync(user);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("revoke-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RevokeAll()
        {
            await _accountService.RevokeAllAsync();
            return Ok();
        }

    }
}
