namespace Introcs.Practice.Tests {

using System;
using System.Linq;
using NUnit.Framework;

using Util = Introcs.Util.Library;
using Introcs.Practice;

[TestFixture]
public class TcClassic : Base.ClsBase {
    private float epsilon = 0.001f; //1.20e-7f;
    
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

	[Test]
	public void SquareTest()
	{
		var funcs = new Func<float, float>[] {Classic.SquareLp,
			Classic.SquareI};
		float[] param1 = {2.0f, 11.0f, 20.0f};
	    
	    foreach (var n in param1) {
			float ans = (float)Math.Pow(n, 2.0f);
			foreach (var f in funcs)
				Assert.AreEqual(ans, f(n), (ans * epsilon), "SquareTest");
		}
	}
	
	[Test] [Category("Tag1")]
	public void ExptTest()
	{
		var funcs = new Func<float, float, float>[] {Classic.ExptLp,
			Classic.ExptI, ClassicFs.exptI, ClassicFs.exptR,
			Classic.FastExptLp, Classic.FastExptI};
		float[] param1 = {2.0f, 11.0f, 20.0f}, param2 = {3.0f, 6.0f, 10.0f};
	    var prodParams =		// cartesian product (System.Linq)
	    	from b in param1
	    	from n in param2
	    	select new {b, n};
	    foreach (var f in funcs)
			foreach (var tup in prodParams) {
				float ans = (float)Math.Pow(tup.b, tup.n);
				Assert.AreEqual(ans, f(tup.b, tup.n), (ans * epsilon),
					"ExptTest");
			}
	}
    
	[Test]
	public void SumToTest()
	{
		var funcs = new Func<long, long, long>[] {Classic.SumToLp,
			Classic.SumToI};
		
		foreach (var f in funcs) {
			long ans1 = 15L, ans2 = 75L;
	    	Assert.AreEqual(ans1, f(5L, 0L), "SumToTest1");
	    	Assert.AreEqual(ans2, f(15L, 10L), "SumToTest2");
		}
	}
    
	[Test] [Category("Tag1")]
	public void FactTest()
	{
		var funcs = new Func<long, long>[] {Classic.FactLp, Classic.FactI,
			ClassicFs.factI, ClassicFs.factR};
		long n = 5L;
		
		foreach (var f in funcs)
	    	Assert.AreEqual(120L, f(n), "FactTest");
	}
    
	[Test]
	public void FibTest()
	{
		var funcs = new Func<int, int>[] {Classic.FibLp, Classic.FibI};
		
		foreach (var f in funcs)
	    	Assert.AreEqual(13, f(7), "FibTest");
	}
    
	[Test]
	public void QuotRemTest()
	{
		int[] param1 = {10, -10}, param2 = {3, -3};
	    var prodParams = 
	    	from n0 in param1
	    	from n1 in param2
	    	select new {n0, n1};
		
		foreach (var tup in prodParams) {
			int ansQ = tup.n0 / tup.n1, ansR = tup.n0 % tup.n1;
	    	Assert.AreEqual(ansQ, Classic.QuotM(tup.n0, tup.n1), "QuotTest");
	    	Assert.AreEqual(ansR, Classic.RemM(tup.n0, tup.n1), "RemTest");
		}
	}
    
	[Test]
	public void GcdLcmTest()
	{
		var funcsGcd = new Func<int[], int>[] {Classic.GcdLp, Classic.GcdI};
		var funcsLcm = new Func<int[], int>[] {Classic.LcmLp, Classic.LcmI};
		int[] arr1 = {24, 16}, arr2Gcd = {24, 16, 12}, arr2Lcm = {24, 16, 32};
		int ans1Gcd = 8, ans1Lcm = 48, ans2Gcd = 4, ans2Lcm = 96;
		
		for (int i = 0; funcsGcd.Length > i; ++i) {
	    	Assert.AreEqual(ans1Gcd, funcsGcd[i](arr1), "GcdTest1");
	    	Assert.AreEqual(ans1Lcm, funcsLcm[i](arr1), "LcmTest1");
	    	
	    	Assert.AreEqual(ans2Gcd, funcsGcd[i](arr2Gcd), "GcdTest2");
	    	Assert.AreEqual(ans2Lcm, funcsLcm[i](arr2Lcm), "LcmTest2");
		}
	}
    
	[Test]
	public void BaseExpandTest()
	{
		var funcs = new Func<int, int, int[]>[] {Classic.BaseExpandLp,
			Classic.BaseExpandI};
		int[] ans1 = {1, 0, 1, 1}, ans2 = {1, 1, 0, 1};
		int b1 = 2, b2 = 4, n1 = 11, n2 = 81;
		
		foreach (var f in funcs) {
	    	//Assert.AreEqual(ans1, f(b1, n1), "BaseExpandTest1");
	    	//Assert.AreEqual(ans2, f(b2, n2), "BaseExpandTest2");
	    	Assert.True(ans1.SequenceEqual(f(b1, n1)), "BaseExpandTest1");
	    	Assert.True(ans2.SequenceEqual(f(b2, n2)), "BaseExpandTest2");
		}
	}
    
	[Test]
	public void BaseTo10Test()
	{
		var funcs = new Func<int, int[], int>[] {Classic.BaseTo10Lp,
			Classic.BaseTo10I};
		int[] arr1 = {1, 0, 1, 1}, arr2 = {1, 1, 0, 1};
		int b1 = 2, b2 = 4, ans1 = 11, ans2 = 81;
		
		foreach (var f in funcs) {
	    	Assert.AreEqual(ans1, f(b1, arr1), "BaseTo10Test1");
	    	Assert.AreEqual(ans2, f(b2, arr2), "BaseTo10Test2");
		}
	}
    
	[Test]
	public void RangeTest()
	{
		var funcsRgStep = new Func<int, int, int, int[]>[] {
			Classic.RangeStepLp, Classic.RangeStepI};
		var funcsRg = new Func<int, int, int[]>[] {Classic.RangeLp, 
			Classic.RangeI};
		
		int start1 = 0, stop1 = 5;
		int[] ans1 = {0, 1, 2, 3, 4};
		foreach (var f in funcsRg)
	    	Assert.True(ans1.SequenceEqual(f(start1, stop1)), "RangeTest");
		
		int step2 = -1, start2 = 4, stop2 = -1;
		int[] ans2 = {4, 3, 2, 1, 0};
		foreach (var f in funcsRgStep)
	    	Assert.True(ans2.SequenceEqual(f(step2, start2, stop2)), 
				"RangeStepTest");
	}
}

}
