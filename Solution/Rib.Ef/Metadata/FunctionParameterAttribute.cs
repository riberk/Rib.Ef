namespace Rib.Ef.Metadata
{
    using System;
    using System.Data.Entity.Core.Metadata.Edm;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class FunctionParameterAttribute : Attribute
    {
        public string Name { get; }
        public PrimitiveTypeKind Kind { get; }
        public ParameterMode ParameterMode { get; }
        public int Order { get; }

        public FunctionParameterAttribute(string name, PrimitiveTypeKind kind, ParameterMode parameterMode, int order = 0)
        {
            Name = name;
            Kind = kind;
            ParameterMode = parameterMode;
            Order = order;
        }
    }
}