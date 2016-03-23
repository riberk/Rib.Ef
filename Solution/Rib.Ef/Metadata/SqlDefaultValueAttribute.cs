namespace Rib.Ef.Metadata
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class SqlDefaultValueAttribute : Attribute
    {
        public SqlDefaultValueAttribute(string sqlDefaultValue)
        {
            SqlDefaultValue = sqlDefaultValue;
        }

        public string SqlDefaultValue { get; }
    }
}