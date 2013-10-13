using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shake
{
    public abstract class Task
    {
        public Task()
        {
            DependsOn = new List<string>();
        }
        public abstract int Execute();
        public List<string> DependsOn { get; private set; }
    }
}
