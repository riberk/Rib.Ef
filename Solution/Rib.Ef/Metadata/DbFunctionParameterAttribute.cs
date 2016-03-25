namespace Rib.Ef.Metadata
{
    using System;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class DbFunctionParameterAttribute : Attribute
    {
        public DbFunctionParameterAttribute([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        public string Name { get; }
    }
}