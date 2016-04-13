namespace Rib.Ef.Tests.Context
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using JetBrains.Annotations;
    using Rib.Ef.Interceptors;
    using Rib.Ef.Tests.Context.Interfaces;

    public class SetUpdateDateInterceptor : Interceptor<IHasUpdateDate>
    {
        [NotNull] public static HashSet<EntityState> EntityStates = new HashSet<EntityState>
        {
            EntityState.Added,
            EntityState.Modified
        };

        protected override HashSet<EntityState> States => EntityStates;

        protected override void BeforeSave(IHasUpdateDate entity)
        {
            entity.Modified = DateTime.UtcNow;
        }
    }
}