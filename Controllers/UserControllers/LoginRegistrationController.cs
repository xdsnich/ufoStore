using Microsoft.AspNetCore.Mvc;
using ufoShopBack.Services;
using ufoShopBack.Data;
using ufoShopBack.Data.Entities;
using ufoShopBack.CRUDoperations;
using System.Text;
using ufoShopBack.Models.UsersValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Data;

namespace ufoShopBack.Controllers.UserControllers
{
    [ApiController]
    [Route("api/loginregistration")]
    public class LoginRegistrationController : Controller
    {
        private readonly UserService _userService;
        private readonly Context _context;
        private readonly UsersCRUD usersCRUD;
        private readonly GenerateTokenService _tokenService;

        public LoginRegistrationController(UserService userService, Context context, GenerateTokenService tokenService)
        {
            _context = context;
            _userService = userService;
            _tokenService = tokenService;
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

            var roles = userFromDb.UserRoles.Select(userRole => userRole.Role.RoleName).ToList();

            var token = _tokenService.GenerateToken(userFromDb, roles);
            var response = new
            {
                access_token = token,
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