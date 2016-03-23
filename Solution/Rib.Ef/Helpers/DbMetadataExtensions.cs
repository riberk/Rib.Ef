namespace Rib.Ef.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using JetBrains.Annotations;

    public static class DbMetadataExtensions
    {
        public static EdmFunction CreateFunction([NotNull] this EdmModel item,
                                                       string name,
                                                       IList<FunctionParameter> parameters,
                                                       IList<FunctionParameter> returnValues,
                                                       string body = null)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var payload = new EdmFunctionPayload
            {
                StoreFunctionName = name,
                Parameters = parameters,
                ReturnParameters = returnValues,
                Schema = item.GetDefaultSchema(),
                
            };

            var function = EdmFunction.Create(name, item.GetDefaultNamespace(), item.DataSpace, payload, null);

            return function;
        }


        /// <summary>
        ///     Translate a conceptual primitive type to an adequate store specific primitive type according to the
        ///     provider configuration of the model.
        /// </summary>
        /// <param name="model">A DbModel instance with provider information</param>
        /// <param name="typeKind">A PrimitiveTypeKind instance representing the conceptual primitive type to be translated</param>
        /// <returns>An EdmType instance representing the store primitive type</returns>
        public static EdmType GetStorePrimitiveType([NotNull] this DbModel model, PrimitiveTypeKind typeKind)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return model
                    .ProviderManifest
                    .GetStoreType(
                                  TypeUsage.CreateDefaultTypeUsage(
                                                                   PrimitiveType.GetEdmPrimitiveType(typeKind))).EdmType;
        }

        /// <summary>
        ///     Obtain the namespace name from existing model defined types.
        /// </summary>
        /// <param name="layerModel">An EdmModel instance representing conceptual or store model.</param>
        /// <returns>A string contining the name of the namespace.</returns>
        /// <remarks>
        ///     Only one namespace is allowed. Throws if there are multiple namespaces or if there aren't any types defined in the
        ///     model.
        /// </remarks>
        public static string GetDefaultNamespace([NotNull] this EdmModel layerModel)
        {
            if (layerModel == null) throw new ArgumentNullException(nameof(layerModel));
            return layerModel
                    .GlobalItems
                    .OfType<EdmType>()
                    .Select(t => t.NamespaceName)
                    .Distinct()
                    .Single();
        }

        /// <summary>
        ///     Obtains the default schema from existing entity sets in the model.
        /// </summary>
        /// <param name="layerModel">An instance of EdmModel representing either the conceptual or the store model.</param>
        /// <returns>A string containing the name of the schema.</returns>
        /// <remarks>
        ///     Throws if more than one schema is used or if the model contains no entity sets.
        /// </remarks>
        public static string GetDefaultSchema(this EdmModel layerModel)
        {
            return layerModel
                    .Container
                    .EntitySets
                    .Select(s => s.Schema)
                    .Distinct()
                    .SingleOrDefault();
        }
    }
}