using System;
using System.Collections.Generic;
using Shake.Infrastructure;

namespace Shake
{
    public class MsBuild : Task
    {
        public string Solution { get; set; }
        public string Verbosity { get; set; }
        public string Loggermodule { get; set; }
        public int? MaxCpuCount { get; set; }
        public string[] Targets { get; set; }
        private IDictionary<string, object> _otherSwitches;
        public object OtherSwitches
        {
            get { throw new NotImplementedException("!"); }
            set { _otherSwitches = ReflectionHelper.ObjectToDictionary(value); }
        }
        private bool _nologo = false;
        private IDictionary<string, object> _properties;
        public object Properties
        {
            get { throw new NotImplementedException("!"); }
            set { _properties = ReflectionHelper.ObjectToDictionary(value); }
        }
        public bool NoLogo
        {
            get { return _nologo; }
            set { _nologo = value; }
        }
        public override int Execute()
        {
            return BuildSolution(Solution);
        }

        public int BuildSolution(string solution)
        {
            CheckSolution(Solution);

            var commandParameters = new List<string>();
            commandParameters.Add(String.Format("\"{0}\"", Solution));

            if (null != _properties)
            {
                commandParameters.Add(GetBuildProperties());
            }

            var run = new RunCommand();
            if (_nologo)
            {
                run.Params.NoLogo = true;
            }
            if (null != _otherSwitches)
            {
                run.Params.AddRange(_otherSwitches);
            }
            if (!String.IsNullOrEmpty(Verbosity))
            {
                run.Params.Verbosity = Verbosity;
            }
            if (!String.IsNullOrEmpty(Loggermodule))
            {
                run.Params.Loggermodule = Loggermodule;
            }
            if (MaxCpuCount.HasValue)
            {
                run.Params.MaxCpuCount = MaxCpuCount;
            }
            if (null != Targets)
            {
                run.Params.Targets = String.Join(";", Targets);
            }
            run.FileName = "msbuild";
            run.Arguments = String.Join(" ", commandParameters);
            return (run.Execute());
            //result = run_command "MSBuild", commandParameters.join(" ")

            //failure_message = 'MSBuild Failed. See Build Log For Detail'
            //fail_with_message failure_message if !result
        }

        private static void CheckSolution(string file)
        {
            if (!String.IsNullOrEmpty(file)) return;
            throw new Exception("solution cannot be nil");
        }

        private string GetBuildProperties()
        {
            var optionText = new List<string>();
            foreach (var kv in _properties)
            {
                optionText.Add(String.Format("/p:{0}=\"{1}\"", kv.Key, kv.Value));
            }
            return String.Join(" ", optionText);
        }

    }
}
