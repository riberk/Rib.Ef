﻿namespace Rib.Ef.Tests.Context
{
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.SqlServer;
    using System.Diagnostics;
    using System.IO;
    using Rib.Ef.Conventions;
    using Rib.Ef.Tests.Context.Tables;

    public class RibEfContext : DbContext
    {
        public RibEfContext() : base("Data Source=CurrentServer;Initial Catalog=RibEf;Integrated Security=SSPI;")
        {
            Database.Log = s => Debug.WriteLine(s);
        }

        public DbSet<Project> Projects { get; set; }
        public IDbSet<ApplicationTask> Tasks { get; set; }

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
    }
}