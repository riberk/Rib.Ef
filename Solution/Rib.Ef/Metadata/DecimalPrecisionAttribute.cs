namespace Rib.Ef.Metadata
{
    using System;


    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalPrecisionAttribute : Attribute
    {
        public DecimalPrecisionAttribute(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;
        }

        public byte Precision { get; }

        public byte Scale { get; }
    }
}