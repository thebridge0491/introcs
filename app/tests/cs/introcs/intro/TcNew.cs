namespace Introcs.Intro.Tests {

using System;
using System.Linq;
using NUnit.Framework;

using Introcs.Intro;

[TestFixture]
public class TcNew : Base.ClsBase {
    private float epsilon = 0.001f; //1.20e-7f;
    
    public bool InEpsilon(double a, double b, double tolerance = 0.001)
    {
		double delta = Math.Abs(tolerance);
		//return (a - delta) <= b && (a + delta) >= b;
		return !((a + delta) < b) && !((b + delta) < a);
	}
	
    [TestFixtureSetUp]
    public override void SetUpClass()
    {
        base.SetUpClass();
        Console.Error.WriteLine("SetUpClass({0})", GetType());
    }
    
    [TestFixtureTearDown]
    public override void TearDownClass()
    {
        Console.Error.WriteLine("TearDownClass({0})", GetType());
        base.TearDownClass();
    }
    
    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        Console.Error.WriteLine("SetUp");
    }
    
    [TearDown]
    public override void TearDown()
    {
        Console.Error.WriteLine("TearDown");
        base.TearDown();
    }
    
    public override void Dispose()
    {
        Console.Error.WriteLine("Derived Dispose({0})", GetType());
        base.Dispose();
    }
    
	[Test] [Category("Tag1")]
	public void MethodTest()
	{
		Assert.That(2*2, Is.EqualTo(4), "(Constraint) Multiply");
	    Assert.AreEqual(2 * 2, 4, "Multiplication");
	}
	
	[Test] [Category("Tag1")]
	public void FloatTest()
	{
	    /*Assert.That(4.0f, Is.EqualTo(4.0f).Within(4.0f * epsilon), 
	            "Floats constraint-based");*/
	    //Assert.AreEqual(4.0f, 4.0f, 4.0f * epsilon, "Floats");
	    Assert.True(InEpsilon(4.0f, 4.0f, epsilon * 4.0f), "Floats");
	}
	
	[Test] [Category("Tag1")]
	public void StringTest()
	{
	    string str1 = "Hello", str2 = "hello";
	    /*Assert.True(0 == String.Compare(str1, str2, 
            StringComparison.OrdinalIgnoreCase));*/
	    StringAssert.AreEqualIgnoringCase(str1, str2, "Strings");
	}
	
	[Test] [Category("Tag2")]
	public void BadTest()
	{
	    Assert.AreEqual(4, 5, "Equals");
	}
	
	[Test] [Category("Tag2")]
	public void FailedTest()
	{
		Assert.Fail();
	}
	
	[Test] [Category("Tag2")] [Ignore("ignored test")]
	public void IgnoredTest()
	{
		throw new Exception();
	}
	
	[Test] [Category("Tag2")] [Platform("Win98, WinME")]
	public void SkippedWinMETest()
    {
        // ... 
    }
    
	[Test] [Category("Tag1")] //[Test, Timeout(100)]
	public void PassedTest()
	{
		//Assert.Pass();
	}
	
	[Test] [ExpectedException(typeof(InvalidOperationException))]
	public void ExpectAnException()
	{
		throw new InvalidOperationException();
	}
}

}
