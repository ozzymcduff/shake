using Shake;
Define.It(d =>
{
    d.Task("Build", new MsBuild
        {
            Solution = @"C:\project\somesolution.sln",
        });
}).Execute(ScriptArgs);