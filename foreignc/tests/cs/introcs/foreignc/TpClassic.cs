namespace Introcs.Foreignc.Tests {

using System;
using System.Linq;
using NUnit.Framework;
using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;

using Util = Introcs.Util.Library;
using Introcs.Foreignc;

[TestFixture]
public class TpClassic : Base.ClsBase {
  private double epsilon = 0.001; //1.20e-7;
  /*
  public override void Dispose()
  {
      //Console.Error.WriteLine("Derived Dispose({0})", GetType());
      //base.Dispose();
  }*/

  [Property] [Category("Tag3")]
	public bool FactProp(uint x)
	{
		var funcs = new Func<long, long>[] {Classic.FactLp, Classic.FactI};
		int n = (int)x % 19 + 1;
		var ans = Enumerable.Range(1, n).Select(e =>
			(long)e).Aggregate((acc, e) => acc * e);

		/*bool res = true;
		foreach (var f in funcs)
			res = res && ans == f(n);
		return res;*/
		return funcs.Aggregate(true, (acc, f) => acc && ans == f(n));
	}

  [Property] [Category("Tag3")]
	public bool ExptProp(uint x, uint y)
	{
		var funcs = new Func<float, float, float>[] {Classic.ExptLp,
			Classic.ExptI};
		float b = (float)(x % 19 + 1); float n = (float)(y % 10 + 1);
		var ans = Math.Pow(b, n);

		/*bool res = true;
		foreach (var f in funcs)
			res = res && Util.InEpsilon(ans, f(b, n), epsilon * ans);
		return res;*/
		return funcs.Aggregate(true, (acc, f) => acc && Util.InEpsilon(ans,
			f(b, n), epsilon * ans));
	}
}

}
