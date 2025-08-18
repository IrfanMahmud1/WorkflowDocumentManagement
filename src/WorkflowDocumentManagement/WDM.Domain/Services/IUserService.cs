using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDM.Domain.Dtos;
using WDM.Domain.Entities;

namespace WDM.Domain.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<bool> CreateUserAsync(CreateUserDto userDto);
        Task<bool> UpdateUserAsync(Guid id, UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
