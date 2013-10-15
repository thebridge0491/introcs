namespace Introcs.Practice {

using System;
//using SysCollGen = System.Collections.Generic;
using System.Collections.Generic;

/// <summary>Sequenceops class.</summary>
public static class Sequenceops {
	private static readonly log4net.ILog log = 
		log4net.LogManager.GetLogger("prac");
	
	public static void SwapItems<T>(int a, int b, List<T> lst)
	{
		T swap = lst[a];
		lst.Insert(a, lst[b]); lst.RemoveAt(a + 1);
		lst.Insert(b, swap); lst.RemoveAt(b + 1);
	}
	
	public static List<T> TabulateLp<T>(Func<int, T> proc, int cnt)
	{
		List<T> newlst = new List<T>(cnt);
		for (int i = 0; cnt > i; ++i)
			newlst.Add(proc(i));
		return newlst;
	}
	
	public static Tuple<int, T> IndexFindLp<T>(T data, List<T> lst,
		Comparison<T> cmp)
	{
		for (int i = 0; lst.Count > i; ++i)
			if (0 == cmp(data, lst[i]))
				return new Tuple<int, T>(i, lst[i]);
		return new Tuple<int, T>(-1, default(T));
	}
	
	public static int IndexOfLp<T>(T data, List<T> lst) where T : IComparable
	{
		log.Info("IndexOfLp()");
		return IndexFindLp(data, lst, (T a, T b) => a.CompareTo(b)).Item1;
	}
	
	public static T FindLp<T>(T data, List<T> lst) where T : IComparable
	{
		return IndexFindLp(data, lst, (T a, T b) => a.CompareTo(b)).Item2;
	}
	
	public static T[] MinMaxLp<T>(List<T> lst) where T : IComparable
	{
		if (0 == lst.Count)
			return new T[] {default(T), default(T)};
		T[] arrMinMax = {lst[0], lst[0]};
		foreach (var v in lst)
			if (0 > v.CompareTo(arrMinMax[0]))
				arrMinMax[0] = v;
			else if (0 < v.CompareTo(arrMinMax[1]))
				arrMinMax[1] = v;
		return arrMinMax;
	}
	
	public static T MinLp<T>(List<T> lst) where T : IComparable
	{
		return MinMaxLp(lst)[0];
	}
	
	public static T MaxLp<T>(List<T> lst) where T : IComparable
	{
		return MinMaxLp(lst)[1];
	}
	
	public static void ReverseLp<T>(List<T> lst)
	{
		for (int i = 0, j = lst.Count - 1; j > i; ++i, --j)
			SwapItems<T>(i, (int)j, lst);
	}
	
	public static List<T> CopyOfLp<T>(List<T> lst)
	{
		var newLst = new List<T>();
	
		for (var i = 0; lst.Count > i; ++i)
			newLst.Add(lst[i]);
		return newLst;
	}
	
	public static void ForeachLp<T>(Action<T> proc, List<T> lst)
	{
		foreach (var v in lst)
			proc(v);
	}
	
	public static bool IsOrderedLp<T>(List<T> lst, Comparison<T> cmp)
	{
		for (int i = 1; lst.Count > i; ++i)
			if (0 < cmp(lst[i - 1], lst[i]))
				return false;
		return true;
	}
	
	private static int Qpartition<T>(List<T> lst, int lo, int hi) where T : IComparable
	{
		int lwr = lo, upr = hi;
		
		while (lwr < upr) {
			while (0 >= lst[lwr].CompareTo(lst[lo]) && lwr < upr)
				++lwr;
			while (0 < lst[upr].CompareTo(lst[lo]))
				--upr;
			if (lwr < upr)
				SwapItems(lwr, upr, lst);
		}
		SwapItems(lo, upr, lst);
		return upr;
	}
	
	public static void QuickSortLp<T>(List<T> lst, int lo, int hi) where T : IComparable
	{
		DateTime time1 = DateTime.Now;
		Random rnd = new Random(time1.Millisecond);
		if (hi > lo) {
			int rNdx = rnd.Next(0, hi - lo + 1) + lo;
			SwapItems(lo, rNdx, lst);
			int split = Qpartition(lst, lo, hi);
			QuickSortLp(lst, lo, split - 1);
			QuickSortLp(lst, split + 1, hi);
		}
	}
	
	public static List<T> AppendLp<T>(List<T> lst1, List<T> lst2)
	{
		List<T> newlst = new List<T>(lst1.Count + lst2.Count);
		foreach (var v in lst1)
			newlst.Add(v);
		foreach (var v in lst2)
			newlst.Add(v);
		return newlst;
	}
	
	public static List<T> InterleaveLp<T>(List<T> lst1, List<T> lst2)
	{
		List<T> newlst = new List<T>(lst1.Count + lst2.Count);
		for (int i = 0, j = 0; lst1.Count > i || lst2.Count > j; ++i, ++j) {
			if (lst1.Count > i)
				newlst.Add(lst1[i]);
			if (lst2.Count > j)
				newlst.Add(lst2[j]);
		}
		return newlst;
	}
	
	/// <summary>Main entry point.</summary>
    /// <param name="args">An array</param>
    /// <returns>The exit code.</returns>
	public static int Main(string[] args)
    {
		int n = 3;
		int[] arr1 = {2, 1, 0, 4, 3};
		var lst = new List<int>(arr1);
		Console.Write("IndexOf({0}, [{1}]) : {2}\n", n,
			String.Join(", ", lst), IndexOfLp(n, lst));
		return 0;
	}
}

}
