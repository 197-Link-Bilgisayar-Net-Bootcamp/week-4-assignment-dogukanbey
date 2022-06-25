using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using week4.Data.Dtos;
using week4.Service.Services;


namespace week4.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public UserController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            return Ok(await _jwtService.CreateUserAsync(createUserDto));
        }

        [HttpGet]
       [Authorize]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _jwtService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }
    }
}
