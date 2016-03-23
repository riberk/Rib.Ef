namespace Rib.Ef.Metadata
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimePrecisionAttribute : Attribute
    {
        public DateTimePrecisionAttribute(byte value)
        {
            Precision = value;
        }

        public byte Precision { get; }
    }
}