namespace Introcs.Foreignc {

using System;
using System.Runtime.InteropServices;

/// <summary>Classic class.</summary>
public static class Classic {
	[System.Security.SuppressUnmanagedCodeSecurityAttribute]
	private static class SafeNativeMethods
	{
		[DllImport("libintro_c-practice.so", EntryPoint="fact_i")]
		internal static extern long fact_i(int n);

		[DllImport("libintro_c-practice.so")]
		internal static extern long fact_lp(int n);

		[DllImport("libintro_c-practice.so")]
		internal static extern float expt_i(float b, float n);

		[DllImport("libintro_c-practice.so")]
		internal static extern float expt_lp(float b, float n);
	}

    /// <summary>Computes factorial (loop version).</summary>
    /// <param name="n">A long</param>
    /// <returns>The factorial of number.</returns>
	public static long FactLp(long n) {
		return SafeNativeMethods.fact_lp((int)n);
	}

    /// <summary>Computes factorial (iterative version).</summary>
    /// <param name="n">A long</param>
    /// <returns>The factorial of number.</returns>
	public static long FactI(long n) { 
		return SafeNativeMethods.fact_i((int)n);
	}

    /// <summary>Computes n-th exponent of base.(loop version).</summary>
    /// <param name="b">A float</param>
    /// <param name="n">A float</param>
    /// <returns>The n-th exponent of base.</returns>
	public static float ExptLp(float b, float n) {
		return SafeNativeMethods.expt_lp(b, n);
	}

    /// <summary>Computes n-th exponent of base.(iterative version).</summary>
    /// <param name="b">A float</param>
    /// <param name="n">A float</param>
    /// <returns>The n-th exponent of base.</returns>
	public static float ExptI(float b, float n) { 
		return SafeNativeMethods.expt_i(b, n);
	}
	
	/// <summary>Main entry point.</summary>
    /// <param name="args">An array</param>
    /// <returns>The exit code.</returns>
	public static int Main(string[] args)
    {
		long n = 5;
		Console.Write("Fact({0}) : {1}\n", n, FactI(n));
		return 0;
	}
}

}
