namespace Introcs.Practice.Tests {

using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using FsCheck.Fluent;
using FsCheck;
using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;
using Microsoft.FSharp.Core;

using Util = Introcs.Util.Library;
using Seqops = Introcs.Practice.Sequenceops;
using SeqopsArr = Introcs.Practice.SequenceopsArray;

[TestFixture]
public class TpSequenceops : Base.ClsBase {
    private double epsilon = 0.001; //1.20e-7;
    /*
    public override void Dispose()
    {
        //Console.Error.WriteLine("Derived Dispose({0})", GetType());
        //base.Dispose();
    }*/
	
    [Property]
	public bool TabulateProp(uint x)
	{
		var funcsL = new Func<Func<int, int>, int, List<int>>[] {
			Seqops.TabulateLp};
	    var funcsA = new Func<Func<int, int>, int, int[]>[] {
			SeqopsArr.TabulateLp};
		int n = (int)x % 18 + 1;
		Func<int, int> proc = e => e + 2;
		var ans = Enumerable.Range(0, n).Select(e => proc(e));
		
		return funcsL.Aggregate(true, (acc, f) => acc &&
			ans.SequenceEqual(f(proc, n))) && funcsA.Aggregate(true, (acc, f) => acc &&
			ans.SequenceEqual(f(proc, n)));
	}
	
    [Property] [Category("Tag3")]
	public bool IndexFindProp(int el, List<int> xs)
	{
		FSharpFunc<int, bool> predFs = (Converter<int, bool>)(e => el == e);
		var funcsIdxL = new Func<int, List<int>, int>[] {Seqops.IndexOfLp,
			(el1, xs1) => SequenceopsFs.findIndexSeqI(predFs, xs1)};
	    var funcsFndL = new Func<int, List<int>, int>[] {Seqops.FindLp};
	    var funcsIdxA = new Func<int, int[], int>[] {SeqopsArr.IndexOfLp};
	    var funcsFndA = new Func<int, int[], int>[] {SeqopsArr.FindLp};
		var ansIdx = xs.IndexOf(el);
		var ansFnd = xs.Find(e => el == e);
		
		return funcsIdxL.Aggregate(true, (acc, f) => acc && 
			ansIdx == f(el, xs)) && funcsFndL.Aggregate(true, (acc, f) =>
			acc && ansFnd == f(el, xs)) && funcsIdxA.Aggregate(true, (acc, f) => acc && 
			ansIdx == f(el, xs.ToArray())) && funcsFndA.Aggregate(true, (acc, f) =>
			acc && ansFnd == f(el, xs.ToArray()));
	}
	
    [Property]
	public bool MinMaxProp(int n, List<int> lst)
	{
		var funcsMinMaxL = new (Func<List<int>, int>, Func<List<int>, int>)[] {
			(Seqops.MinLp, Seqops.MaxLp)};
	    var funcsMinMaxA = new (Func<int[], int>, Func<int[], int>)[] {
			(SeqopsArr.MinLp, SeqopsArr.MaxLp)};
		List<int> xs = lst.Prepend(n).ToList();
		
		return funcsMinMaxL.Aggregate(true, (acc, fMinMax) => acc &&
			xs.Min() == fMinMax.Item1(xs) && xs.Max() == fMinMax.Item2(xs)) &&
			funcsMinMaxA.Aggregate(true, (acc, fMinMax) => acc &&
			xs.Min() == fMinMax.Item1(xs.ToArray()) && xs.Max() == fMinMax.Item2(xs.ToArray()));
	}
    
    [Property] [Category("Tag3")]
	public bool ReverseProp(List<int> xs)
	{
		var funcsMutL = new Action<List<int>>[] {Seqops.ReverseLp};
		var funcsImmL = new Func<IEnumerable<int>, IEnumerable<int>>[] {
			SequenceopsFs.revSeqR, SequenceopsFs.revSeqI};
	    var funcsMutA = new Action<int[]>[] {SeqopsArr.ReverseLp};
		List<int> ys = xs.ToList();
		ys.Reverse();
		
		return funcsMutL.Aggregate(true, (acc, f) => {
			var tmp = Seqops.CopyOfLp<int>(xs);
			f(tmp);
			return acc && tmp.SequenceEqual(ys);
		}) && funcsImmL.Aggregate(true, (acc, f) => 
			acc && f(xs).SequenceEqual(ys)) && funcsMutA.Aggregate(true, (acc, f) => {
			var tmp = SeqopsArr.CopyOfLp<int>(xs.ToArray());
			f(tmp);
			return acc && tmp.SequenceEqual(ys);
		});
	}
	
