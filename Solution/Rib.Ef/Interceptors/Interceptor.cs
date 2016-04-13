namespace Rib.Ef.Interceptors
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using JetBrains.Annotations;

    public abstract class Interceptor<T> : IInterceptor
            where T : class
    {
        [NotNull]
        protected abstract HashSet<EntityState> States { get; }


        public int Order(DbEntityEntry entry)
        {
            return int.MaxValue;
        }

        public bool IsApplicable(DbEntityEntry entry)
        {
            var entity = entry.Entity as T;
            return States.Contains(entry.State) && entity != null;
        }

        public void BeforeSave(DbEntityEntry entry)
        {
            BeforeSave((T) entry.Entity);
        }

        protected abstract void BeforeSave(T entity);
    }
}