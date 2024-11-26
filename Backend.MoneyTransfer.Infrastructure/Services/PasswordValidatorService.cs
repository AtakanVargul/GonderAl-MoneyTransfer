using Backend.MoneyTransfer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Backend.MoneyTransfer.Infrastructure.Services;

public class PasswordValidatorService : IPasswordValidator<User>
{
    private readonly IConfiguration _configuration;

    public PasswordValidatorService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IdentityResult> ValidateAsync(UserManager<User> _userManager, User user, string password)
    {
        var requiredLength = _configuration.GetValue<int>("IdentitySettings:Password:RequiredLength");
        if (password.Length != requiredLength)
        {
            return await Task.FromResult(IdentityResult.Failed(
                new IdentityError
                {
                    Code = "PasswordLengthError",
                    Description = $"Password must have {requiredLength} characters."
                }));
        }

        if (!int.TryParse(password, out _))
        {
            return await Task.FromResult(IdentityResult.Failed(
                new IdentityError
                {
                    Code = "PasswordContentError",
                    Description = $"Password must contains only numbers."
                }));
        }

        var repetitiveRegexPattern = _configuration.GetValue<string>("PasswordValidation:RepetitiveRegexPattern");
        if (Regex.IsMatch(password, repetitiveRegexPattern))
        {
            return await Task.FromResult(IdentityResult.Failed(
                  new IdentityError
                  {
                      Code = "PasswordRepetitiveCharacterError",
                      Description = $"Password can not have repetitive characters."
                  }));
        }

        var successiveRegexPattern = _configuration.GetValue<string>("PasswordValidation:SuccessiveRegexPattern");
        if (Regex.IsMatch(password, successiveRegexPattern))
        {
            return await Task.FromResult(IdentityResult.Failed(
                new IdentityError
                {
                    Code = "PasswordSuccessiveCharacterError",
                    Description = $"Password can not have successive characters."
                }));
        }

        return await Task.FromResult(IdentityResult.Success);
    }
}