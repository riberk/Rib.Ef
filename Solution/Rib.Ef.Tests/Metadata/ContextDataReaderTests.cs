namespace Rib.Ef.Metadata
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Ef.Tests.Infrastructure.Context;
    using Rib.Ef.Tests.Infrastructure.Context.Tables;

    [TestClass]
    public class ContextDataReaderTests
    {
        [TestMethod]
        public void GetEntityTypesTest()
        {
            var types = new HashSet<Type>();
            var acceptedTypes = new[]
            {
                typeof (AppTask),
                typeof (AppUser),
                typeof (Project),
                typeof (Tag)
            };
            using (var ribEfTestContext = new RibEfTestContext())
            {
                var gettedTypes = ribEfTestContext.GetEntityTypes(typeof (RibEfTestContext).Assembly);
                foreach (var gettedType in gettedTypes)
                {
                    Assert.IsTrue(types.Add(gettedType));
                }
            }
            Assert.AreEqual(acceptedTypes.Length, types.Count);
            foreach (var acceptedType in acceptedTypes)
            {
                Assert.IsTrue(types.Contains(acceptedType));
            }
        }
    }
}