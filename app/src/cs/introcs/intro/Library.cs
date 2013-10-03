namespace Introcs.Intro {

using System;
using IO = System.IO;
using Threading = System.Threading;

/// <summary>Library class.</summary>
public static class Library {
	private static readonly log4net.ILog log = 
		log4net.LogManager.GetLogger("prac");
	
	public static string Greeting(string greetPath, string name)
	{
		string buf = "";
		log.Info("Greeting()");
		
        using (IO.StreamReader istr = new IO.StreamReader(greetPath)) {
            if (-1 < istr.Peek())
                buf = istr.ReadLine();
        } // auto istr.Close();
        return buf + name;
	}
	
	public static char DelayChar(int delayMsecs)
	{
        char ch = '\0';
        
        while (true) {
            Threading.Thread.Sleep(delayMsecs);
            Console.WriteLine("Type any character when ready.");
            
            string input = Console.ReadLine();
            
            try {
                ch = Convert.ToChar(input[0]);
            } catch (IndexOutOfRangeException exc) {
				continue;
			} catch (Exception exc) {
                Console.Error.WriteLine("({0}) {1}", exc, exc.Message);
                Environment.Exit(1);
            }
            if ('\n' == ch || '\0' == ch)
                continue;
            else
                break;
        }
		return ch;
	}
	
	/// <summary>Main entry point.</summary>
    /// <param name="args">An array</param>
    /// <returns>The exit code.</returns>
	public static int Main(string[] args)
    {
		int mSecs = 2500;
		Console.Write("DelayChar({0}) : {1}\n", mSecs, DelayChar(mSecs));
		return 0;
	}
}

}
