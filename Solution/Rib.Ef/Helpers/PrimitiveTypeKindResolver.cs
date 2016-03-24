namespace Rib.Ef.Helpers
{
    using System;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Spatial;
    using JetBrains.Annotations;

    public static class PrimitiveTypeKindResolver
    {
        /// <summary>
        ///     Returns the <see cref="PrimitiveTypeKind" /> corresponding to the given CLR type
        /// </summary>
        /// <param name="clrType"> The CLR type for which the PrimitiveTypeKind value should be resolved </param>
        /// <param name="resolvedPrimitiveTypeKind"> The PrimitiveTypeKind value to which the CLR type resolves, if any. </param>
        /// <returns> True if the CLR type represents a primitive (EDM) type; otherwise false. </returns>
        public static bool TryGetPrimitiveTypeKind([NotNull] Type clrType, out PrimitiveTypeKind resolvedPrimitiveTypeKind)
        {
            if (clrType == null) throw new ArgumentNullException(nameof(clrType));
            PrimitiveTypeKind? primitiveTypeKind = null;
            if (!clrType.IsEnum) // Enums return the TypeCode of their underlying type
            {
                // As an optimization, short-circuit when the provided type has a known type code.
                switch (Type.GetTypeCode(clrType))
                {
                    // PrimitiveTypeKind.Binary = byte[] = TypeCode.Object
                    case TypeCode.Boolean:
                        primitiveTypeKind = PrimitiveTypeKind.Boolean;
                        break;
                    case TypeCode.Byte:
                        primitiveTypeKind = PrimitiveTypeKind.Byte;
                        break;
                    case TypeCode.DateTime:
                        primitiveTypeKind = PrimitiveTypeKind.DateTime;
                        break;
                    // PrimitiveTypeKind.DateTimeOffset = System.DateTimeOffset = TypeCode.Object
                    case TypeCode.Decimal:
                        primitiveTypeKind = PrimitiveTypeKind.Decimal;
                        break;
                    case TypeCode.Double:
                        primitiveTypeKind = PrimitiveTypeKind.Double;
                        break;
                    // PrimitiveTypeKind.Geography = System.Data.Entity.Spatial.DbGeometry (or subtype) = TypeCode.Object
                    // PrimitiveTypeKind.Geometry = System.Data.Entity.Spatial.DbGeometry (or subtype) = TypeCode.Object
                    // PrimitiveTypeKind.Guid = System.Guid = TypeCode.Object
                    case TypeCode.Int16:
                        primitiveTypeKind = PrimitiveTypeKind.Int16;
                        break;
                    case TypeCode.Int32:
                        primitiveTypeKind = PrimitiveTypeKind.Int32;
                        break;
                    case TypeCode.Int64:
                        primitiveTypeKind = PrimitiveTypeKind.Int64;
                        break;
                    case TypeCode.SByte:
                        primitiveTypeKind = PrimitiveTypeKind.SByte;
                        break;
                    case TypeCode.Single:
                        primitiveTypeKind = PrimitiveTypeKind.Single;
                        break;
                    case TypeCode.String:
                        primitiveTypeKind = PrimitiveTypeKind.String;
                        break;
                    // PrimitiveTypeKind.Time = System.TimeSpan = TypeCode.Object
                    case TypeCode.Object:
                        {
                            if (typeof(byte[]) == clrType)
                            {
                                primitiveTypeKind = PrimitiveTypeKind.Binary;
                            }
                            else if (typeof(DateTimeOffset) == clrType)
                            {
                                primitiveTypeKind = PrimitiveTypeKind.DateTimeOffset;
                            }
                            // DbGeography/Geometry are abstract so subtypes must be allowed
                            else if (typeof(DbGeography).IsAssignableFrom(clrType))
                            {
                                primitiveTypeKind = PrimitiveTypeKind.Geography;
                            }
                            else if (typeof(DbGeometry).IsAssignableFrom(clrType))
                            {
                                primitiveTypeKind = PrimitiveTypeKind.Geometry;
                            }
                            else if (typeof(Guid) == clrType)
                            {
                                primitiveTypeKind = PrimitiveTypeKind.Guid;
                            }
                            else if (typeof(TimeSpan) == clrType)
                            {
                                primitiveTypeKind = PrimitiveTypeKind.Time;
                            }
                            break;
                        }
                }
            }

            if (primitiveTypeKind.HasValue)
            {
                resolvedPrimitiveTypeKind = primitiveTypeKind.Value;
                return true;
            }
            resolvedPrimitiveTypeKind = default(PrimitiveTypeKind);
            return false;
        }
    }
}