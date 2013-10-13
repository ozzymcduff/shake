using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Dynamic;

namespace Shake
{
    public class RunCommand:Task
    {
        public class ParameterDictionary : DynamicObject
        {
            private IDictionary<string, object> dic =
                new Dictionary<string, object>();
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
            Params = new ParameterDictionary();
            Arguments = string.Empty;
            _renderParams = renderParams;
        }
        private readonly RenderParams _renderParams;
        public string FileName { get; set; }
        public string Arguments { get; set; }
        public dynamic Params { get; private set; }
        public override int Execute()
        {
            var startInfo = new ProcessStartInfo
                                {
                                    FileName = FileName,
                                    CreateNoWindow = true,
                                    RedirectStandardOutput = true,
                                    UseShellExecute = false,
                                    Arguments = Arguments + " " + DoRenderParams()
                                };
            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    Console.WriteLine(exeProcess.StandardOutput.ReadToEnd());
                    exeProcess.WaitForExit();
                    return exeProcess.ExitCode;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return 1;
            }
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
