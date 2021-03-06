﻿namespace Rib.Ef.Conventions
{
    using System.ComponentModel;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Text;

    public class DescriptionAnnotationConvention : AttributeToColumnAnnotationConvention<DescriptionAttribute, string>
    {
        internal const string AnnotationName = "Description";

        public DescriptionAnnotationConvention() : base(AnnotationName, (propertyInfo, attributes) =>
        {
            //TODO multi language
            var desc = attributes.Single().Description;
            var utf8 = Encoding.UTF8;
            var windows1251 = Encoding.GetEncoding(1251);

            var utf8Byte = utf8.GetBytes(desc);
            var windows1251Byte = Encoding.Convert(utf8, windows1251, utf8Byte);
            desc = windows1251.GetString(windows1251Byte);

            return desc;
        })
        {
        }
    }
}