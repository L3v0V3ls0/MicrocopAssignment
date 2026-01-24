using Core.DTOs.Request;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Services.Interfaces;
using Core.DTOs.Response;
using Core.Errors;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/User/Create
        [HttpPost("create")]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest dto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET: api/User/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // GET: api/User/by-username/{username}
        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<UserResponse>> GetUserByUserName(string username)
        {
            var user = await _userService.GetUserByUserNameAsync(username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // PUT: api/User/{id}
        [HttpPut("update/{id:guid}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UpdateUserRequest dto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, dto);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: api/User/validate-password
        [HttpPost("validate-password")]
        public async Task<ActionResult> ValidatePassword([FromBody] ValidatePasswordRequest dto)
        {
            var isValid = await _userService.ValidatePasswordAsync(dto.UserName, dto.Password);
            if (!isValid) return Unauthorized(new { message = UserMessages.InvalidUsernamePassword });
            return Ok(new { message = UserMessages.PasswordOK });
        }
    }
}
