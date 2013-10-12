#load "runcommand.csx";
using System.Collections.Generic;
public class MsBuild{
  public string Solution{get;set;}
  public string Verbosity{get;set;}
  public string Loggermodule{get;set;}
  public string MaxCpuCount{get;set;}
  public string[] Targets{get;set;}
  public IDictionary<string,object> Properties{get;set;}
  public IDictionary<string,string> OtherSwitches{get;set;}
  private bool _nologo=false;
  public void Execute(){
     BuildSolution(Solution);
  }

  public void NoLogo(){
    _nologo = true;
  }
  
  public void BuildSolution(string solution){
    check_solution(Solution);
    
    var command_parameters = new List<string>();
    command_parameters.Add(String.Format("\"{0}\"",Solution)); 
    if (!String.IsNullOrEmpty(Verbosity)){
      command_parameters.Add(String.Format("\"/verbosity:{0}\"",Verbosity));
    }
    if (!String.IsNullOrEmpty(Loggermodule)){
      command_parameters.Add(String.Format("\"/logger:{0}\"",Loggermodule));
    }
    if (!String.IsNullOrEmpty(MaxCpuCount)){
      command_parameters.Add(String.Format("\"/maxcpucount:{0}\"",MaxCpuCount));
    }
    //command_parameters << "\"/nologo\"" if @nologo
    if (null!=Properties){
      command_parameters.Add(build_properties);
    }
    if (null!=OtherSwitches){
      command_parameters.Add(build_switches);
    }
    if (null!=Targets){
      command_parameters.Add(String.Format("\"/target:{0}\"",Targets));
    }

    //command_parameters <<  if @targets != nil
    var run = new RunCommand();
    run.FileName = "msbuild";
    run.Arguments = String.Join(" ",command_parameters);
    run.Execute();
    //result = run_command "MSBuild", command_parameters.join(" ")
    
    //failure_message = 'MSBuild Failed. See Build Log For Detail'
    //fail_with_message failure_message if !result
  }

  private void check_solution(string file){
    if (!String.IsNullOrEmpty(file)) return;
    throw new Exception("solution cannot be nil");
  }
  
  private string build_targets{
    get{ return String.Join(";",Targets); }
  }
  

  private string build_properties{
    get{
      var option_text = new List<string>();
      foreach (var kv in Properties){
        option_text.Add(String.Format("/p:{0}=\"{1}\"",kv.Key,kv.Value));
      }
      return String.Join(" ",option_text);
    }
  }
  
  private string build_switches{
    get{
      var switch_text = new List<string>();
      foreach (var kv in OtherSwitches){
        switch_text.Add(print_switch(kv.Key,kv.Value));
      }
      return String.Join(" ",switch_text);
    }
  }

  private string print_switch(string key, object value){
    return pure_switch(value) ? "/"+key : String.Format("/#{0}:\"{1}\"",key,value);
  }
    

  private bool pure_switch(object value){
    return (value is Boolean) && ((Boolean)value);
  }

}