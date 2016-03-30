namespace Rib.Ef.Interceptors
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public static class InterceptorsEngine
    {
        [NotNull]
        public static IInterceptorsInvoker Invoker { get; set; } = new InterceptorsInvoker();

        [NotNull]
        public static ICollection<IInterceptor> Interceptors { get; } = new HashSet<IInterceptor>();

        public static void InvokeBeforeSave([NotNull] DbContext ctx)
        {
            Invoker.InvokeBeforeSave(Interceptors, ctx.ChangeTracker.Entries());
        }
    }
}