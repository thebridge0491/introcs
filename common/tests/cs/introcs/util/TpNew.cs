namespace Introcs.Util.Tests {

using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using FsCheck;

using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;
using MyArbitraries = Base.MyArbitraries;

using Util = Introcs.Util.Library;

[TestFixture]
public class TpNew : Base.ClsBase {
  private double epsilon = 0.001; //1.20e-7;
  /*
  public override void Dispose()
  {
      //Console.Error.WriteLine("Derived Dispose({0})", GetType());
      //base.Dispose();
  }*/

  // nunit spec style - call quickcheck
  [Test] [Category("Tag3")]
	public void CommutAddCheck()
	{
		Func<int, int, bool> func = (a, b) => (a + b) == (b + a);
		Prop.ForAll<int, int>(func).QuickCheckThrowOnFailure();
	}

	[Test] [Category("Tag3")]
	public void AssocAddCheck()
	{
		Func<int, int, int, bool> func = (x, y, z) => {
			double a = (double)x; double b = (double)y; double c = (double)z;
			return Util.InEpsilon((a + b) + c, a + (b + c),
			    epsilon * ((a + b) + c));
			};
		Prop.ForAll<int, int, int>(func).QuickCheckThrowOnFailure();
	}

	[Test] [Category("Tag3")]
	public void RevRevCheck()
	{
		Func<int[], bool> func = xs =>
	        xs.Reverse().Reverse().SequenceEqual(xs);
		Prop.ForAll<int[]>(func).QuickCheckThrowOnFailure();
	}

	[Test]
	public void IdRevCheck()
	{
		Func<double[], bool> func = xs => xs.Reverse().SequenceEqual(xs);
		Prop.ForAll<double[]>(func).QuickCheckThrowOnFailure();
	}

	[Test] [Category("Tag3")]
	public void SortRevCheck()
	{
		Func<double[], bool> func = xs => {
			double[] ys = xs.Clone() as double[];
			Array.Sort(ys);
			Array.Reverse(xs);
			Array.Sort(xs);
			return xs.SequenceEqual(ys);
			};
		Prop.ForAll<double[]>(func).QuickCheckThrowOnFailure();
	}

	[Test] [Category("Tag3")]
	public void MinSortHeadCheckA()
	{
		Func<FsCheck.NonEmptyArray<FsCheck.NormalFloat>, bool> func = xs => {
			double[] ys = xs.Get.Select(e => e.Get).ToArray();
			Array.Sort(ys);
			return Util.InEpsilon(xs.Get.Min().Get, ys[0], epsilon * ys[0]);
			};
		Prop.ForAll<FsCheck.NonEmptyArray<FsCheck.NormalFloat>>(
		    func).QuickCheckThrowOnFailure();
	}

	[Test] [Category("Tag3")]
	public void MinSortHeadCheckB()
	{
		Func<IEnumerable<double>, bool> func = xs => {
			double[] ys = xs.Select(e => e).ToArray();
			Array.Sort(ys);
			return Util.InEpsilon(xs.Min(), ys[0], epsilon * xs.Min());
			};
		Arb.Register<MyArbitraries>();
		Prop.ForAll<IEnumerable<double>>(func).QuickCheckThrowOnFailure();
	}

	// fscheck property style
    [Property(MaxTest = 20, Verbose = true)] [Category("Tag3")]
	public bool CommutAddProp(int a, int b)
	{
		return (a + b) == (b + a);
	}

	[Property] [Category("Tag3")]
	public bool AssocAddProp(int x, int y, int z)
	{
		double a = (double)x; double b = (double)y; double c = (double)z;
		return Util.InEpsilon((a + b) + c, a + (b + c), epsilon * ((a + b) + c));
	}

	[Property] [Category("Tag3")]
	public bool RevRevProp(int[] xs)
	{
		return xs.Reverse().Reverse().SequenceEqual(xs);
	}

	[Property]
	public bool IdRevProp(double[] xs)
	{
		return xs.Reverse().SequenceEqual(xs);
	}

	[Property] [Category("Tag3")]
	public bool SortRevProp(double[] xs)
	{
		double[] ys = xs.Clone() as double[];
		Array.Sort(ys);
		Array.Reverse(xs);
		Array.Sort(xs);
		return xs.SequenceEqual(ys);
	}

	[Property] [Category("Tag3")]
	public bool MinSortHeadPropA(FsCheck.NonEmptyArray<FsCheck.NormalFloat> xs)
	{
		double[] ys = xs.Get.Select(e => e.Get).ToArray();
		Array.Sort(ys);
		return Util.InEpsilon(xs.Get.Min().Get, ys[0], epsilon * ys[0]);
	}

	[Property(Arbitrary = new[] { typeof(MyArbitraries) })] [Category("Tag3")]
	public bool MinSortHeadPropB(IEnumerable<double> xs)
	{
		double[] ys = xs.Select(e => e).ToArray();
		Array.Sort(ys);
		return Util.InEpsilon(xs.Min(), ys[0], epsilon * xs.Min());
	}
}

}
