namespace Rib.Ef.Interceptors
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using JetBrains.Annotations;

    public interface IInterceptorsInvoker
    {
        void InvokeBeforeSave([NotNull, ItemNotNull] IEnumerable<IInterceptor> interceptors,
                              [NotNull, ItemNotNull] IEnumerable<DbEntityEntry> entries);
    }
}