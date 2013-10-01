namespace Base {

using System;
using NUnit.Framework;

public abstract class ClsBase : IDisposable {
    protected ClsBase()
    {
        ;
    }
    [TestFixtureSetUp]
    public virtual void SetUpClass()
    {
        Console.Error.WriteLine("Base SetUpClass({0})", GetType().BaseType);
    }
    [TestFixtureTearDown]
    public virtual void TearDownClass()
    {
        Console.Error.WriteLine("Base TearDownClass({0})", GetType().BaseType);
    }
    [SetUp]
    public virtual void SetUp()
    {
        Console.Error.WriteLine("Base SetUp");
    }
    [TearDown]
    public virtual void TearDown()
    {
        Console.Error.WriteLine("Base TearDown");
    }
    public virtual void Dispose()
    {
        Console.Error.WriteLine("Base Dispose({0})", GetType().BaseType);
    }
}

}
