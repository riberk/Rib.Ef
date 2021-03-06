﻿namespace Rib.Ef.Conventions
{
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Rib.Ef.Metadata;

    public class DateTimePrecisionConvention : PrimitivePropertyAttributeConfigurationConvention<DateTimePrecisionAttribute>
    {
        /// <summary>
        /// Applies this convention to a property that has an attribute of type TAttribute applied.
        /// </summary>
        /// <param name="configuration">The configuration for the property that has the attribute.</param><param name="attribute">The attribute.</param>
        public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DateTimePrecisionAttribute attribute)
        {
            configuration.HasPrecision(attribute.Precision);
        }
    }
}