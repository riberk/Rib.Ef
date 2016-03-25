namespace Rib.Ef.Tests.Context
{
    using System;
    using System.Data.Entity;
    using Rib.Ef.Metadata;

    public static class RibEfFunctions
    {
        [DbFunction("CodeFirstDatabaseSchema", "SumTwoInts")]
        [return: DbFunctionParameter("result")]
        public static int SumTwoInts(
            [DbFunctionParameter("left")]int left,
            [DbFunctionParameter("right")]int right)
        {
            throw new NotSupportedException();
        }
    }
}