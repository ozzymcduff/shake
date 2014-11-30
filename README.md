# Shake

Shake is c# make.

## Features

Right now you can execute msbuild tasks and use runcommand to run executables.

## Example 
Use

    scriptcs --install Shake

Then you can write your first shakefile.csx :

    using Shake;
    Define.It(d =>
    {
      d.Task("Build", new MsBuild
        {
            Solution = @"C:\project\somesolution.sln",
        });
    }).Execute(ScriptArgs);
    
To execute this you need to use a bit of convoluted syntax, but hey it works on my machine ;)

    scriptcs .\shakefile.csx -- build
    
## Alternatives

rake with albacore

Nake:
https://github.com/yevhen/Nake
Cake:
https://github.com/cake-build/cake
(I've not tried any of those yet)
