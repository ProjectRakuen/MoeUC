using Microsoft.Extensions.Configuration;
using MoeUC.Core.Infrastructure.Dependency;

namespace MoeUC.Service.Auth;

public class JwtHelper : IScoped
{
    private readonly IConfiguration _configuration;

    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;


    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;

        _secretKey = _configuration["Jwt:Issuer"]!;
        _issuer = _configuration["Jwt:Issuer"]!;
        _audience = _configuration["Jwt:Audience"]!;
    }

    public string Create(int userId)
    {
        throw new NotImplementedException();
    }
}