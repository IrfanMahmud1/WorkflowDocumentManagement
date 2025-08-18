using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WDM.Domain.Dtos;
using WDM.Domain.Entities;
using WDM.Domain.Services;

namespace WDM.Ui.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;

        public UserService(HttpClient httpClient, ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("users");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<User>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<User>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
            }
            return new List<User>();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id: {UserId}", id);
            }
            return null;
        }

        public async Task<bool> CreateUserAsync(CreateUserDto userDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(userDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("users", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserDto userDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(userDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"users/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", id);
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"users/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", id);
                return false;
            }
        }
    }
}
