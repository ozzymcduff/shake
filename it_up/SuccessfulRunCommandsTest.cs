using System;
using System.IO;
using NUnit.Framework;

namespace Shake.It.Up
{
    [TestFixture]
    public class SuccessfulRunCommandsTest
    {
        private StringWriter _out;
        private StringWriter _error;
        private RunCommand _runCommand;
        private int _exitstatus;
        [SetUp]
        public void Writeline()
        {
            _out = new StringWriter();
            _error = new StringWriter();
            _runCommand = new RunCommand() { FileName = "writeline.exe",Params=new{Test="value"}, Out = _out, Error = _error };
            _exitstatus = _runCommand.Execute();
            Console.WriteLine(_out.ToString());
            Console.WriteLine(_error.ToString());
        }

        [Test]
        public void It_should_have_a_zero_exit_status()
        {
            Assert.That(_exitstatus, Is.EqualTo(0));
        }

        [Test]
        public void Out_should_contain_written_line()
        {
            Assert.That(_out.ToString(), Is.StringContaining("test"));
            Assert.That(_out.ToString(), Is.StringContaining("value"));
        }
    }
}
