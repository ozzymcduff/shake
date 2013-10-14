using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Shake.It.Up
{
    [TestFixture]
    public class DefineWithThreeTasksWhereTwoDependsOnOne
    {
        private Define definition;
        private int build1Executed = 0;
        private int build2Executed = 0;
        private int build3Executed = 0;

        [SetUp]
        public void Three_tasks_where_two_depend_on_one()
        {
            definition = Define.It(d =>
            {
                d.Task("build1", t => build1Executed++, "build2", "build3");
                d.Task("build2", t => build2Executed++, "build3");
                d.Task("build3", t => build3Executed++);
            });
            definition.ExecuteTasksWithName("build1");
        }

        [TearDown]
        public void Cleanup()
        {
            definition = null;
            build1Executed = 0;
            build2Executed = 0;
            build3Executed = 0;
        }

        [Test]
        public void Should_not_execute_build3_more_than_one_when_building_build1() 
        {
            Assert.That(build3Executed, Is.EqualTo(1));
        }
        [Test]
        public void Should_execute_build1_once()
        {
            Assert.That(build1Executed, Is.EqualTo(1));
        }
        [Test]
        public void Should_execute_build2_once()
        {
            Assert.That(build2Executed, Is.EqualTo(1));
        }
    }
}
