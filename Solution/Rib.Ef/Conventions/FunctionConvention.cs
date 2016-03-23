namespace Rib.Ef.Conventions
{
    using System;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class FunctionConvention : IStoreModelConvention<EdmModel>
    {
        private readonly Type _functionsDefineType;

        public FunctionConvention(Type functionsDefineType)
        {
            if (functionsDefineType == null) throw new ArgumentNullException(nameof(functionsDefineType));
            _functionsDefineType = functionsDefineType;
        }

        /// <summary>
        /// Applies this convention to an item in the model.
        /// </summary>
        /// <param name="item">The item to apply the convention to.</param><param name="model">The model.</param>
        public void Apply(EdmModel item, DbModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}