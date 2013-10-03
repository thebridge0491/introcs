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
using Introcs.Practice;

[TestFixture]
public class TpSequenceops : Base.ClsBase {
    private double epsilon = 0.001; //1.20e-7;
    /*
    public override void Dispose()
    {
        //Console.Error.WriteLine("Derived Dispose({0})", GetType());
        //base.Dispose();
    }*/
    
    [Property] [Category("Tag3")]
	public bool ReverseProp(List<int> xs)
	{
		var funcsMut = new Action<List<int>>[] {Sequenceops.ReverseLp};
		var funcsImm = new Func<IEnumerable<int>, IEnumerable<int>>[] {
			SequenceopsFs.revSeqR, SequenceopsFs.revSeqI};
		List<int> ys = xs.ToList();
		ys.Reverse();
		
		return funcsMut.Aggregate(true, (acc, f) => {
			var tmp = Sequenceops.CopyOf<int>(xs);
			f(tmp);
			return acc && tmp.SequenceEqual(ys);
		}) && funcsImm.Aggregate(true, (acc, f) => 
			acc && f(xs).SequenceEqual(ys));
	}
	
    [Property] [Category("Tag3")]
	public bool IndexOfProp(int el, List<int> xs)
	{
		FSharpFunc<int, bool> predFs = (Converter<int, bool>)(e => el == e);
		var funcsIdx = new Func<int, List<int>, int>[] {Sequenceops.IndexOfLp,
			(el1, ys) => SequenceopsFs.findIndexSeqI(predFs, ys)};
		var ansIdx = xs.IndexOf(el);
		
		return funcsIdx.Aggregate(true, (acc, f) =>
			acc && ansIdx == f(el, xs));
	}
}

}
