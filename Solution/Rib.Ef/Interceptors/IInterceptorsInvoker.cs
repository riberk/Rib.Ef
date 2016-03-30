namespace Rib.Ef.Interceptors
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IInterceptorsInvoker
    {
        void InvokeBeforeSave([NotNull, ItemNotNull] IEnumerable<IInterceptor> interceptors,
            [NotNull, ItemNotNull] IEnumerable<DbEntityEntry> entries);
    }
}