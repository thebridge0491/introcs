namespace Introcs.Practice.Tests {

using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;

using Util = Introcs.Util.Library;
using Introcs.Practice;

[TestFixture]
public class TpClassic : Base.ClsBase {
    private double epsilon = 0.001; //1.20e-7;
    /*
    public override void Dispose()
    {
        //Console.Error.WriteLine("Derived Dispose({0})", GetType());
        //base.Dispose();
    }*/
	
    [Property]
	public bool SquareProp(uint x)
	{
		var funcs = new Func<float, float>[] {Classic.SquareLp, 
			Classic.SquareI};
		float n = (float)(x % 100 + 1);
		var ans = Math.Pow(n, 2.0f);
		
		return funcs.Aggregate(true, (acc, f) => acc && Util.InEpsilon(ans,
			f(n), epsilon * ans));
	}
    
    [Property] [Category("Tag3")]
	public bool ExptProp(uint x, uint y)
	{
		var funcs = new Func<float, float, float>[] {Classic.ExptLp, 
			Classic.ExptI, ClassicFs.exptI, ClassicFs.exptR,
			Classic.FastExptLp, Classic.FastExptI};
		float b = (float)(x % 19 + 1); float n = (float)(y % 10 + 1);
		var ans = Math.Pow(b, n);
		
		/*bool res = true;
		foreach (var f in funcs)
			res = res && Util.InEpsilon(ans, f(b, n), epsilon * ans);
		return res;*/
		return funcs.Aggregate(true, (acc, f) => acc && Util.InEpsilon(ans,
			f(b, n), epsilon * ans));
	}
    
    [Property]
	public bool SumToProp(uint x, uint y)
	{
		var funcs = new Func<long, long, long>[] {Classic.SumToLp,
			Classic.SumToI};
		int hi = (int)x % 201 - 50, lo = (int)y % 201 - 50;
		var ans = Enumerable.Range(lo, hi >= lo ? hi + 1 - lo : 0).Select(e => 
			(long)e).Aggregate(0L, (acc, e) => acc + e);
		
		return funcs.Aggregate(true, (acc, f) => acc && ans == f(hi, lo));
	}
    
    [Property] [Category("Tag3")]
	public bool FactProp(uint x)
	{
		var funcs = new Func<long, long>[] {Classic.FactLp, Classic.FactI,
			ClassicFs.factI, ClassicFs.factR};
		int n = (int)x % 19 + 1;
		var ans = Enumerable.Range(1, n).Select(e => 
			(long)e).Aggregate((acc, e) => acc * e);
		
		/*bool res = true;
		foreach (var f in funcs)
			res = res && ans == f(n);
		return res;*/
		return funcs.Aggregate(true, (acc, f) => acc && ans == f(n));
	}
    
    [Property]
	public bool FibProp(uint x)
	{
		var funcs = new Func<int, int>[] {Classic.FibLp, Classic.FibI};
		int n = (int)x % 20 + 1;
		var ans = Enumerable.Range(0, n).Aggregate((0, 1), (accTup, e) => (accTup.Item1 + accTup.Item2, accTup.Item1)).Item1;
		
		return funcs.Aggregate(true, (acc, f) => acc && ans == f(n));
	}
    
    [Property]
	public bool QuotRemProp(uint x, uint y)
	{
		int m = (int)x % 201 - 50, n = (int)y % 150 + 1;
		
		return (m / n) == Classic.QuotM(m, n) && (m % n) == Classic.RemM(m, n);
	}
	
	static Func<int, int, int> euclid = 
		(a, b) => 0 == b ? Math.Abs(a) : euclid(b, a % b);
#if false    
    [Property]
	public bool GcdLcmProp(uint n, uint[] nums)
	{
		if (0u == n || 0 == nums.Length)
			return true;
		int[] nums1 = nums.Select(e => 0 == e ? 1 : (int)e % 50 + 1).Take(5).ToArray();
		var funcs = new (Func<int[], int>, Func<int[], int>)[] {
			(Classic.GcdLp, Classic.LcmLp), (Classic.GcdI, Classic.LcmI)};
		int[] nNums = new int[nums1.Length + 1];
		nNums[0] = (int)n;
		Array.Copy(nums1, 0, nNums, 1, nums1.Length);
		var ansG = nNums.Aggregate((acc, e) => euclid(acc, e));
		var ansL = nNums.Aggregate((acc, e) => acc * e / euclid(acc, e));
		
		return funcs.Aggregate(true, (acc, fTup) => acc && ansG == fTup.Item1(nNums) && ansL == fTup.Item2(nNums));
	}
#endif    
    [Property]
	public bool BaseExpandProp(uint x, uint y)
	{
		var funcs = new Func<int, int, int[]>[] {Classic.BaseExpandLp, 
			Classic.BaseExpandI};
		int b = (int)x % 9 + 2, n = (int)y % 201 + 1;
		var ans = Enumerable.Range(0, (int)(Math.Log(n) / Math.Log(b)) + 1).Aggregate(((IEnumerable<int>)new List<int>(), n), (accTup, e) => 0 == accTup.Item2 ? (accTup.Item1, 0) : (accTup.Item1.Prepend((accTup.Item2 % b)), accTup.Item2 / b)).Item1;
		
		return funcs.Aggregate(true, (acc, f) => acc && ans.SequenceEqual(f(b, n)));
	}
    
    [Property]
	public bool BaseTo10Prop(uint x, uint y)
	{
		var funcs = new Func<int, int[], int>[] {Classic.BaseTo10Lp, 
			Classic.BaseTo10I};
		int b = (int)x % 9 + 2, n = (int)y % 201 + 1;
		var nums = Enumerable.Range(0, (int)(Math.Log(n) / Math.Log(b)) + 1).Aggregate(((IEnumerable<int>)new List<int>(), n), (accTup, e) => 0 == accTup.Item2 ? (accTup.Item1, 0) : (accTup.Item1.Prepend((accTup.Item2 % b)), accTup.Item2 / b)).Item1;
		var ans = nums.Reverse().Select((el, idx) => new Tuple<int, int>(idx, el)).Aggregate(0, (acc, ieTup) => acc + (ieTup.Item2 * (int)(Math.Pow(b, ieTup.Item1))));
		
		return funcs.Aggregate(true, (acc, f) => acc && ans == f(b, nums.ToArray()));
	}
    
    [Property]
	public bool RangeProp(uint x, uint y)
	{
		var funcsRgStep = new Func<int, int, int, int[]>[] {
			Classic.RangeStepLp, Classic.RangeStepI};
		var funcsRg = new Func<int, int, int[]>[] {Classic.RangeLp, 
			Classic.RangeI};
		int start = (int)x % 41 - 20, stop = (int)y % 41 - 20, step = -1;
		var ans = Enumerable.Range(start, stop >= start ? Math.Abs(stop - start) : 0);
		var ansRev = ans.AsEnumerable().Reverse().ToArray();
		
		return funcsRgStep.Aggregate(true, (acc, f) => acc && ansRev.SequenceEqual(f(step, stop - 1, start - 1))) && funcsRg.Aggregate(true, (acc, f) => acc && ans.SequenceEqual(f(start, stop)));
	}
}

}
