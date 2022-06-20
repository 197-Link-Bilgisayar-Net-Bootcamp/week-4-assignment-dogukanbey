using Microsoft.AspNetCore.Mvc;
using week4.Service.Services;
using week4.Data.Dtos;

namespace week4.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthenticationController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<string> CreateToken(LoginDto loginDto)
        {
            var result = await _jwtService.CreateTokenAsync(loginDto);

            return result.StatusCode;
        }

 
        [HttpPost]
        public string CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result = _jwtService.CreateTokenByClient(clientLoginDto);

            return result.StatusCode;
        }
        [HttpPost]
        public async Task<string> RevokeRefreshToken(string refreshTokenDto)
        {
            var result = await _jwtService.RevokeRefreshTokenAsync(refreshTokenDto);

            return result.StatusCode;
        }



        [HttpPost]
        public async Task<string> CreateTokenByRefreshToken(string refreshTokenDto)
        {
            var result = await _jwtService.CreateTokenByRefreshTokenAsync(refreshTokenDto);
            return result.StatusCode;
        }
    }
}
