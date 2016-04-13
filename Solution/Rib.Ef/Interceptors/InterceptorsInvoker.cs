namespace Rib.Ef.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using JetBrains.Annotations;

    internal class InterceptorsInvoker : IInterceptorsInvoker
    {
        public void InvokeBeforeSave(IEnumerable<IInterceptor> interceptors, IEnumerable<DbEntityEntry> entries)
        {
            Invoke(interceptors, entries, (interceptor, entry) => interceptor.BeforeSave(entry));
        }

        private static void Invoke([NotNull, ItemNotNull] IEnumerable<IInterceptor> interceptors,
                                   [NotNull, ItemNotNull] IEnumerable<DbEntityEntry> entries,
                                   [NotNull] Action<IInterceptor, DbEntityEntry> invoker)
        {
            foreach (var dbEntityEntry in entries)
            {
                foreach (var interceptor in interceptors.Where(i => i.IsApplicable(dbEntityEntry)).OrderBy(i => i.Order(dbEntityEntry)))
                {
                    invoker(interceptor, dbEntityEntry);
                }
            }
        }
    }
}