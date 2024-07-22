using System.ComponentModel.DataAnnotations;

namespace MoeUC.Api.Models;

public class RegisterRequestModel
{
    [EmailAddress]
    public required string Email { get; set; }

    public required string Password { get; set; }
}