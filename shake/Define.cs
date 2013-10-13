using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Shake
{
    public class Define
    {
        private Dictionary<string, Task> _tasks;

        public Define()
        {
            _tasks = new Dictionary<string, Task>(StringComparer.InvariantCultureIgnoreCase);
        }

        public static Define It (Action<Define> define)
        {
            var d = new Define();
            define(d);
            return d;
        }

        public void Task(string name, Action<Task> action, params string[] depends)
        {
            _tasks.Add(name,new RunLambdaTask(action));
        }

        public void Task(string name, Func<Task> action, params string[] depends)
        {
            _tasks.Add(name, action());//This is wrong should not execute yet
        }

        public IEnumerable<Task> Tasks
        {
            get { return _tasks.Values; }
        }

        public Task TasksWithName(string name)
        {
            return _tasks[name];
        }
    }

    public class RunLambdaTask : Task
    {
        public RunLambdaTask(Action<Task> action)
        {
            throw new NotImplementedException();
        }

        public override int Execute()
        {
            throw new NotImplementedException();
        }
    }
}
