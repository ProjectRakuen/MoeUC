using MoeUC.Service.Auth;

namespace MoeUC.Test.Auth;

public class AuthTest : IClassFixture<AuthHelper>
{
    private readonly AuthHelper _authHelper;

    public AuthTest(AuthHelper authHelper)
    {
        _authHelper = authHelper;
    }

    [Fact]
    public void TestJwt()
    {
        var token = _authHelper.CreateToken(1);
        Assert.NotNull(token);
        var userId = _authHelper.GetUserIdFromToken(token);
        Assert.Equal(1, userId);
    }

    [Fact]
    public async Task TestAuthHash()
    {
        var hash = await _authHelper.GetAuthStringHash("123456");
        Assert.NotEmpty(hash);
    }
}