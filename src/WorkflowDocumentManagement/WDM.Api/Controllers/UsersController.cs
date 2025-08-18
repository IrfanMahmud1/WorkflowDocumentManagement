using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDM.Domain.Dtos;
using WDM.Domain.Entities;
using WDM.Domain.Repositories;
using WDM.Infrastructure;

namespace WDM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = userDto.Email,
                    Password = userDto.Password, // In production, hash the password
                    UserName = userDto.UserName,
                    AccessLevel = userDto.AccessLevel,
                    CreatedBy = userDto.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _userRepository.AddAsync(user);
                if (result)
                {
                    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
                }
                return BadRequest("Failed to create user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUser = await _userRepository.GetByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                existingUser.Email = userDto.Email;
                existingUser.UserName = userDto.UserName;
                existingUser.AccessLevel = userDto.AccessLevel;
                existingUser.IsActive = userDto.IsActive;

                var result = await _userRepository.UpdateAsync(existingUser);
                if (result)
                {
                    return NoContent();
                }
                return BadRequest("Failed to update user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                var result = await _userRepository.DeleteAsync(existingUser.Id);
                if (result)
                {
                    return NoContent();
                }
                return BadRequest("Failed to delete user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
