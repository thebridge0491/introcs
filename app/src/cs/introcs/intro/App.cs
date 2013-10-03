namespace Introcs.Intro {

using System;
using System.Runtime.InteropServices; // for StructLayout [FieldOffset(0)]
using System.Diagnostics;
using System.Reflection;
using IO = System.IO;
using SysCollGen = System.Collections.Generic;
using System.Linq;
using SysTextRegex = System.Text.RegularExpressions;
using NewtJson = Newtonsoft.Json;

using Util = Introcs.Util.Library;
using Misc = Introcs.Intro.Library;
using Classic = Introcs.Practice.Classic;
using Seqops = Introcs.Practice.Sequenceops;

struct OptsRecord {
    public string Name { get; set; }
    public int Num { get; set; }
    public bool IsExpt2 { get; set; }
    
    public override string ToString()
    {
		return String.Format("OptsRecord: [Name: {0}; Num: {1}; IsExpt2: {2}]",
			Name, Num, IsExpt2);
    }
}

static class Constants {
    public const float PI = 3.14f;
}
enum ConstItems : int {ZERO = 0, NUMZ = 26}
enum Kind : byte {FLOAT, DOUBLE, DECIMAL, INTSHORT, UINTLONG}

[StructLayout(LayoutKind.Explicit)]
struct UnionVal {
    // integral types: [s]byte, [u]short, [u]int, [u]long, char
    [FieldOffset(0)] public short sh;
    [FieldOffset(0)] public ulong ul;
    
    // floating point & decimal types: float, double, decimal
    [FieldOffset(0)] public float f;
    [FieldOffset(0)] public double d;
    [FieldOffset(0)] public decimal m;
    
    public override string ToString()
    {
		return string.Format(
			"UnionVal: [sh: {0:D}; ul: {1:D}; f: {2:F}; d: {3:F}; m: {4:F}]", 
			sh, ul, f, d, m);
    }
}

struct UVar {
    public byte kind { get; set; }
    public UnionVal val { get; set; }
    public override string ToString()
    {
    	return string.Format("UVar: [kind: {0}; val: {1}]", kind, val);
    }
}

/** @mainpage Description: 
 * <p>Brief comment.</p> */
/// <summary>App class.</summary>
public static class App {
	//private static readonly log4net.ILog log = 
	//	log4net.LogManager.GetLogger(
	//	System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
	private static readonly log4net.ILog log = 
		log4net.LogManager.GetLogger("root");
	
	static Type fromType = typeof(App);
	static Assembly assembly = Assembly.GetAssembly(fromType);
    static bool showHelp = false;
    static string progName = IO.Path.GetFileName(
		Environment.GetCommandLineArgs()[0]);
    
    static string GetFromResources(string rsrcFileNm, string prefix = null)
    {
		string pathPfx = null != prefix ? prefix : 
			fromType.Namespace + ".resources";
		using (var strm = assembly.GetManifestResourceStream(rsrcFileNm) ??
			assembly.GetManifestResourceStream(pathPfx + "." + rsrcFileNm))
		{
			using (var reader = new IO.StreamReader(strm))
			{
				return reader.ReadToEnd();
			}
		}
	}
    
    static void RunIntro(string name, int num, bool isExpt2,
		string rsrcPath = "resources")
    {
        DateTime time1 = DateTime.Now;
        Stopwatch stopwatch1 = new Stopwatch();
        stopwatch1.Start();
        
        // basic datatypes
        bool isDone = false;
        int numI = 0, arrLen = (int)ConstItems.ZERO, delayMsecs = 2500,
        	seedp = time1.Millisecond;
        float timeDiff = 0.0f;
        char ch = '\0';
        
        // strings & arrays
        string greetStr, dateStr, greetPath = "greet.txt";
        char[] str1 = new char[64];
        int[] numArr = {9, 9, 0x9, 9}; // {bin, oct, hex, dec}
        
        //composites
        UVar uVar1 = new UVar();
        //User user1 = new User() {Name = "World", Num = 0, TimeIn = time1};
        User user1 = new User("World", 0, time1);
        var tup1 = new Tuple<byte, short>((byte)Kind.INTSHORT, 100);
        Person pers; 
        
        Random rnd = new Random(seedp);
        
        IO.TextWriter fOut = Console.Out;
        TimeZone tz1 = TimeZone.CurrentTimeZone;
        string tzStr = tz1.IsDaylightSavingTime(time1) ? tz1.DaylightName 
        	: tz1.StandardName;
        
        arrLen = numArr.Length;
        
        for (int i = 0; arrLen > i; ++i) // foreach (int elem in numArr)
            numI += numArr[i];            //     numI += elem;
        Debug.Assert((numArr.Length * numArr[0]) == numI, 
			"arrLen * numArr[0] != numI");
        
        ch = Misc.DelayChar(delayMsecs);
        
        do {
            uVar1.kind = tup1.Item1;
            uVar1.val = new UnionVal(){sh = tup1.Item2};
            uVar1.kind = (byte)Kind.INTSHORT;
            uVar1.val = new UnionVal(){sh = -1};
            uVar1.kind = (byte)Kind.UINTLONG;
            uVar1.val = new UnionVal(){ul = 1UL};
            uVar1.kind = (byte)Kind.FLOAT;
            uVar1.val = new UnionVal(){f = 100.0f};
            uVar1.kind = (byte)Kind.DOUBLE;
            uVar1.val = new UnionVal(){d = 100.0d};
            uVar1.kind = (byte)Kind.DECIMAL;
            uVar1.val = new UnionVal(){m = 1000.0m};
            str1[0] = '\0';
            Debug.Assert(((byte)Kind.DECIMAL == uVar1.kind) && 
                    (1000.0m == uVar1.val.m), 
                    "kind == Kind.DECIMAL && val == 1000.0m is false");
        } while (isDone);
        
        user1.Name = name;
	    user1.Num = 0 == num ? rnd.Next(0, 17) + 2 : num;
	    
        SysTextRegex.Regex re = new SysTextRegex.Regex(@"(?i)^quit$");
        SysTextRegex.Match m = re.Match(name);
        Console.WriteLine("{0} match: {1} to {2}\n",
			m.Success ? "Good" : "Does not", name, re);
	    
	    dateStr = user1.TimeIn.ToString("ddd MMM dd HH:mm:ss yyyy zzz");
        
        //greetStr = Misc.Greeting(greetPath, user1.Name);
        try {
			//greetStr = (new IO.StreamReader(rsrcPath + "/" + greetPath)).ReadToEnd().TrimEnd('\n', '\r') + user1.Name;
			greetStr = IO.File.ReadAllText(rsrcPath + "/" + greetPath).TrimEnd('\n', '\r') + user1.Name;
		} catch (Exception exc0) {
			Console.Error.WriteLine("(exc: {0}) Bad env var RSRC_PATH: {1}\n",
				exc0, rsrcPath);
			try {
				greetStr = GetFromResources(greetPath).TrimEnd('\n', '\r') + user1.Name;
			} catch (Exception exc1) {
				throw;
				Environment.Exit(1);
			}
		}
        Console.WriteLine("{0} {1}\n{2}!", dateStr, tzStr, greetStr);
        
        stopwatch1.Stop();
        timeDiff = stopwatch1.ElapsedMilliseconds;
        Console.WriteLine("(program {0}) Took {1:F1} seconds.", progName, 
			timeDiff * 1.0e-3);
      	
      	int[] ints = {2, 1, 0, 4, 3};
      	var lst = new SysCollGen.List<int>(ints);
      	
      	if (isExpt2) {
		    fOut.WriteLine("Expt(2.0, {0}) = {1}", user1.Num, 
		        Classic.ExptLp(2.0f, user1.Num));
		    
		    var res = Util.MkString(lst);
		    Console.Write("Reverse({0}): ", res);
		    var lstTmp = Seqops.CopyOf<int>(lst);
		    Seqops.ReverseLp<int>(lstTmp);
		    res = Util.MkString(lstTmp);
		    Console.WriteLine("{0}", res);
		    
		    res = Util.MkString(lst);
		    Console.Write("{0}.Sort(): ", res);
		    lst.Sort();
		    res = Util.MkString(lst);
		    Console.WriteLine("{0}", res);
        } else {
		    fOut.WriteLine("Fact({0}) = {1}", user1.Num, 
		        Classic.FactLp(user1.Num));
		    
		    int el = 3;
		    var res = Util.MkString(lst);
		    int idx = Seqops.IndexOfLp<int>(el, lst);
		    Console.WriteLine("IndexOf({0}, {1}): {2}", el, res, idx);
		    
		    int newVal = 50;
		    Console.Write("{0}.Add({1}): ", res, newVal);
		    lst.Add(newVal);
		    res = Util.MkString(lst);
		    Console.WriteLine("{0}", res);
        }
        Console.WriteLine(new string('-', 40));
        
        //pers = new Person("John", 32);
        pers = new Person {Name = "I.M. Computer", Age = 32};
        
        Debug.Assert(pers.GetType() == typeof(Person), 
			"Debug Error: Type mismatch");
        Debug.Assert(pers is Object, 
			"Trace Error: Type inheritance mismatch");
        Console.WriteLine("{0}", pers.ToString());
        Console.Write("pers.Age = {0}: ", 33);
        pers.Age = 33;
        Console.WriteLine("{0}", pers.ToString());
        Console.WriteLine(new string('-', 40));
    }
    
    static void ParseCmdopts(string[] args, Mono.Options.OptionSet options)
    {
        SysCollGen.List<string> extra = new SysCollGen.List<string>();
        log.Info("parseCmdopts()");
        try {
            extra = options.Parse(args);
        } catch (Mono.Options.OptionException exc) {
            Console.Error.WriteLine("{0}: {1}", progName, exc.Message);
            Environment.Exit(1);
        }
        if (0 < extra.Count)
            Console.Error.WriteLine("Extra args: {0}", extra.Count);
        if (showHelp) {
		    Console.WriteLine("Usage: {0} [options]\n\nOptions:", progName);
	    	options.WriteOptionDescriptions(Console.Out);
		    Environment.Exit(1);
	    }
    }
    
    struct YamlConfig
    {
		public string hostname { get; set; }
		public string domain { get; set; }
		public SysCollGen.Dictionary<string, object> file1 { get; set; }
		public SysCollGen.Dictionary<string, object> user1 { get; set; }
	}
    
    /** DocComment: 
     * <p>Brief description.</p>
     * @param args - array of command-line arguments */
	/// <summary>Main entry point.</summary>
    /// <param name="args">An array</param>
    /// <returns>The exit code.</returns>
    static int Main(string[] args)
    {
        OptsRecord opts = new OptsRecord() {Name = "World", Num = 0,
        	IsExpt2 = false};
        var options = new Mono.Options.OptionSet() {
            {"u|user=", "user name", (string v) => opts.Name = v},
            {"n|num=", "number", (int v) => opts.Num = v},
            {"2|expt2", "expt 2 n", v => opts.IsExpt2 = true},
            { "h|help",  "show this message", v => showHelp = true },
            //v != null },
        };

        IO.Stream traceOut = IO.File.Create("trace.log");
        TraceListener[] lstnrs = {new ConsoleTraceListener(true),
        	new TextWriterTraceListener(traceOut)};
        foreach (var lstnr in lstnrs)
        	Debug.Listeners.Add(lstnr);	// /define:[TRACE|DEBUG]
        
        ParseCmdopts(args, options);
        
        string envRsrcPath = Environment.GetEnvironmentVariable("RSRC_PATH");
        string rsrcPath = null != envRsrcPath ? envRsrcPath : "resources";
        
        string iniStr = String.Empty, jsonStr = String.Empty,
			yamlStr = String.Empty;
        try {
			//iniStr = (new IO.StreamReader(rsrcPath + "/prac.conf")).ReadToEnd();
			iniStr = IO.File.ReadAllText(rsrcPath + "/prac.conf");
			//jsonStr = IO.File.ReadAllText(rsrcPath + "/prac.json");
			//yamlStr = IO.File.ReadAllText(rsrcPath + "/prac.yaml");
		} catch (Exception exc0) {
			Console.Error.WriteLine("(exc: {0}) Bad env var RSRC_PATH: {1}\n",
				exc0, rsrcPath);
			try {
				iniStr = GetFromResources("prac.conf");
				//jsonStr = GetFromResources("prac.json",
				//	fromType.Namespace + ".resources");
				//yamlStr = GetFromResources("prac.yaml");
			} catch (Exception exc1) {
				throw;
				Environment.Exit(1);
			}
		}
        
        //var cfgIni = new KeyFile.GKeyFile();
        //cfgIni.LoadFromData(iniStr);
        var cfgIni = new IniParser.StringIniParser().ParseString(iniStr);
        
        //var defn1 = new {hostname = String.Empty, domain = String.Empty, 
		//	file1 = new {path = String.Empty, ext = String.Empty}, 
		//	user1 = new {name = String.Empty, age = 0}};
		//var anonType = NewtJson.JsonConvert.DeserializeAnonymousType(
		//	jsonStr, defn1);
        //var dictRootJson = NewtJson.JsonConvert.DeserializeObject<
		//	SysCollGen.Dictionary<string, object>>(jsonStr);
        //var dictUserJson = NewtJson.JsonConvert.DeserializeObject<
		//	SysCollGen.Dictionary<string, object>>(
		//	String.Format("{0}", dictRootJson["user1"]));
        
		//var deserializer = new YamlDotNet.Serialization.Deserializer();
		//var objRootYaml = deserializer.Deserialize<YamlConfig>(
		//	new IO.StringReader(yamlStr));
        
        Tuple<string, string, string>[] arrTups = {
			//Tuple.Create(Util.IniCfgToStr(cfgIni), 
			//	cfgIni.GetValue("default", "domain"),
			//	cfgIni.GetValue("user1", "name")),
			Tuple.Create(Util.IniCfgToStr(cfgIni), 
				cfgIni["default"]["domain"],
				cfgIni["user1"]["name"]) //,
			//Tuple.Create(anonType.ToString(), 
			//	String.Format("{0}", anonType.domain),
			//	String.Format("{0}", anonType.user1.name)),
			//Tuple.Create(Util.MkString(dictRootJson.ToArray(), beg: "{", 
			//	stop: "}"),
			//	String.Format("{0}", dictRootJson["domain"]), 
			//	String.Format("{0}", dictUserJson["name"])),
			//Tuple.Create(yamlStr,
			//	String.Format("{0}", objRootYaml.domain), 
			//	String.Format("{0}", objRootYaml.user1["name"]))
		};
		foreach (Tuple<string, string, string> tup in arrTups) {
			Console.WriteLine("config: {0}", tup.Item1);
			Console.WriteLine("domain: {0}", tup.Item2);
			Console.WriteLine("user1Name: {0}\n", tup.Item3);
		}
        
        RunIntro(opts.Name, opts.Num, opts.IsExpt2, rsrcPath);
        
        //Trace.Fail("Trace example");
        Trace.Flush(); //Debug.Flush();
        traceOut.Close();
        return 0;
    }
}

}
