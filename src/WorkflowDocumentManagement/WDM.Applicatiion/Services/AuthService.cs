using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDM.Applicatiion.Interfaces;
using WDM.Domain.Dtos;
using WDM.Domain.Entities;
using WDM.Domain.Repositories;
using WDM.Domain.Services;

namespace WDM.Applicatiion.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _adminRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _adminRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(LoginRequest request)
        {
            var user = await _adminRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    Message = "Invalid credentials"
                };
            }

            var isValidPassword = await _adminRepository.ValidateCredentialsAsync(request.Email, request.Password);

            if (!isValidPassword)
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    Message = "Invalid credentials"
                };
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult
            {
                IsSuccess = true,
                Token = token,
                User = user,
                Message = "Authentication successful"
            };
        }

        public string GenerateToken(User user)
        {
            return _jwtTokenGenerator.GenerateToken(user);
        }
    }
}
