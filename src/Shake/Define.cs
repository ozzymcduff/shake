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
            var task = new RunLambdaTask(action);
            if (null != depends)
            {
                task.DependsOn.AddRange(depends);
            }
            _tasks.Add(name, task);
        }

        public void Task(string name, Func<Task> action, params string[] depends)
        {
            var task = new WrappedTask(action);
            if (null != depends)
            {
                task.DependsOn.AddRange(depends);
            }
            _tasks.Add(name, task);
        }
        public void Task(string name, Task task, params string[] depends)
        {
            if (null != depends)
            {
                task.DependsOn.AddRange(depends);
            }
            _tasks.Add(name, task);
        }
        public IEnumerable<Task> Tasks
        {
            get { return _tasks.Values; }
        }

        public Task TasksWithName(string name)
        {
            return _tasks[name];
        }

        public class RunLambdaTask : Task
        {
            private Action<Task> _action;
            public RunLambdaTask(Action<Task> action)
            {
                _action = action;
            }

            public override int Execute()
            {
                try
                {
                    _action(this);
                    return 0;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    return 1;
                }
            }
        }
        public class WrappedTask : Task
        {
            private Func<Task> action;

            public WrappedTask(Func<Task> action)
            {
                this.action = action;
            }

            public override int Execute()
            {
                return action().Execute();
            }
        }

        public int ExecuteTasksWithName(string name)
        {
            var task = _tasks[name];
            foreach (var dependingTask in task.DependsOn)
            {
                var retval = ExecuteTasksWithName(dependingTask);
                if (retval != 0) 
                {
                    return retval;
                }
            }
            return task.Execute();
        }
    }

}
