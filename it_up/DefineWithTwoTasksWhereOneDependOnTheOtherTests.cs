using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Shake.It.Up
{
    [TestFixture]
    public class DefineWithTwoTasksWhereOneDependOnTheOtherTests
    {
        private Define definition;

        [SetUp]
        public void Two_tasks_where_one_depend_on_the_other()
        {
            definition = Define.It(d =>
            {
                d.Task("Check", t => Console.WriteLine("Check"));

                d.Task("Build", t => new MsBuild
                {
                    Solution = @"C:\project\somesolution.sln",
                    MaxCpuCount = 2,
                    Properties = new { }
                },
                    "Check");
                /*
                d.Task
                    .Check()
                d.Task
                    .Depends(t=>t.Check())
                    .Build()
                    ;
                 */
            });
        }

        [Test]
        public void It_should_setup_definition()
        {
            Assert.That(definition, Is.Not.Null);
        }
        [Test]
        public void It_should_have_two_tasks()
        {
            Assert.That(definition.Tasks, Is.EqualTo(2));
        }
    }
}
