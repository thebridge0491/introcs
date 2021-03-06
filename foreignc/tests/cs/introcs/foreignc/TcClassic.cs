namespace Introcs.Foreignc.Tests {

using System;
using System.Linq;
using NUnit.Framework;

using Introcs.Foreignc;

[TestFixture]
public class TcClassic : Base.ClsBase {
  private float epsilon = 0.001f; //1.20e-7f;

  [OneTimeSetUp]
  public override void SetUpClass()
  {
      base.SetUpClass();
      Console.Error.WriteLine("SetUpClass({0})", GetType());
  }

  [OneTimeTearDown]
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
	public void FactTest()
	{
		var funcs = new Func<long, long>[] {Classic.FactLp, Classic.FactI};
		int n = 5;

		foreach (var f in funcs)
	    	Assert.AreEqual(120, f(n), "FactTest");
	}

	[Test] [Category("Tag1")]
	public void ExptTest()
	{
		var funcs = new Func<float, float, float>[] {Classic.ExptLp,
			Classic.ExptI};
		float[] param1 = {2.0f, 11.0f, 20.0f}, param2 = {3.0f, 6.0f, 10.0f};
	    var prodParams =		// cartesian product (System.Linq)
	    	from b in param1
	    	from n in param2
	    	select new {b, n};
	    foreach (var f in funcs)
			foreach (var tup in prodParams) {
				float ans = (float)Math.Pow(tup.b, tup.n);
				Assert.AreEqual(ans, f(tup.b, tup.n),
					(ans * epsilon), "ExptTest");
			}
	}
}

}
