namespace Introcs.Practice {

using System;

/// <summary>Classic class.</summary>
public static class Classic {
	private static readonly log4net.ILog log = 
		log4net.LogManager.GetLogger("prac");
	
	public static float ExptLp(float b, float n)
	{
		float acc = 1.0f;
		
		for (float i = n; 0.0f < i; --i)
			acc *= b;
		return acc;
	}
	
	private static float ExptIter(float b, float n, float acc)
	{
		return n > 0.0f ? ExptIter(b, n - 1.0f, acc * b) : acc;
	}
	public static float ExptI(float b, float n)
	{
		return ExptIter(b, n, 1.0f);
	}
	
	public static float FastExptLp(float b, float n)
	{
		float acc = 1.0f, num = n;
		
		while (num > 0.0f) {
			if (0 == num % 2) {
				acc *= b * b;
				num -= 2.0f;
			} else {
				acc *= b;
				num -= 1.0f;
			}
		}
		return acc;
	}
	
	private static float FastExptIter(float b, float n, float acc)
	{
		if (0.0f >= n)
			return acc;
		else if (0 == n % 2)
			return FastExptIter(b, n - 2.0f, acc * (float)Math.Pow(b, 2.0f));
		return FastExptIter(b, n - 1.0f, acc * b);
	}
	public static float FastExptI(float b, float n)
	{
		return FastExptIter(b, n, 1.0f);
	}
	
	public static float SquareLp(float n)
	{
		return ExptLp(n, 2.0f);
	}
	
	public static float SquareI(float n)
	{
		return ExptI(n, 2.0f);
	}
	
	public static long NumSeqMathLp(Func<long, long, long> op, long init,
		long hi, long lo)
	{
		long acc = init;
		for (long i = lo; hi >= i; ++i)
			acc = op(acc, i);
		return acc;
	}
	
	private static long NumSeqMathIter(Func<long, long, long> op, long acc,
		long hi, long lo)
	{
		return hi >= lo ? NumSeqMathIter(op, op(acc, lo), hi, lo + 1L) : acc;
	}
	public static long NumSeqMathI(Func<long, long, long> op, long init,
		long hi, long lo)
	{
		return NumSeqMathIter(op, init, hi, lo);
	}

	public static long SumToLp(long hi, long lo)
	{
		return NumSeqMathLp((a, b) => a + b, 0L, hi, lo);
	}
	
	public static long SumToI(long hi, long lo)
	{
		return NumSeqMathI((a, b) => a + b, 0L, hi, lo);
	}

	public static long FactLp(long n)
	{
		log.Info("FactLp()");
		return NumSeqMathLp((a, b) => a * b, 1L, n, 1L);
	}
	
	public static long FactI(long n)
	{
		return NumSeqMathI((a, b) => a * b, 1L, n, 1L);
	}
	
	public static int FibLp(int n)
	{
		int acc = 0;
		
		for (int sum0 = 0, sum1 = 1, ct = n; 0 <= ct; --ct) {
			acc = sum0;
			sum0 = sum1;
			sum1 = sum1 + acc;
		}
		return acc;
	}
	
	private static int FibIter(int s0, int s1, int cnt)
	{
		return 0 >= cnt ? s0 : FibIter(s1, s0 + s1, cnt -1);
	}
	public static int FibI(int n)
	{
		return FibIter(0, 1, n);
	}
	
	private static void MkRows(int n, int[][] arr2d)
	{
		for (int row = 1; n >= row; ++row) {
			arr2d[row][0] = arr2d[row][row] = 1;
			for (int col = 1; row > col; ++col)
				arr2d[row][col] = arr2d[row - 1][col - 1] + 
					arr2d[row - 1][col];
		}
	}
	public static int[][] PascalTriAdd(int n)
	{
		int[][] result = new int[n + 1][];
		for (int row = 0; n >= row; ++row)
			for (int col = 0; row >= col; ++col)
				result[row] = new int[row + 1];
		result[0][0] = 1;
		MkRows(n, result);
		return result;
	}
	
	public static void PrintPascalTri(int n, int[][] arr2d)
	{
		if (null == arr2d)
			return;
		for (int row = 0; n >= row; ++row, Console.WriteLine())
			for (int col = 0; row >= col; ++col)
				Console.Write("{0,-3} ", arr2d[row][col]);
		Console.WriteLine();
	}
	
	public static int QuotM(int n, int d)
	{
		return n / d;
	}
	
	public static int RemM(int n, int d)
	{
		return n % d;
	}
	
	private static int EuclidLp(int m, int n)
	{
		int acc = m;
		for (int b = n, swap = 0; 0 != b; ) {
			swap = acc;
			acc = b;
			b = swap % b;
		}
		return Math.Abs(acc);
	}
	
