namespace Introcs.Practice.Tests {

using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.FSharp.Core;

using Util = Introcs.Util.Library;
using Seqops = Introcs.Practice.Sequenceops;
using SeqopsArr = Introcs.Practice.SequenceopsArray;

[TestFixture]
public class TcSequenceops : Base.ClsBase {
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
	public void TabulateTest()
	{
	    var funcsL = new Func<Func<int, int>, int, List<int>>[] {
			Seqops.TabulateLp};
	    var funcsA = new Func<Func<int, int>, int, int[]>[] {
			SeqopsArr.TabulateLp};
		
		int n = 5;
		var lst = Enumerable.Range(0, n).ToList();
	    var lstRev = Enumerable.Range(0, n).Reverse().ToList();
	    
	    foreach (var f in funcsL)
	    	Assert.True(lst.SequenceEqual(f(i => i, n)) &&
				lstRev.SequenceEqual(f(i => n - 1 - i, n)), "TabulateTestL");
	    foreach (var f in funcsA)
	    	Assert.True(lst.SequenceEqual(f(i => i, n)) &&
				lstRev.SequenceEqual(f(i => n - 1 - i, n)), "TabulateTestA");
	}
	
	[Test] [Category("Tag1")]
	public void IndexFindTest()
	{
	    int el = 3;
	    FSharpFunc<int, bool> predFs = (Converter<int, bool>)(e => el == e);
	    var funcsIdxL = new Func<int, List<int>, int>[] {Seqops.IndexOfLp,
			(el1, xs) => SequenceopsFs.findIndexSeqI(predFs, xs)};
	    var funcsFndL = new Func<int, List<int>, int>[] {Seqops.FindLp};
	    var funcsIdxA = new Func<int, int[], int>[] {SeqopsArr.IndexOfLp};
	    var funcsFndA = new Func<int, int[], int>[] {SeqopsArr.FindLp};
	    int n = 5;
	    List<int>[] lsts = {Enumerable.Range(0, n).ToList(),
			Enumerable.Range(0, n).Reverse().ToList()};
	    
	    foreach (var lst in lsts) {
			foreach (var f in funcsIdxL)
				Assert.AreEqual(lst.IndexOf(el), f(el, lst), "IndexOfTestL");
			foreach (var f in funcsFndL)
				Assert.AreEqual(lst.Find(e => el == e), f(el, lst),
					"FindTestL");
			foreach (var f in funcsIdxA)
				Assert.AreEqual(lst.IndexOf(el), f(el, lst.ToArray()), "IndexOfTestA");
			foreach (var f in funcsFndA)
				Assert.AreEqual(lst.Find(e => el == e), f(el, lst.ToArray()),
					"FindTestA");
	    }
	}
	
	[Test]
	public void MinMaxTest()
	{
	    var funcsMinMaxL = new (Func<List<int>, int>, Func<List<int>, int>)[] {
			(Seqops.MinLp, Seqops.MaxLp)};
	    var funcsMinMaxA = new (Func<int[], int>, Func<int[], int>)[] {
			(SeqopsArr.MinLp, SeqopsArr.MaxLp)};
	    int n = 5;
	    List<int>[] lsts = {Enumerable.Range(0, n).ToList(),
			Enumerable.Range(0, n).Reverse().ToList()};
	    
	    foreach (var lst in lsts) {
			foreach (var fTup in funcsMinMaxL)
				Assert.True(lst.Min() == fTup.Item1(lst) &&
					lst.Max() == fTup.Item2(lst), "MinMaxTestL");
			foreach (var fTup in funcsMinMaxA)
				Assert.True(lst.Min() == fTup.Item1(lst.ToArray()) &&
					lst.Max() == fTup.Item2(lst.ToArray()), "MinMaxTestA");
	    }
	}
    
	[Test] [Category("Tag1")]
	public void ReverseTest()
	{
	    var funcsMutL = new Action<List<int>>[] {Seqops.ReverseLp};
	    var funcsImmL = new Func<IEnumerable<int>, IEnumerable<int>>[] {
			SequenceopsFs.revSeqR, SequenceopsFs.revSeqI};
	    var funcsMutA = new Action<int[]>[] {SeqopsArr.ReverseLp};
	    int n = 5;
		var lst = Enumerable.Range(0, n).ToList();
	    
	    foreach (var f in funcsMutL) {
	    	var tmp = Seqops.CopyOfLp<int>(lst);
	    	f(tmp);
			
			for (int i = 0, j = lst.Count - 1; lst.Count > i; ++i, --j)
				Assert.AreEqual(lst[i], tmp[j], "ReverseTest mutL");
		}
	    foreach (var f in funcsImmL)
	    	Assert.True(lst.AsEnumerable().Reverse().SequenceEqual(f(lst)),
				"ReverseTest immL");
	    foreach (var f in funcsMutA) {
	    	var tmp = SeqopsArr.CopyOfLp<int>(lst.ToArray());
	    	f(tmp);
			
			for (int i = 0, j = lst.Count - 1; lst.Count > i; ++i, --j)
				Assert.AreEqual(lst[i], tmp[j], "ReverseTest mutA");
		}
	}
    
	[Test]
	public void CopyOfTest()
	{
	    var funcsL = new Func<List<int>, List<int>>[] {Seqops.CopyOfLp};
	    var funcsA = new Func<int[], int[]>[] {SeqopsArr.CopyOfLp};
	    int n = 5;
		var lst = Enumerable.Range(0, n).ToList();
	    
	    foreach (var f in funcsL)
			Assert.True(lst.SequenceEqual(f(lst)), "CopyTestL");
	    foreach (var f in funcsA)
			Assert.True(lst.SequenceEqual(f(lst.ToArray())), "CopyTestA");
	}
    
