namespace Base {

using FsCheck.NUnit.Addin;
using NUnit.Core.Extensibility;

[NUnitAddin(Description = "FsCheck addin")]
public class NunitAddinFsCheck : IAddin {
	public bool Install(IExtensionHost host)
	{
		var tcBuilder = new FsCheckTestCaseBuider();
		host.GetExtensionPoint("TestCaseBuilders").Install(tcBuilder);
		return true;
	}
}

}