    [Property]
	public bool CopyOfProp(List<int> xs)
	{
		var funcsL = new Func<List<int>, List<int>>[] {Seqops.CopyOfLp};
	    var funcsA = new Func<int[], int[]>[] {SeqopsArr.CopyOfLp};
		
		return funcsL.Aggregate(true, (acc, f) => acc && 
			xs.SequenceEqual(f(xs))) && funcsA.Aggregate(true, (acc, f) =>
				acc && xs.SequenceEqual(f(xs.ToArray())));
	}
	
    [Property]
	public bool ForeachProp(List<int> xs)
	{
		var funcsL = new Action<Action<int>, List<int>>[] {
			Seqops.ForeachLp};
	    var funcsA = new Action<Action<int>, int[]>[] {SeqopsArr.ForeachLp};
		Action<int> proc = e => Console.Write("{0} ", e);
		
		bool acc = true;
		xs.ForEach(proc);
		foreach (var f in funcsL)
			f(proc, xs);
		Array.ForEach(xs.ToArray(), proc);
		foreach (var f in funcsA)
			f(proc, xs.ToArray());
		return acc;
	}
	
    [Property]
	public bool SortOrderedProp(List<int> xs)
	{
		var funcsSortOrderedL = new (Action<List<int>, int, int>, Func<List<int>, Comparison<int>, bool>)[] {
			(Seqops.QuickSortLp, Seqops.IsOrderedLp)};
	    var funcsSortOrderedA = new (Action<int[], int, int>, Func<int[], Comparison<int>, bool>)[] {
			(SeqopsArr.QuickSortLp, SeqopsArr.IsOrderedLp)};
		Comparison<int> cmp = (a, b) => a.CompareTo(b);
	    Comparison<int> cmpRev = (a, b) => b.CompareTo(a);
	    
		bool acc = true;
		foreach (var fSortOrder in funcsSortOrderedL) {
			var res = xs.ToList();
			fSortOrder.Item1(res, 0, xs.Count - 1);
			acc &= xs.OrderBy(e => e).SequenceEqual(res) &&
				fSortOrder.Item2(res, cmp);
			
			var resRev = xs.ToList();
			fSortOrder.Item1(resRev, 0, xs.Count - 1);
			resRev.Reverse();
			acc &= xs.OrderBy(e => e).Reverse().SequenceEqual(resRev) &&
				fSortOrder.Item2(resRev, cmpRev);
		}
		foreach (var fSortOrder in funcsSortOrderedA) {
			var res = xs.ToArray();
			fSortOrder.Item1(res, 0, xs.Count - 1);
			acc &= xs.OrderBy(e => e).SequenceEqual(res) &&
				fSortOrder.Item2(res, cmp);
			
			var resRev = xs.ToArray();
			fSortOrder.Item1(resRev, 0, xs.Count - 1);
			Array.Reverse(resRev);
			acc &= xs.OrderBy(e => e).Reverse().SequenceEqual(resRev) &&
				fSortOrder.Item2(resRev, cmpRev);
		}
		return acc;
	}
	
    [Property]
	public bool AppendProp(List<int> xs, List<int> ys)
	{
		var funcsL = new Func<List<int>, List<int>, List<int>>[] {
			Seqops.AppendLp};
	    var funcsA = new Func<int[], int[], int[]>[] {SeqopsArr.AppendLp};
		
		return funcsL.Aggregate(true, (acc, f) => acc &&
			xs.Concat(ys).SequenceEqual(f(xs, ys))) && funcsA.Aggregate(true, (acc, f) => acc &&
			xs.Concat(ys).SequenceEqual(f(xs.ToArray(), ys.ToArray())));
	}
	
    [Property]
	public bool InterleaveProp(List<int> xs, List<int> ys)
	{
		var funcsL = new Func<List<int>, List<int>, List<int>>[] {
			Seqops.InterleaveLp};
	    var funcsA = new Func<int[], int[], int[]>[] {SeqopsArr.InterleaveLp};
		var lenShort = xs.Count < ys.Count ? xs.Count : ys.Count;
	    var ans = xs.Zip(ys, (e0, e1) => new int[] {e0, e1}).SelectMany(
			arr => arr).Concat(xs.Skip(lenShort).Concat(ys.Skip(lenShort)));
		
		return funcsL.Aggregate(true, (acc, f) => acc && 
			ans.SequenceEqual(f(xs, ys))) && funcsA.Aggregate(true, (acc, f) => acc && 
			ans.SequenceEqual(f(xs.ToArray(), ys.ToArray())));
	}
}

}