	[Test]
	public void ForeachTest()
	{
	    var funcsL = new Action<Action<int>, List<int>>[] {
			Seqops.ForeachLp};
	    var funcsA = new Action<Action<int>, int[]>[] {SeqopsArr.ForeachLp};
	    int n = 5;
		var lst = Enumerable.Range(0, n).ToList();
		Action<int> proc = el => Console.Write("{0} ", el);
	    
	    foreach (var f in funcsL) {
			lst.ForEach(proc);
			f(proc, lst);
			Assert.True(true, "ForeachTestL");
		}
	    foreach (var f in funcsA) {
			lst.ForEach(proc);
			f(proc, lst.ToArray());
			Assert.True(true, "ForeachTestA");
		}
	}
	
	[Test]
	public void SortOrderedTest()
	{
	    var funcsSortOrderedL = new (Action<List<int>, int, int>, Func<List<int>, Comparison<int>, bool>)[] {
			(Seqops.QuickSortLp, Seqops.IsOrderedLp)};
	    var funcsSortOrderedA = new (Action<int[], int, int>, Func<int[], Comparison<int>, bool>)[] {
			(SeqopsArr.QuickSortLp, SeqopsArr.IsOrderedLp)};
	    List<int> lst1 = (new int[] {9, 9, 9, 0, 3, 4}).ToList();
	    List<int> lst2 = (new int[] {4, 0, 9, 9, 9, 3}).ToList();
	    Comparison<int> cmp = (a, b) => a.CompareTo(b);
	    Comparison<int> cmpRev = (a, b) => b.CompareTo(a);
	    
	    foreach (var fTup in funcsSortOrderedL) {
			var ans = lst1.OrderBy(e => e).ToList();
			//ans.Sort(cmp);
			var res = lst1.ToList();
			fTup.Item1(res, 0, lst1.Count - 1);
			
			Assert.True(ans.SequenceEqual(res) &&
				fTup.Item2(ans, cmp) == fTup.Item2(res, cmp),
				"SortOrderedTestL");
			
			var ansRev = lst2.OrderBy(e => e).Reverse().ToList();
			var resRev = lst2.ToList();
			fTup.Item1(resRev, 0, lst2.Count - 1);
			resRev.Reverse();
			
			Assert.True(ansRev.SequenceEqual(resRev) && fTup.Item2(ansRev, 
				cmpRev) == fTup.Item2(resRev, cmpRev),
				"RevSortRevOrderedTestL");			
		}
	    foreach (var fTup in funcsSortOrderedA) {
			var ans = lst1.OrderBy(e => e).ToArray();
			//ans.Sort(cmp);
			var res = lst1.ToArray();
			fTup.Item1(res, 0, lst1.Count - 1);
			
			Assert.True(ans.SequenceEqual(res) &&
				fTup.Item2(ans, cmp) == fTup.Item2(res, cmp),
				"SortOrderedTestA");
			
			var ansRev = lst2.OrderBy(e => e).Reverse().ToArray();
			var resRev = lst2.ToArray();
			fTup.Item1(resRev, 0, lst2.Count - 1);
			Array.Reverse(resRev);
			
			Assert.True(ansRev.SequenceEqual(resRev) && fTup.Item2(ansRev, 
				cmpRev) == fTup.Item2(resRev, cmpRev),
				"RevSortRevOrderedTestA");			
		}
	}
    
	[Test]
	public void AppendTest()
	{
	    var funcsL = new Func<List<int>, List<int>, List<int>>[] {
			Seqops.AppendLp};
	    var funcsA = new Func<int[], int[], int[]>[] {SeqopsArr.AppendLp};
	    int n = 5;
		var lst = Enumerable.Range(0, n).ToList();
	    var lstRev = Enumerable.Range(0, n + 2).Reverse().ToList();
	    var ans = lst.AsEnumerable().Concat(lstRev);
	    
	    foreach (var f in funcsL)
			Assert.True(ans.SequenceEqual(f(lst, lstRev)), "AppendTestL");
	    foreach (var f in funcsA)
			Assert.True(ans.SequenceEqual(f(lst.ToArray(),
				lstRev.ToArray())), "AppendTestA");
	}
    
	[Test]
	public void InterleaveTest()
	{
	    var funcsL = new Func<List<int>, List<int>, List<int>>[] {
			Seqops.InterleaveLp};
	    var funcsA = new Func<int[], int[], int[]>[] {SeqopsArr.InterleaveLp};
	    int n = 5;
		var lst = Enumerable.Range(0, n).ToList();
	    var lstRev = Enumerable.Range(0, n + 2).Reverse().ToList();
	    var lenShort = lst.Count < lstRev.Count ? lst.Count : lstRev.Count;
	    var ans = lst.AsEnumerable().Zip(lstRev, (e0, e1) => 
			new int[] {e0, e1}).SelectMany(arr => arr).Concat(
			lst.Skip(lenShort).Concat(lstRev.Skip(lenShort)));
	    
	    foreach (var f in funcsL)
			Assert.True(ans.SequenceEqual(f(lst, lstRev)), "InterleaveTestL");
	    foreach (var f in funcsA)
			Assert.True(ans.SequenceEqual(f(lst.ToArray(), lstRev.ToArray())),
				"InterleaveTestA");
	}
}

}
