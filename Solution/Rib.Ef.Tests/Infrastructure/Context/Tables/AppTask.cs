namespace Rib.Ef.Tests.Infrastructure.Context.Tables
{
    public class AppTask
    {
        public int Id { get; set; }

        public AppUser User { get; set; }

        public Project Project { get; set; }
    }
}