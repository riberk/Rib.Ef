namespace Rib.Ef.Interceptors
{
    using System.Data.Entity.Infrastructure;
    using JetBrains.Annotations;

    public interface IInterceptor
    {
        int Order([NotNull] DbEntityEntry entry);

        bool IsApplicable([NotNull] DbEntityEntry entry);

        void BeforeSave([NotNull] DbEntityEntry entry);
    }
}