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
using Microsoft.Extensions.Options;

namespace week4.Service.Services
{
    public class JwtService : IJwtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOptions _tokenOptions;
        private readonly List<Client> _clients;
       

        public JwtService(IOptions<List<Client>> optionsClient,
         IUnitOfWork unitOfWork, IOptions<CustomTokenOptions> options,
         IGenericRepository<UserRefreshToken> userRefreshTokenService, UserManager<UserApp> userManager)
        {
            _clients = optionsClient.Value;
            _tokenOptions = options.Value;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }



       public async Task<TokenDto> CreateTokenAsync(LoginDto loginDto)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var token = CreateToken(user);
            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id.ToString()).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id.ToString(),
                    Code = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }


            await _unitOfWork.CommitAsync();
            token.isSuccessful = true;
            token.StatusCode = "200";

            return token;

        }


        public TokenDto CreateToken(UserApp user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(user, _tokenOptions.Audience),
                signingCredentials: signingCredentials
                );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };
            return tokenDto;
        }

        public IEnumerable<Claim> GetClaims(UserApp user , List<string> audiences)
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return userList;
        }

        public IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()));

            return claims;
        }

        public string CreateRefreshToken()
        {
            var numberByte = new byte[32];

            using var rnd = RandomNumberGenerator.Create();

            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }

        public async Task<TokenDto> RevokeRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            TokenDto tdo = new TokenDto();
            tdo.RefreshToken = refreshToken;
            if (existRefreshToken == null)
            {
                tdo.StatusCode = "404";
                tdo.message = "Refresh token not found";
                tdo.isSuccessful = true;
                return tdo;
            }
            else
            {
                _userRefreshTokenService.Delete(existRefreshToken);

                await _unitOfWork.CommitAsync();
                tdo.isSuccessful = true;
                tdo.StatusCode = "200";
                return tdo;
            }



        }

        public async Task<TokenDto> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            TokenDto tdo = new TokenDto();
            if (existRefreshToken == null)
            {
                tdo.StatusCode = "404";
                tdo.message = "Refresh token not found";
                tdo.isSuccessful = true;
                return tdo;
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

            if (user == null)
            {
                tdo.StatusCode = "404";
                tdo.message = "User Id not found";
                tdo.isSuccessful = true;
                return tdo;

            }

            var tokenDto = CreateToken(user);

            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();


            tdo.isSuccessful = true;
            tdo.StatusCode = "200";
            return tdo;
        }

        public TokenDto CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            TokenDto tdo = new TokenDto();
            var _client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId &&
            x.Secret == clientLoginDto.ClientSecret);
            if (_client == null)
            {
                tdo.StatusCode = "404";
                tdo.message = "ClientId or ClientSecret not found";
                tdo.isSuccessful = true;
                return tdo;

            }
            var token = CreateTokenByClient(new ClientLoginDto { ClientId = _client.Id, ClientSecret = _client.Secret });
            tdo.StatusCode = "200";
            tdo.isSuccessful = true;
            return tdo;

        }

        public async Task<TokenDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp { Email = createUserDto.Email, UserName = createUserDto.UserName };
            TokenDto tdo = new TokenDto();
            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                tdo.isSuccessful = true;
                tdo.StatusCode = "400";
                //its a disaster
                foreach(var e in errors)
                {
                    tdo.message = tdo.message + " " + e;
                }
                return tdo;
            }

            tdo.isSuccessful = true;
            tdo.StatusCode = "200";
            return tdo;


        }

        public async Task<TokenDto> GetUserByNameAsync(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            TokenDto tdo = new TokenDto();
            if (user == null)
            {
                tdo.isSuccessful = true;
                tdo.StatusCode = "400";
                tdo.message = "UserName not found";
                return tdo;
            }

            tdo.isSuccessful = true;
            tdo.StatusCode = "200";
            return tdo;
        }

    }
}
