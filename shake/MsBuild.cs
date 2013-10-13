using System;
using System.Collections.Generic;

namespace Shake
{
    public class MsBuild
    {
        public string Solution { get; set; }
        public string Verbosity { get; set; }
        public string Loggermodule { get; set; }
        public string MaxCpuCount { get; set; }
        public string[] Targets { get; set; }
        public IDictionary<string, object> Properties { get; set; }
        public IDictionary<string, string> OtherSwitches { get; set; }
        private bool _nologo = false;
        public void Execute()
        {
            BuildSolution(Solution);
        }

        public void NoLogo()
        {
            _nologo = true;
        }

        public void BuildSolution(string solution)
        {
            check_solution(Solution);

            var commandParameters = new List<string>();
            commandParameters.Add(String.Format("\"{0}\"", Solution));

            if (null != Properties)
            {
                commandParameters.Add(GetBuildProperties());
            }

            var run = new RunCommand();
            if (_nologo)
            {
                run.Params.NoLogo = true;
            }
            if (null != OtherSwitches)
            {
                run.Params.AddRange(OtherSwitches);
            }
            if (!String.IsNullOrEmpty(Verbosity))
            {
                run.Params.Verbosity = Verbosity;
            }
            if (!String.IsNullOrEmpty(Loggermodule))
            {
                run.Params.Loggermodule = Loggermodule;
            }
            if (!String.IsNullOrEmpty(MaxCpuCount))
            {
                run.Params.MaxCpuCount = MaxCpuCount;
            }
            if (null != Targets)
            {
                run.Params.Targets = String.Join(";", Targets);
            }
            run.FileName = "msbuild";
            run.Arguments = String.Join(" ", commandParameters);
            Environment.Exit(run.Execute());
            //result = run_command "MSBuild", commandParameters.join(" ")

            //failure_message = 'MSBuild Failed. See Build Log For Detail'
            //fail_with_message failure_message if !result
        }

        private void check_solution(string file)
        {
            if (!String.IsNullOrEmpty(file)) return;
            throw new Exception("solution cannot be nil");
        }

        private string GetBuildProperties()
        {
            var optionText = new List<string>();
            foreach (var kv in Properties)
            {
                optionText.Add(String.Format("/p:{0}=\"{1}\"", kv.Key, kv.Value));
            }
            return String.Join(" ", optionText);
        }

    }
}
