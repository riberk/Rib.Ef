namespace Rib.Ef.Tests.Infrastructure.Context
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Rib.Ef.Interceptors;
    using Rib.Ef.Tests.Infrastructure.Context.Tables;

    public class RibEfTestContext : DbContext
    {
        private readonly IReadOnlyCollection<IInterceptor> _interceptors = new List<IInterceptor>();
        private readonly IInterceptorsInvoker _interceptorsInvoker;

        public RibEfTestContext() : base("RibEfConnection")
        {
        }

        public RibEfTestContext(IReadOnlyCollection<IInterceptor> interceptors, IInterceptorsInvoker interceptorsInvoker): this()
        {
            if (interceptorsInvoker == null) throw new ArgumentNullException(nameof(interceptorsInvoker));
            _interceptors = interceptors ?? new List<IInterceptor>();
            _interceptorsInvoker = interceptorsInvoker;
        }

        public IDbSet<AppTask> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public override int SaveChanges()
        {
            _interceptorsInvoker?.InvokeBeforeSave(_interceptors, ChangeTracker.Entries());
            return base.SaveChanges();
        }
    }
}