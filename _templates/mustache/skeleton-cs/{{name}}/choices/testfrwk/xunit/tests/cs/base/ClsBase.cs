namespace Base {

using System;
using System.IO;
using Xunit;

public class ClsFixture : IDisposable {
    protected StreamWriter output;
	
	public ClsFixture()
	{
		output = File.AppendText("testout.txt");
		output.WriteLine("Base SetUpClass({0})", GetType().BaseType);
	}
	public virtual void Dispose()
    {
        output.WriteLine("Base TearDownClass({0})", GetType().BaseType);
        output.WriteLine(new string('-', 40));
        output.Close();
    }
}

public abstract class ClsBase : ClsFixture {
    protected ClsBase()
    {
        ;
    }
    public virtual void SetUp()
    {
        output.WriteLine("Base SetUp");
    }
    public virtual void TearDown()
    {
        output.WriteLine("Base TearDown");
    }
    public override void Dispose()
    {
        output.WriteLine("Base Dispose({0})", GetType().BaseType);
        base.Dispose();
    }
}

}
