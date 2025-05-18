using BookStore.API.Contracts.Users;
using BookStore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContext;

        public UsersController(IUserService userService, IHttpContextAccessor httpContext)
        {
            _userService = userService;
            _httpContext = httpContext;
        }

        [HttpPost("register")]
        public async Task<IResult> Register([FromBody] RegisterUserRequest request)
        {
            await _userService.Register(request.UserName, request.Email, request.Password);

            return Results.Ok();
        }

        [HttpPost("login")]
        public async Task<IResult> Login([FromBody]LoginUserRequest request)
        {
            var token = await _userService.Login(request.Email, request.Password);

            _httpContext.HttpContext.Response.Cookies.Append("first-cookies", token, new CookieOptions
            {
                HttpOnly = true, 
                Secure = true,
                SameSite = SameSiteMode.Lax, 
                Expires = DateTimeOffset.UtcNow.AddHours(12)
            });

            return Results.Ok();
        }
    }
}
