using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

            // Validate email, phone number and password
            var validateResponse = ValidateLogin(dto.EmailOrPhoneNumber, dto.Password);
            if (validateResponse == null)
            {
                if (dto.UserRoleData == "Tenant")
                {
                    var response = await _loginRepo.UserExistsOrNot(dto.EmailOrPhoneNumber);

                    if (response != null)
                    {
                        return new LoginResponseDTO
                        {
                            id = response.Id,
                            message = null
                        };
                    }
                    else
                    {
                        return new LoginResponseDTO
                        {
                            id = new Guid(),
                            message = "User Not Found"
                        };
                    }
                }
                else if (dto.UserRoleData == "Owner")
                {
                    var response = await _loginRepo.GetUserId(dto.EmailOrPhoneNumber, dto.Password);

                    if (response != null)
                    {
                        return new LoginResponseDTO
                        {
                            id = response.HostelId,
                            message = null
                        };
                    }
                    else
                    {
                        return new LoginResponseDTO
                        {
                            id = new Guid(),
                            message = "User Not Found"
                        };
                    }
                }
                else
                    return new LoginResponseDTO
                    {
                        id = new Guid(),
                        message = "Select the Valid Role"
                    };
            }
            else
            {
                return validateResponse;
            }
        }

        public async Task<LoginResponseDTO> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            // Validate email, phone number and password
            var validateResponse = ValidateLogin(dto.EmailOrPhone, dto.CurrentPassword);
            if (validateResponse == null)
            {
                var user = await _loginRepo.GetUserId(dto.EmailOrPhone, dto.CurrentPassword);
                if (user != null)
                {
                    user.PasswordHash = dto.NewPassword;
                    user.CreatedAt = user.CreatedAt.ToUniversalTime();
                    var isSuccess = await _loginRepo.UpdateUserAsync(user);
                    if (isSuccess)
                    {
                        return new LoginResponseDTO
                        {
                            id = new Guid(),
                            message = "Password updated Succesfully"
                        };
                    }
                    else
                    {
                        return new LoginResponseDTO
                        {
                            id = new Guid(),
                            message = "Unable to update the password please try again after sometime"
                        };
                    }
                }
                else
                {
                    return new LoginResponseDTO
                    {
                        id = new Guid(),
                        message = "User Not Found"
                    };
                }
            }
            else
            {
                return validateResponse;
            }
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

        public LoginResponseDTO? ValidateLogin(string emailOrPhone, string password)
        {
            if (string.IsNullOrEmpty(emailOrPhone) && string.IsNullOrEmpty(password))
            {
                return new LoginResponseDTO
                {
                    id = new Guid(),
                    message = "Please enter the Email id & password"
                };
            }
            if (!IsValid(emailOrPhone))
            {
                return new LoginResponseDTO
                {
                    id = new Guid(),
                    message = "Invalid Email or Password"
                };
            }
            if (emailOrPhone == null || string.IsNullOrEmpty(emailOrPhone))
            {
                return new LoginResponseDTO
                {
                    id = new Guid(),
                    message = "Please enter the Email id"
                };
            }
            if (password == null || string.IsNullOrEmpty(password))
            {
                return new LoginResponseDTO
                {
                    id = new Guid(),
                    message = "Please enter the Password"
                };
            }
            return null;
        }
    }
}
