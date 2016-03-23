namespace Rib.Ef.Conventions
{
    using System.ComponentModel;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class DefaultValueAnnotationConvention : AttributeToColumnAnnotationConvention<DefaultValueAttribute, object>
    {
        internal const string AnnotationName = "DefaultValue";

        public DefaultValueAnnotationConvention() : base(AnnotationName, (propertyInfo, attributes) => attributes.Single().Value)
        {
        }
    }
}