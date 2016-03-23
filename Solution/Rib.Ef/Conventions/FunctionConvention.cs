namespace Rib.Ef.Conventions
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Ef.Helpers;

    public class FunctionConvention : IStoreModelConvention<EdmModel>
    {
        [NotNull] private readonly Type _functionsDefineType;

        public FunctionConvention([NotNull] Type functionsDefineType)
        {
            if (functionsDefineType == null) throw new ArgumentNullException(nameof(functionsDefineType));
            _functionsDefineType = functionsDefineType;
        }

        /// <summary>
        ///     Applies this convention to an item in the model.
        /// </summary>
        /// <param name="item">The item to apply the convention to.</param>
        /// <param name="model">The model.</param>
        public void Apply([NotNull] EdmModel item, [NotNull] DbModel model)
        {
            var functions = _functionsDefineType
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Select(x => new
                    {
                        Attr = x.GetCustomAttribute<DbFunctionAttribute>(),
                        Method = x
                    })
                    .Where(x => x.Attr != null)
                    .Select(x => new
                    {
                        x.Attr,
                        x.Method,
                        Parameters = x.Method.GetParameters()
                                      .Select(Parameter.Create)
                                      .Select(p => p.ToFunctionParameter(model, ParameterMode.In)),
                        ReturnValue = Parameter.Create(x.Method.ReturnParameter).ToFunctionParameter(model, ParameterMode.ReturnValue)
                    })
                    .Select(f => item.CreateFunction(f.Attr.FunctionName, f.Parameters.ToList(), new[] {f.ReturnValue}));
            foreach (var function in functions)
            {
                item.AddItem(function);
            }
        }

        private struct Parameter
        {
            private Parameter(PrimitiveTypeKind kind, [NotNull] string name)
            {
                Kind = kind;
                Name = name;
            }

            public static Parameter Create([NotNull] ParameterInfo info)
            {
                if (info == null) throw new ArgumentNullException(nameof(info));
                PrimitiveTypeKind kind;
                if (!PrimitiveTypeKindResolver.TryGetPrimitiveTypeKind(info.ParameterType, out kind))
                {
                    throw new InvalidOperationException($"{info.ParameterType} is undefined PrimitiveTypeKind");
                }
                return new Parameter(kind, info.Name);
            }

            public FunctionParameter ToFunctionParameter([NotNull] DbModel model, ParameterMode mode)
            {
                return FunctionParameter.Create(Name, model.GetStorePrimitiveType(Kind), mode);
            }

            private PrimitiveTypeKind Kind { get; }

            [NotNull]
            private string Name { get; }
        }
    }
}