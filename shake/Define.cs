using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Shake
{
    public class Define
    {
        public static Define It (Action<Define> define)
        {
            throw new NotImplementedException();
        }

        public void Task(string name, Action<Task> action, params string[] depends)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> Tasks
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
