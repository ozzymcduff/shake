using System.Diagnostics;
using System.Dynamic;
using System.Collections.Generic;
public class RunCommand{
	public class ParameterDictionary: DynamicObject{
		private IDictionary<string,object> dic = 
			new Dictionary<string,object>();
		
	}
	public RunCommand(){
		Params = new ParameterDictionary();
	}
	public string FileName{get;set;}
	public string Arguments{get;set;}
	public ParameterDictionary Params{get; private set;} 
	public void Execute(){
		var startInfo = new ProcessStartInfo();
	    startInfo.FileName = FileName;
	    startInfo.CreateNoWindow = true;
	    startInfo.RedirectStandardOutput = true;
	    
	    startInfo.UseShellExecute = false;
	    startInfo.Arguments = Arguments;
	    try
	    {
	      using (Process exeProcess = Process.Start(startInfo))
	      {
	        Console.WriteLine(exeProcess.StandardOutput.ReadToEnd());
	        exeProcess.WaitForExit();
	      }
	    }
	    catch(Exception e)
	    {
	      Console.WriteLine(e.Message);
	      Environment.Exit(1);
	    }
	}
}