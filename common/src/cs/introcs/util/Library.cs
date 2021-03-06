namespace Introcs.Util {

using System;
//using SysCollGen = System.Collections.Generic;
using System.Collections.Generic;
using Text = System.Text;
using System.Linq;
using System.Reflection;

/// <summary>Library class.</summary>
public static class Library {
	/// <summary>Creates string representation of ICollection.</summary>
  /// <param name="coll">An ICollection.</param>
  /// <param name="beg">A string</param>
  /// <param name="sep">A string</param>
  /// <param name="stop">A string</param>
  /// <returns>The string representation.</returns>
  public static string MkString<T>(ICollection<T> coll, string beg = "[",
		string sep = ", ", string stop = "]")
  {
    if (null == coll)
		  return beg + String.Empty + stop;
    /*Text.StringBuilder strBldr = new Text.StringBuilder();

    foreach (T elem in coll)
			strBldr.Append(((0 < strBldr.Length) ? sep : String.Empty) + elem);
    return beg + strBldr.ToString() + stop;*/
    return beg + String.Join(sep, coll) + stop;
  }

  /// <summary>Get file contents from embedded resources.</summary>
  /// <param name="rsrcFileNm">A string.</param>
  /// <param name="assy">An assembly or null.</param>
  /// <param name="prefix">A string or null.</param>
  /// <returns>The file contents.</returns>
  public static string GetFromResources(string rsrcFileNm, Assembly assy = null, string prefix = null)
  {
	  Assembly assembly = null != assy ? assy : Assembly.GetExecutingAssembly();
		string pathPfx = null != prefix ? prefix :
			assembly.GetName().Name + ".resources";
		using (var strm = assembly.GetManifestResourceStream(rsrcFileNm) ??
			assembly.GetManifestResourceStream(pathPfx + "." + rsrcFileNm))
		{
			using (var reader = new System.IO.StreamReader(strm))
			{
				return reader.ReadToEnd();
			}
		}
	}

	/// <summary>Create string representation of ini config.</summary>
  /// <param name="cfg">An Ini config.</param>
  /// <returns>The string representation.</returns>
	public static string IniCfgToStr(IniParser.Model.IniData cfg)
	{
		var dictIni = new Dictionary<string, string>();
    if (null == cfg)
			return MkString(dictIni.ToArray(), beg: "{", stop: "}");

		/*// KeyFile.GKeyFile cfg
		foreach (string grp in cfg.GetGroups())
      foreach (string key in cfg.GetKeys(grp))
				try {
					dictIni.Add(grp + ":" + key, cfg.GetValue(grp, key));
				} catch (ArgumentException exc) {
					Console.Error.WriteLine("({0}) {1}", exc, exc.Message);
				}*/
		// IniParser.IniData cfg
		foreach (var sect in cfg.Sections)
      foreach (var key in cfg[sect.SectionName])
				try {
					dictIni[sect.SectionName + ":" + key.KeyName] =
						cfg[sect.SectionName][key.KeyName];
				} catch (ArgumentException exc) {
					Console.Error.WriteLine("({0}) {1}", exc, exc.Message);
				}
        return MkString(dictIni.ToArray(), beg: "{", stop: "}");
	}

	/// <summary>Compares equality within tolerance.</summary>
  /// <param name="a">A double</param>
  /// <param name="b">A double</param>
  /// <param name="tolerance">A double</param>
  /// <returns>The truth result of comparison.</returns>
  public static bool InEpsilon(double a, double b, double tolerance = 0.001)
  {
		double delta = Math.Abs(tolerance);
		//return (a - delta) <= b && (a + delta) >= b;
		return !((a + delta) < b) && !((b + delta) < a);
	}

	/// <summary>Creates Cartesian product of two arrays.</summary>
  /// <param name="arr1">An array</param>
  /// <param name="arr2">An array</param>
  /// <returns>The cartesian product.</returns>
	//public static Tuple<float, float>[] CartesianProdFloats(float[] arr1, float[] arr2) {
	//	Tuple<float, float>[] arrProd = new Tuple<float, float>[arr1.Count * arr2.Count];
	public static Tuple<T, T>[] CartesianProd<T>(T[] arr1, T[] arr2)
	{
		//System.Linq version
		/*return arr1.SelectMany(x => arr2, (x, y) => new Tuple<T, T>(x, y)).Where(
			res => true).Select(arrProd => arrProd).ToArray();*/
		return (
			from x in arr1
			from y in arr2
			where true
			select new Tuple<T, T>(x, y)).ToArray();
	}

	/// <summary>Main entry point.</summary>
  /// <param name="args">An array</param>
  /// <returns>The exit code.</returns>
	public static int Main(string[] args)
  {
		int[] arr1 = {0, 1, 2}, arr2 = {10, 20, 30};
		Console.Write("CartesianProd({0}, {1}) : {2}\n", MkString(arr1),
			MkString(arr2), MkString(CartesianProd(arr1, arr2)));
		return 0;
	}
}

}
