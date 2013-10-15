namespace Introcs.Practice {

using System;

/// <summary>Sequenceops (arrays) class.</summary>
public static class SequenceopsArray {
	public static void SwapItems<T>(int a, int b, T[] arr)
	{
		T swap = arr[a]; arr[a] = arr[b]; arr[b] = swap;
	}
	
	public static T[] TabulateLp<T>(Func<int, T> proc, int cnt)
	{
		T[] newarr = new T[cnt];
		for (int i = 0; cnt > i; ++i)
			newarr[i] = proc(i);
		return newarr;
	}
	
	public static Tuple<int, T> IndexFindLp<T>(T data, T[] arr,
		Comparison<T> cmp)
	{
		for (int i = 0; arr.Length > i; ++i)
			if (0 == cmp(data, arr[i]))
				return new Tuple<int, T>(i, arr[i]);
		return new Tuple<int, T>(-1, default(T));
	}
	
	public static int IndexOfLp<T>(T data, T[] arr) where T : IComparable
	{
		return IndexFindLp(data, arr, (T a, T b) => a.CompareTo(b)).Item1;
	}
	
	public static T FindLp<T>(T data, T[] arr) where T : IComparable
	{
		return IndexFindLp(data, arr, (T a, T b) => a.CompareTo(b)).Item2;
	}
	
	public static T[] MinMaxLp<T>(T[] arr) where T : IComparable
	{
		if (0 == arr.Length)
			return new T[] {default(T), default(T)};
		T[] arrMinMax = {arr[0], arr[0]};
		foreach (var v in arr)
			if (0 > v.CompareTo(arrMinMax[0]))
				arrMinMax[0] = v;
			else if (0 < v.CompareTo(arrMinMax[1]))
				arrMinMax[1] = v;
		return arrMinMax;
	}
	
	public static T MinLp<T>(T[] arr) where T : IComparable
	{
		return MinMaxLp(arr)[0];
	}
	
	public static T MaxLp<T>(T[] arr) where T : IComparable
	{
		return MinMaxLp(arr)[1];
	}
	
	public static void ReverseLp<T>(T[] arr)
	{
		for (int i = 0, j = arr.Length - 1; j > i; ++i, --j)
			SwapItems<T>(i, (int)j, arr);
	}
	
	public static T[] CopyOfLp<T>(T[] arr)
	{
		var newArr = new T[arr.Length];
	
		for (var i = 0; arr.Length > i; ++i)
			newArr[i] = arr[i];
		return newArr;
	}
	
	public static void ForeachLp<T>(Action<T> proc, T[] arr)
	{
		foreach (var v in arr)
			proc(v);
	}
	
	public static bool IsOrderedLp<T>(T[] arr, Comparison<T> cmp)
	{
		for (int i = 1; arr.Length > i; ++i)
			if (0 < cmp(arr[i - 1], arr[i]))
				return false;
		return true;
	}
	
	private static int Qpartition<T>(T[] arr, int lo, int hi) where T : IComparable
	{
		int lwr = lo, upr = hi;
		
		while (lwr < upr) {
			while (0 >= arr[lwr].CompareTo(arr[lo]) && lwr < upr)
				++lwr;
			while (0 < arr[upr].CompareTo(arr[lo]))
				--upr;
			if (lwr < upr)
				SwapItems(lwr, upr, arr);
		}
		SwapItems(lo, upr, arr);
		return upr;
	}
	
	public static void QuickSortLp<T>(T[] arr, int lo, int hi) where T : IComparable
	{
		DateTime time1 = DateTime.Now;
		Random rnd = new Random(time1.Millisecond);
		if (hi > lo) {
			int rNdx = rnd.Next(0, hi - lo + 1) + lo;
			SwapItems(lo, rNdx, arr);
			int split = Qpartition(arr, lo, hi);
			QuickSortLp(arr, lo, split - 1);
			QuickSortLp(arr, split + 1, hi);
		}
	}
	
	public static T[] AppendLp<T>(T[] arr1, T[] arr2)
	{
		T[] newarr = new T[arr1.Length + arr2.Length];
		for (int i = 0; arr1.Length > i; ++i)
			newarr[i] = arr1[i];
		for (int i = 0; arr2.Length > i; ++i)
			newarr[arr1.Length + i] = arr2[i];
		return newarr;
	}
	
	public static T[] InterleaveLp<T>(T[] arr1, T[] arr2)
	{
		T[] newarr = new T[arr1.Length + arr2.Length];
		for (int i = 0, j = 0, k = 0; arr1.Length > i || arr2.Length > j; ++i, ++j) {
			if (arr1.Length > i)
				newarr[k++] = arr1[i];
			if (arr2.Length > j)
				newarr[k++] = arr2[j];
		}
		return newarr;
	}
	
	/// <summary>Main entry point.</summary>
    /// <param name="args">An array</param>
    /// <returns>The exit code.</returns>
	public static int Main(string[] args)
    {
		int n = 3;
		int[] arr1 = {2, 1, 0, 4, 3};
		Console.Write("IndexOf({0}, [{1}]) : {2}\n", n,
			String.Join(", ", arr1), IndexOfLp(n, arr1));
		return 0;
	}
}

}
