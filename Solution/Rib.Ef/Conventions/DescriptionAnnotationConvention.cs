namespace Rib.Ef.Conventions
{
    using System.ComponentModel;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class DescriptionAnnotationConvention : AttributeToColumnAnnotationConvention<DescriptionAttribute, string>
    {
        internal const string AnnotationName = "Description";

        public DescriptionAnnotationConvention() : base(AnnotationName, (propertyInfo, attributes) => attributes.Single().Description)
        {
        }
    }
}