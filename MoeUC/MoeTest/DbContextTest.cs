using MoeUC.Core.Infrastructure.Data;

namespace MoeUC.Test
{
    public class DbContextTest : IClassFixture<MoeDbContext>
    {
        private readonly MoeDbContext _dbContext;

        public DbContextTest(MoeDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [Fact]
        public void GenerateCreateSql()
        {
            var generateSql = _dbContext.GenerateCreateScript();
            Assert.True(!string.IsNullOrWhiteSpace(generateSql));
        }
    }
}