using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using week4.Data.UnitOfWork;
using week4.Data.Repositories;
using week4.Data.Models;
using week4.Data.Dtos;

namespace week4.Service.Services
{
    public interface IJwtService
    {
       

        Task<TokenDto> CreateTokenAsync(LoginDto loginDto);

        TokenDto CreateToken(UserApp user);

        Task<TokenDto> RevokeRefreshTokenAsync(string refreshToken);

        Task<TokenDto> CreateTokenByRefreshTokenAsync(string refreshToken);

        TokenDto CreateTokenByClient(ClientLoginDto clientLoginDto);

        Task <TokenDto> CreateUserAsync(CreateUserDto createUserDto);
        Task <TokenDto> GetUserByNameAsync(string userName);

 
        IEnumerable<Claim> GetClaims(UserApp user, List<string> audiences);
        IEnumerable<Claim> GetClaimsByClient(Client client);
        string CreateRefreshToken();
   

 
    }
}
