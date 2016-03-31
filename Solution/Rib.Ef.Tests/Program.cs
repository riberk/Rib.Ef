namespace Rib.Ef.Tests
{
    using System.Linq;
    using Rib.Ef.Interceptors;
    using Rib.Ef.Metadata;
    using Rib.Ef.Tests.Context;
    using Rib.Ef.Tests.Context.Tables;

    public class Program
    {
        public static void Main()
        {
            using (var ctx = new RibEfContext())
            {
                var types = ctx.GetEntityTypes(typeof(Comment).Assembly).ToList();
                ctx.Set<User>().Add(new User { Login = "123" });
                ctx.SaveChanges();
            }
        }
    }
}