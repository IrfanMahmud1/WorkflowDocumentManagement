using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDM.Domain.Dtos;
using WDM.Domain.Entities;

namespace WDM.Domain.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResult> AuthenticateAsync(LoginRequest request);
        string GenerateToken(User user);
    }
}
