namespace Rib.Ef.Tests.Migrations
{
    using System.Data.Entity.Migrations;
    using Rib.Ef.Tests.Context;

    internal sealed class Configuration : DbMigrationsConfiguration<RibEfContext>
    {
        public Configuration()
        {
            SetSqlGenerator("System.Data.SqlClient", new RibEfSqlServerMigrationSqlGenerator());
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RibEfContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}