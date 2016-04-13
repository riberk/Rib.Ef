namespace Rib.Ef.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Ef.Tests.Infrastructure.Context;
    using Rib.Ef.Tests.Infrastructure.Context.Tables;

    [TestClass]
    public class InterceptorTests
    {
        [TestMethod]
        public void OrderTest()
        {
            var ic = new TestInterceptor();
            Assert.AreEqual(int.MaxValue, ic.Order(null));
        }

        [TestMethod]
        public void IsApplicableTest()
        {
            var ic = new TestInterceptor();
            var ctx = new RibEfTestContext();

            var t1 = new AppTask {Id = 1};
            var t2 = new AppTask {Id = 2 };

            var t3 = new AppTask();

            var p1 = new Project {Id = 1};
            var p2 = new Project();

            ctx.Tasks.Attach(t1);
            ctx.Tasks.Attach(t2);
            ctx.Tasks.Add(t3);
            ctx.Set<Project>().Attach(p1);
            ctx.Set<Project>().Add(p2);

            var t1e = ctx.Entry(t1);
            var t2e = ctx.Entry(t2);
            var t3e = ctx.Entry(t3);
            var p1e = ctx.Entry(p1);
            var p2e = ctx.Entry(p2);

            Assert.IsFalse(ic.IsApplicable(t1e));
            Assert.IsFalse(ic.IsApplicable(t2e));
            Assert.IsTrue(ic.IsApplicable(t3e));
            Assert.IsFalse(ic.IsApplicable(p1e));
            Assert.IsFalse(ic.IsApplicable(p2e));
        }

        [TestMethod]
        public void BeforeSaveTest()
        {
            var ctx = new RibEfTestContext();

            var t1 = new AppTask { Id = 1 };
            ctx.Tasks.Add(t1);
            var t1e = ctx.Entry(t1);
            var invoked = false;
            var ic = new TestInterceptor(t =>
            {
                Assert.AreEqual(t1, t);
                invoked = true;
            });
            ic.BeforeSave(t1e);
            Assert.IsTrue(invoked);
        }

        public class TestInterceptor : Interceptor<AppTask>
        {
            private readonly Action<AppTask> _callback;

            public TestInterceptor(Action<AppTask> callback = null)
            {
                _callback = callback;
            }

            protected override HashSet<EntityState> States { get; } = new HashSet<EntityState>
            {
                EntityState.Added
            };

            protected override void BeforeSave(AppTask entity)
            {
                _callback(entity);
            }
        }
    }
}