using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Service.ServiceBase.Caching;

namespace MoeUC.Service.Auth;

public class AuthHelper : IScoped
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _salt;
    private readonly string _postSalt;

    private readonly TimeSpan _authExpireTime = TimeSpan.FromHours(5);


    public AuthHelper(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"]!;
        _issuer = configuration["Jwt:Issuer"]!;
        _audience = configuration["Jwt:Audience"]!;
        _salt = configuration["Auth:Salt"]!;
        _postSalt = configuration["Auth:PostSalt"]!;
    }

    public string CreateToken(int userId)
    {
        var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };

        var expires = DateTime.Now.AddHours(6);
        var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);

        var key = new SymmetricSecurityKey(secretKeyBytes);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor =
            new JwtSecurityToken(_issuer, _audience, claims, expires: expires, signingCredentials: credentials);
        var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        if (string.IsNullOrWhiteSpace(jwt))
            throw new Exception("Created JWT was null or whitespace");
        return jwt;
    }

    public int GetUserIdFromToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return 0;

        var tokenHandler = new JwtSecurityTokenHandler();
        var validateParams = new TokenValidationParameters();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        validateParams.IssuerSigningKey = key;
        validateParams.ValidIssuer = _issuer;
        validateParams.ValidAudience = _audience;
        var claimsPrinciple = tokenHandler.ValidateToken(token, validateParams, out var secToken);
        var claim = claimsPrinciple.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return !string.IsNullOrWhiteSpace(claim?.Value) ? int.Parse(claim.Value) : 0;
    }

    public async Task<string> GetAuthStringHash(string authString)
    {
        var md5 = MD5.Create();
        var sourceBytes = Encoding.UTF8.GetBytes(authString + _salt);
        using var stream = new MemoryStream(sourceBytes);
        var bytes = await md5.ComputeHashAsync(stream);

        return Convert.ToBase64String(bytes) + _postSalt;
    }

    private CacheKey GetAuthTokenCacheKey(int userId)
    {
        return new CacheKey($"MoeUC:Auth:Token:{userId}", _authExpireTime);
    }
}