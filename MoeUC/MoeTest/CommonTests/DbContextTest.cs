using Microsoft.EntityFrameworkCore;
using MoeUC.Core.Domain;
using MoeUC.Core.Infrastructure.Data;

namespace MoeUC.Test.CommonTests
{
    public class DbContextTest : IClassFixture<MoeDbContext>
    {
        private readonly MoeDbContext _dbContext;

        public DbContextTest(MoeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Fact]
        public void GenerateCreateSql()
        {
            var generateSql = _dbContext.GenerateCreateScript();
            Assert.True(!string.IsNullOrWhiteSpace(generateSql));
        }

        [Fact]
        public async Task TestDbContext()
        {
            var entity = _dbContext.Set<MoeUser>();
            await entity.FirstOrDefaultAsync();
        }
    }
}