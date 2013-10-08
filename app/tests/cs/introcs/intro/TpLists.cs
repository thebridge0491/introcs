namespace Introcs.Intro.Tests {

using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using FsCheck;
using FsCheck.Fluent;

using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;
using MyArbitraries = Base.MyArbitraries;

using Util = Introcs.Util.Library;

[TestFixture]
public class TpLists : Base.ClsBase {
    private double epsilon = 0.001; //1.20e-7;
	
	public bool IsOrdered<T>(List<T> lst, Comparison<T> cmp)
	{
		for (int i = 1; lst.Count > i; ++i)
			if (0 < cmp(lst[i - 1], lst[i]))
				return false;
		return true;
	}
    /*
    public override void Dispose()
    {
        //Console.Error.WriteLine("Derived Dispose({0})", GetType());
        //base.Dispose();
    }*/
	
	[Property] [Category("Tag5")]
	public bool EqualProp(List<int> xs)
	{
		List<int> ys = new List<int>(xs);
		return ys.SequenceEqual(xs);
	}
	
	[Property] [Category("Tag5")]
	public bool NotEqualProp(List<int> xs)
	{
		List<int> ys = new List<int>(xs);
		ys.Add(100);
		return !ys.SequenceEqual(xs);
	}
	
	[Property] [Category("Tag5")]
	public bool AppendProp(List<int> xs, List<int> ys)
	{
		List<int> zs = new List<int>(xs);
		zs.AddRange(ys);
		return zs.GetRange(0, xs.Count).SequenceEqual(xs) &&
			zs.GetRange(xs.Count, ys.Count).SequenceEqual(ys);
	}
	
	[Property] [Category("Tag5")]
	public bool RevRevProp(List<int> xs)
	{
		List<int> ys = new List<int>(xs);
		ys.Reverse();
		ys.Reverse();
		return ys.SequenceEqual(xs);
	}
	
	[Property] [Category("Tag5")]
	public bool FilterProp(List<int> xs)
	{
		Predicate<int> pred = el => 0 == el % 2;
		List<int> ys = xs.FindAll(pred);
		return ys.FindAll(el => !pred(el)).SequenceEqual(new List<int>()) &&
			ys.Aggregate(true, (acc, e) => acc && pred(e));
	}
	
	[Property] [Category("Tag5")]
	public bool MapProp(List<int> xs)
	{
		Func<int, int> proc1 = el => el + 2;
		List<int> ys = xs.Select(proc1).ToList();
		return xs.Aggregate((true, ys), (accTup, e) =>
			(accTup.Item1 && proc1(e) == accTup.Item2.ElementAt(0),
			accTup.Item2.GetRange(1, accTup.Item2.Count - 1))).Item1;
	}
	
	[Property] [Category("Tag5")]
	public bool SortIsOrderedProp(List<int> xs)
	{
		List<int> ys = new List<int>(xs);
		ys.Sort();
		return IsOrdered(ys, (e0, e1) => e0.CompareTo(e1));
	}
	
	[Property] [Category("Tag5")]
	public bool RevSortIsRevOrderedProp(List<int> xs)
	{
		List<int> ys = new List<int>(xs);
		ys.Sort();
		ys.Reverse();
		return IsOrdered(ys, (e0, e1) => e1.CompareTo(e0));
	}
}

}
