using System;
using System.Linq;
using NUnit.Framework;

namespace Shake.It.Up
{
    [TestFixture]
    public class DefineWithTwoTasksWhereOneDependOnTheOtherTests
    {
        private Define definition;
        private bool checkExecuted = false;
        private bool buildExecuted = false;

        class FakeMsBuild:MsBuild
        {
            public override int Execute()
            {
                return 0;
            }
        }

        [SetUp]
        public void Two_tasks_where_one_depend_on_the_other()
        {
            definition = Define.It(d =>
            {
                d.Task("Check", t => checkExecuted=true);

                d.Task("Build", () => {
                    buildExecuted = true;
                    return new FakeMsBuild
                    {
                        Solution = @"C:\project\somesolution.sln",
                        MaxCpuCount = 2,
                        Properties = new { }
                    };
                }, "Check");
            });
        }

        [TearDown]
        public void Cleanup()
        {
            definition = null;
            checkExecuted = false;
            buildExecuted = false;
        }

        [Test]
        public void It_should_setup_definition()
        {
            Assert.That(definition, Is.Not.Null);
        }
        [Test]
        public void It_should_have_two_tasks()
        {
            Assert.That(definition.Tasks.Count(), Is.EqualTo(2));
        }
        [Test]
        public void Build_should_depend_on_check()
        {
            Assert.That(definition.TasksWithName("Build").DependsOn, 
                Is.EquivalentTo(new []{"Check"}));
        }
        [Test]
        public void When_executing_task_should_execute_the_task_that_it_depends_on() 
        {
            definition.ExecuteTasksWithName("Build");
            Assert.That(checkExecuted);
            Assert.That(buildExecuted);
        }
        [Test]
        public void When_executing_task_that_does_not_depend_on_another_task() 
        {
            definition.ExecuteTasksWithName("Check");
            Assert.That(checkExecuted);
            Assert.That(!buildExecuted);
        }
    }
}
