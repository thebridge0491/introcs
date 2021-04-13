namespace Base {

using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;

public class MyArbitraries {
	public static FsCheck.Arbitrary<string> Strings()
	{
		//FsCheck.NonEmptyString
		return FsCheck.Arb.Default.String().Generator.Where(
			s => !string.IsNullOrEmpty(s)).ToArbitrary();
	}
	public static FsCheck.Arbitrary<double> Doubles()
	{
		//FsCheck.NormalFloat
		return FsCheck.Arb.Default.Float().Generator.Where(
			n => !Double.IsNaN(n) && !Double.IsInfinity(n)).ToArbitrary();
	}
	public static FsCheck.Arbitrary<double[]> DoubleArr()
	{
		//FsCheck.NonEmptyArray
		return FsCheck.Arb.Default.Array<double>().Generator.Where(
			xs => xs.Length != 0).ToArbitrary();
	}
	public static FsCheck.Arbitrary<IEnumerable<T>> Enumerables<T>()
	{
		return FsCheck.Arb.Default.Array<T>().Generator.Where(
			xs => xs.Length != 0).ToArbitrary().Convert(x => (IEnumerable<T>)x,
			x => (T[])x);
	}
}

}
