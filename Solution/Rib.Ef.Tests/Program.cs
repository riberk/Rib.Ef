namespace Rib.Ef.Tests
{
    using System.Diagnostics;
    using Rib.Ef.Interceptors;
    using Rib.Ef.Tests.Context;
    using Rib.Ef.Tests.Context.Tables;

    public class Program
    {
        public static void Main()
        {
            InterceptorsEngine.Interceptors.Add(new SetUpdateDateInterceptor());

            using (var ctx = new RibEfContext())
            {
                ctx.Set<User>().Add(new User{Login = "123"});
                ctx.SaveChanges();
            }
        }
    }
}