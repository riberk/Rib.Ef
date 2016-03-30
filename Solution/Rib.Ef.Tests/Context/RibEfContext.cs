namespace Rib.Ef.Tests.Context
{
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.SqlServer;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Rib.Ef.Conventions;
    using Rib.Ef.Interceptors;
    using Rib.Ef.Tests.Context.Tables;

    public class RibEfContext : DbContext
    {
        public RibEfContext() : base("Data Source=CurrentServer;Initial Catalog=RibEf;Integrated Security=SSPI;")
        {
            Database.Log = s => Debug.WriteLine(s);
        }

        public DbSet<Project> Projects { get; set; }
        public IDbSet<ApplicationTask> Tasks { get; set; }
        public IDbSet<Comment> Comments { get; set; }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        ///             before the model has been locked down and used to initialize the context.  The default
        ///             implementation of this method does nothing, but it can be overridden in a derived class
        ///             such that the model can be further configured before it is locked down.
        /// </summary>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        ///             is created.  The model for that context is then cached and is for all further instances of
        ///             the context in the app domain.  This caching can be disabled by setting the ModelCaching
        ///             property on the given ModelBuidler, but note that this can seriously degrade performance.
        ///             More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        ///             classes directly.
        /// </remarks>
        /// <param name="modelBuilder">The builder that defines the model for the context being created. </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DateTimePrecisionConvention());
            modelBuilder.Conventions.Add(new DecimalPrecisionConvention());
            modelBuilder.Conventions.Add(new DefaultValueAnnotationConvention());
            modelBuilder.Conventions.Add(new DescriptionAnnotationConvention());
            modelBuilder.Conventions.Add(new TableDescriptionAnnotationConvention());
            modelBuilder.Conventions.Add(new FunctionConvention(typeof(RibEfFunctions)));
            modelBuilder.Conventions.Add(new SqlDefaultValueAnnotationConvention());
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the underlying database. This can include
        ///             state entries for entities and/or relationships. Relationship state entries are created for 
        ///             many-to-many relationships and relationships where there is no foreign key property
        ///             included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">An error occurred sending updates to the database.</exception><exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">A database command did not affect the expected number of rows. This usually indicates an optimistic 
        ///             concurrency violation; that is, a row has been changed in the database since it was queried.
        ///             </exception><exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">The save was aborted because validation of entity property values failed.
        ///             </exception><exception cref="T:System.NotSupportedException">An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///             on the same context instance.</exception><exception cref="T:System.ObjectDisposedException">The context or connection have been disposed.</exception><exception cref="T:System.InvalidOperationException">Some error occurred attempting to process entities in the context either before or after sending commands
        ///             to the database.
        ///             </exception>
        public override int SaveChanges()
        {
            InterceptorsEngine.InvokeBeforeSave(this);
            return base.SaveChanges();
        }
    }
}