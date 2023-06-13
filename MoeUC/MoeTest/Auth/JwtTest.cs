using MoeUC.Service.Auth;

namespace MoeUC.Test.Auth;

public class JwtTest : IClassFixture<JwtHelper>
{
    private readonly JwtHelper _jwtHelper;

    public JwtTest(JwtHelper jwtHelper)
    {
        _jwtHelper = jwtHelper;
    }

    [Fact]
    public void TestJwtHelper()
    {
        var token = _jwtHelper.Create(1);
        Assert.NotNull(token);
        var userId = _jwtHelper.GetUserIdFromToken(token);
        Assert.Equal(1, userId);
    }
}