	private static int EuclidIter(int a, int b)
	{
		return 0 == b ? Math.Abs(a) : EuclidIter(b, a % b);
	}
	private static int EuclidI(int m, int n)
	{
		return EuclidIter(m, n);
	}
	
	public static int GcdLp(int[] arr)
	{
		int acc = arr[0];
		for (int i = 1; arr.Length > i; ++i)
			acc = EuclidLp(acc, arr[i]);
		return acc;
	}
	
	private static int GcdIter(int acc, int idx, int[] arr)
	{
		return 0 > idx ? acc : GcdIter(EuclidI(acc, arr[idx]), idx - 1, arr);
	}
	public static int GcdI(int[] arr)
	{
		return 2 > arr.Length ? arr[0] : GcdIter(arr[0], arr.Length - 1, arr);
	}
	
	public static int LcmLp(int[] arr)
	{
		int acc = arr[0];
		for (int i = 1; arr.Length > i; ++i)
			acc = (acc * arr[i]) / EuclidLp(acc, arr[i]);
		return acc;
	}
	
	private static int LcmIter(int acc, int idx, int[] arr)
	{
		return 0 > idx ? acc : LcmIter(acc * arr[idx] / 
			EuclidI(acc, arr[idx]), idx - 1, arr);
	}
	public static int LcmI(int[] arr)
	{
		return 2 > arr.Length ? 1 : LcmIter(arr[0], arr.Length - 1, arr);
	}
	
	public static int[] BaseExpandLp(int b, int n)
	{
		int lenArr = 0;
		for (int i = n; 0 < i; i = i / b)
			lenArr += 1;
		int[] arr = new int[lenArr];
		for (int i = n, j = lenArr - 1; 0 < i; i = i / b, --j)
			arr[j] = i % b;
		return arr;
	}
	
	private static int[] BaseExpandIter(int b, int n, int idx, int[] arr)
	{
		if (0 <= idx) {
			arr[idx] = n % b;
			return BaseExpandIter(b, n / b, idx - 1, arr);
		}
		return arr;
	}
	public static int[] BaseExpandI(int b, int n)
	{
		int lenArr = 0;
		for (int i = n; 0 < i; i = i / b)
			lenArr += 1;
		int[] arr = new int[lenArr];
		return BaseExpandIter(b, n, arr.Length - 1, arr);
	}
	
	public static int BaseTo10Lp(int b, int[] arr)
	{
		int acc = 0;
		for (int i = arr.Length - 1, ct = 0; 0 <= i; --i, ++ct)
			acc += arr[i] * (int)Math.Pow(b, ct);
		return acc;
	}
	
	private static int BaseTo10Iter(int acc, int b, int idx, int[] arr)
	{
		return arr.Length <= idx ? acc : BaseTo10Iter(acc + arr[idx] * (int)Math.Pow(b, arr.Length - idx - 1), b, idx + 1, arr);
	}
	public static int BaseTo10I(int b, int[] arr)
	{
		return BaseTo10Iter(0, b, 0, arr);
	}
	
	public static int[] RangeStepLp(int step, int start, int stop)
	{
		int lenArr = 0;
		Func<int, int, bool> pred = (a, b) => step > 0 ? a > b : a < b;
		for (int i = start; pred(stop, i); i += step)
			lenArr += 1;
		int[] arr = new int[lenArr];
		for (int i = start, j = 0; pred(stop, i); i += step, ++j)
			arr[j] = i;
		return arr;
	}
	
	private static int[] RangeStepIter(int step, int start, int stop, int idx,
		int[] arr)
	{
		if (arr.Length > idx) {
			arr[idx] = start;
			return RangeStepIter(step, start + step, stop, idx + 1, arr);
		}
		return arr;
	}
	public static int[] RangeStepI(int step, int start, int stop)
	{
		int lenArr = 0;
		Func<int, int, bool> pred = (a, b) => step > 0 ? a > b : a < b;
		for (int i = start; pred(stop, i); i += step)
			lenArr += 1;
		int[] arr = new int[lenArr];
		return RangeStepIter(step, start, stop, 0, arr);
	}
	
	public static int[] RangeLp(int start, int stop)
	{
		return RangeStepLp(1, start, stop);
	}
	
	public static int[] RangeI(int start, int stop)
	{
		return RangeStepI(1, start, stop);
	}
	
	/// <summary>Main entry point.</summary>
    /// <param name="args">An array</param>
    /// <returns>The exit code.</returns>
	public static int Main(string[] args)
    {
		long n = 5L;
		Console.Write("Fact({0}) : {1}\n", n, FactI(n));
		return 0;
	}
}

}
