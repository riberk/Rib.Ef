namespace Rib.Ef.Conventions
{
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using Rib.Ef.Metadata;

    public class SqlDefaultValueAnnotationConvention : AttributeToColumnAnnotationConvention<SqlDefaultValueAttribute, string>
    {
        internal const string AnnotationName = "SqlDefaultValue";

        public SqlDefaultValueAnnotationConvention() : base(AnnotationName, (propertyInfo, attributes) => attributes.Single().SqlDefaultValue)
        {
        }
    }
}