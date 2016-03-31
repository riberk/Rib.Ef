namespace Rib.Ef.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;

    public static class ContextDataReader
    {
        [NotNull, ItemNotNull]
        public static IEnumerable<Type> GetEntityTypes([NotNull] this DbContext ctx, [NotNull] params Assembly[] assemblies)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            var objectContext = ((IObjectContextAdapter)ctx).ObjectContext;
            var mdw = objectContext.MetadataWorkspace;
            return mdw.GetItems<EntityType>(DataSpace.CSpace).Select(x =>
            {
                var types = assemblies.SelectMany(a => a.GetTypes()).Where(t => t.Name == x.Name).ToArray();
                if (types.Length == 0)
                {
                    throw new ArgumentException($"Could not be found type {x.Name} in assemblies");
                }
                if (types.Length > 1)
                {
                    throw new ArgumentException($"Found many of one type with name {x.Name} in assemblies");
                }
                return types[0];
            });
        }
    }
}