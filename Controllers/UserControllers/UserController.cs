using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ufoShopBack.CRUDoperations;
using ufoShopBack.Services;
using ufoShopBack.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ufoShopBack.Controllers.UserControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UsersCRUD _usersCRUD;
        private readonly UserService _usersService;
        public UserController(UsersCRUD usersCRUD, UserService usersService)
        {
            _usersCRUD = usersCRUD;
            _usersService = usersService;
        }
        [Authorize(Roles = "Moderator,Admin")]
        [HttpGet] 
        public async Task<IActionResult> GetUsers()
        {
            var users = await _usersCRUD.GetAsync();
            return Ok(users);
        }
        [Authorize(Roles = "Moderator,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _usersCRUD.GetAsync(id);
            return user != null ? Ok(user) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]  
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!await _usersService.IsEmailUniqueAsync(user.Email))
            {
                return BadRequest("Email has already been used");
            }
            if (!await _usersService.IsUserNameUniqueAsync(user.Nickname))
            {
                return BadRequest("Nickname has already been used");
            }
            user.Password = _usersService.HashPassword(user.Password);
            await _usersCRUD.CreateAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                var existingUser = await _usersCRUD.GetAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                await _usersCRUD.UpdateAsync(user, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the user.");
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _usersCRUD.GetAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            await _usersCRUD.DeleteAsync(id);
            return NoContent();
        }
    }

}
