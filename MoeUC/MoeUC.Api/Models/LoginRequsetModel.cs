using System.ComponentModel.DataAnnotations;

namespace MoeUC.Api.Models;

public class LoginRequestModel
{
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string AuthString { get; set; } = null!;
}