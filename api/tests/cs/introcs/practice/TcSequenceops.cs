namespace Introcs.Practice.Tests {

using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.FSharp.Core;

using Util = Introcs.Util.Library;
using Introcs.Practice;

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
    
	[Test] [Category("Tag1")]
	public void ReverseTest()
	{
	    var funcsMut = new Action<List<int>>[] {Sequenceops.ReverseLp};
	    var funcsImm = new Func<IEnumerable<int>, IEnumerable<int>>[] {
			SequenceopsFs.revSeqR, SequenceopsFs.revSeqI};
	    int[] ints = {0, 1, 2, 3, 4};
	    var lst = new List<int>(); //new List<int>(ints);
	    //foreach (var e in ints)
	    //	lst.Add(e);
	    lst.AddRange(ints);
	    
	    foreach (var f in funcsMut) {
	    	var tmp = Sequenceops.CopyOf<int>(lst);
	    	f(tmp);
			
			for (int i = 0, j = ints.Length - 1; ints.Length > i; ++i, --j)
				Assert.AreEqual(lst[i], tmp[j], "ReverseTest mut");
		}
	    foreach (var f in funcsImm)
	    	Assert.True(lst.AsEnumerable().Reverse().SequenceEqual(f(lst)),
				"ReverseTest imm");
	}
	
	[Test] [Category("Tag1")]
	public void IndexOfTest()
	{
	    int el = 3;
	    FSharpFunc<int, bool> predFs = (Converter<int, bool>)(e => el == e);
	    var funcsIdx = new Func<int, List<int>, int>[] {Sequenceops.IndexOfLp,
			(el1, xs) => SequenceopsFs.findIndexSeqI(predFs, xs)};
	    int[] ints = {0, 1, 2, 3, 4};
	    List<int>[] lsts = {new List<int>(), new List<int>()};
	    foreach (var e in ints) {
	    	lsts[0].Add(e);
	    	lsts[1].Insert(0, e);
	    }
	    
	    foreach (var lst in lsts) {
			foreach (var f in funcsIdx)
				Assert.AreEqual(lst.IndexOf(el), f(el, lst), "IndexOfTest");
	    }
	}
}

}
