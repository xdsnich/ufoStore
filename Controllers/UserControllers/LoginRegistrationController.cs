using Microsoft.AspNetCore.Mvc;
using ufoShopBack.Services;
using ufoShopBack.Data;
using ufoShopBack.Data.Authentication;
using ufoShopBack.Data.Entities;
using ufoShopBack.CRUDoperations;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ufoShopBack.Models.UsersValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ufoShopBack.Controllers.UserControllers
{
    [ApiController]
    [Route("api/loginregistration")]
    public class LoginRegistrationController : Controller
    {
        private readonly UserService _userService;
        private readonly Context _context;
        private readonly UsersCRUD usersCRUD;

        public LoginRegistrationController(UserService userService, Context context)
        {
            _context = context;
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest user, UsersCRUD usersCRUD)
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }
            var userFromDb = await usersCRUD.GetByEmailAsync(user.Email);
            if (userFromDb == null || !_userService.VerifyPassword(userFromDb.Password, user.Password))
            {
                return Unauthorized(new { message = "invalid email or password" });
            }
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userFromDb.Email) };
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.ISSUER,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(2)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            Console.WriteLine($"Generated JWT: {encodedJwt}");

            var response = new
            {
                access_token = encodedJwt,
                username = userFromDb.Email
            };
            return Json(response);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] User user, UsersCRUD usersCRUD)
        {
            if(user == null)
            {
                return BadRequest("invalid data was entered");
            }
            var result = await _userService.IsUserNameUniqueAsync(user.Nickname);
            if (!result) return BadRequest(new { message = "username has already been taken" });

            result = await _userService.IsEmailUniqueAsync(user.Email);
            if (!result) return BadRequest(new { message = "email has already been taken" });
            await usersCRUD.CreateAsync(user);
            return Created(string.Empty, user);
        }
    }
   
}