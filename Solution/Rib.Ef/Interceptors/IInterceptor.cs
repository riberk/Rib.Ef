namespace Rib.Ef.Interceptors
{
    using System.Data.Entity.Infrastructure;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IInterceptor
    {
        bool IsApplicable([NotNull] DbEntityEntry entry);

        void BeforeSave([NotNull] DbEntityEntry entry);
    }
}