namespace Rib.Ef.Tests.Infrastructure.Context
{
    using System.Data.Entity;
    using Rib.Ef.Tests.Infrastructure.Context.Tables;

    public class RibEfTestContext : DbContext
    {
        public RibEfTestContext() : base("RibEfConnection")
        {
        }

        public IDbSet<AppTask> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}