using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using Shake.Infrastructure;

namespace Shake
{
    public class RunCommand:Task
    {
        public class ParameterDictionary : DynamicObject
        {
            private IDictionary<string, object> dic =
                new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                dic[binder.Name] = value;
                return true;
            }
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (dic.ContainsKey(binder.Name))
                {
                    result = dic[binder.Name];
                    return true;
                }

                result = null;
                return false;
            }

            public string RenderAsSlash()
            {
                return String.Join(" ", dic.Select(RenderSlash));
            }
            private string RenderSlash(KeyValuePair<string, object> kv)
            {
                var key = kv.Key.ToLower();

                return PureSwitch(kv.Value)
                    ? "/" + key
                    : String.Format("\"/{0}:{1}\"", key, kv.Value);
            }
            private bool PureSwitch(object value)
            {
                return (value is Boolean) && ((Boolean)value);
            }
            public void AddRange(IEnumerable<KeyValuePair<string, object>> kvs)
            {
                foreach (var kv in kvs)
                {
                    dic.Add(kv.Key, kv.Value);
                }
            }
        }
        public enum RenderParams
        {
            AsSlash = 0
        }

        public RunCommand(RenderParams renderParams = RenderParams.AsSlash)
        {
            _params = new ParameterDictionary();
            Arguments = string.Empty;
            _renderParams = renderParams;
            Out = Console.Out;
            Error = Console.Error;
        }
        private ParameterDictionary _params;
        private readonly RenderParams _renderParams;
        public string FileName { get; set; }
        public string Arguments { get; set; }
        public dynamic Params 
        { 
            get { return _params; }
            set { _params.AddRange(ReflectionHelper.ObjectToDictionary(value)); } 
        }
        public TextWriter Out { get; set; }
        public TextWriter Error { get; set; }
        public override int Execute()
        {
            var startInfo = new ProcessStartInfo
                                {
                                    FileName = FileName,
                                    CreateNoWindow = true,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    UseShellExecute = false,
                                    Arguments = Arguments + " " + DoRenderParams(),
                                };
            try
            {
                using (Process exeProcess = new Process())
                {
                    exeProcess.StartInfo = startInfo;
                    exeProcess.ErrorDataReceived += ErrorDataReceived;
                    exeProcess.OutputDataReceived += OutputDataReceived;
                    exeProcess.EnableRaisingEvents = true;
                    exeProcess.Start();
                    exeProcess.BeginOutputReadLine();
                    exeProcess.BeginErrorReadLine();
                    exeProcess.WaitForExit();
                    return exeProcess.ExitCode;
                }
            }
            catch (Exception e)
            {
                Error.WriteLine(e.Message);
                return 1;
            }
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Out.WriteLine(e.Data);
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Error.WriteLine(e.Data);
        }

        private string DoRenderParams()
        {
            switch (_renderParams)
            {
                case RenderParams.AsSlash:
                    return Params.RenderAsSlash();
                default: throw new Exception("Not implemented");
            }
        }
    }
}
