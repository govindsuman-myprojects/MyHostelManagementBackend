using AutoMapper;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.DTOs;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyHostelManagement.Services.Implementations
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepo;

        public LoginService(ILoginRepository loginRepo)
        {
            _loginRepo = loginRepo;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDto dto)
        {
            
            if (dto == null)
            {
                return new LoginResponseDTO
                {
                    id = new Guid(),
                    message = "Please enter the Email id & password"
                };
            }
            if (!IsValid(dto.Email))
            {
                return new LoginResponseDTO
                {
                    id = new Guid(),
                    message = "Please enter the Email id or Phone number"
                };
            }
            if (dto.Email == null || string.IsNullOrEmpty(dto.Email))
            {
                return new LoginResponseDTO
                {
                    id = new Guid(),
                    message = "Please enter the Email id"
                };
            }
            if (dto.Password == null || string.IsNullOrEmpty(dto.Password))
            {
                return new LoginResponseDTO
                {
                    id = new Guid(),
                    message = "Please enter the Password"
                };
            }
            var response = await _loginRepo.LoginAsync(dto);
            return response;
        }

        public Boolean IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
                return true;

            if (Regex.IsMatch(value, @"^\+?[1-9]\d{7,14}$"))
                return true;

            return false;

        }
    }
}
