namespace Rib.Ef.Interceptors
{
    using System.Data.Entity.Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Ef.Tests.Infrastructure.Context;
    using Rib.Ef.Tests.Infrastructure.Context.Tables;

    [TestClass]
    public class InterceptorsInvokerTests
    {
        [TestMethod]
        public void InvokeTest()
        {
            var invoker = new InterceptorsInvoker();
            var mf = new MockRepository(MockBehavior.Strict);


            var i1 = mf.Create<IInterceptor>();
            var i2 = mf.Create<IInterceptor>();
            var i3 = mf.Create<IInterceptor>();

            var ctx = new RibEfTestContext();
            var appTask1 = new AppTask {Id = 1};
            var appTask2 = new AppTask {Id = 2};

            ctx.Set<AppTask>().Attach(appTask1);
            ctx.Set<AppTask>().Attach(appTask2);

            var e1 = ctx.Entry(appTask1);
            var e2 = ctx.Entry(appTask2);

            var callOrder = 0;

            i3.Setup(x => x.IsApplicable(e1)).Returns(false).Verifiable();
            i3.Setup(x => x.IsApplicable(e2)).Returns(false).Verifiable();

            i1.Setup(x => x.IsApplicable(e1)).Returns(true).Verifiable();
            i2.Setup(x => x.IsApplicable(e1)).Returns(true).Verifiable();

            i1.Setup(x => x.Order(e1)).Returns(1).Verifiable();
            i2.Setup(x => x.Order(e1)).Returns(2).Verifiable();

            i1.Setup(x => x.BeforeSave(e1)).Callback(() => Assert.AreEqual(0, callOrder++)).Verifiable();
            i2.Setup(x => x.BeforeSave(e1)).Callback(() => Assert.AreEqual(1, callOrder++)).Verifiable();

            i1.Setup(x => x.IsApplicable(e2)).Returns(true).Verifiable();
            i2.Setup(x => x.IsApplicable(e2)).Returns(true).Verifiable();

            i1.Setup(x => x.Order(e2)).Returns(2).Verifiable();
            i2.Setup(x => x.Order(e2)).Returns(1).Verifiable();

            i1.Setup(x => x.BeforeSave(e2)).Callback(() => Assert.AreEqual(3, callOrder++)).Verifiable();
            i2.Setup(x => x.BeforeSave(e2)).Callback(() => Assert.AreEqual(2, callOrder++)).Verifiable();

            invoker.InvokeBeforeSave(new[] {i1.Object, i2.Object, i3.Object}, new DbEntityEntry[] {e1, e2});

            Assert.AreEqual(4, callOrder);
            mf.VerifyAll();
        }
    }
}