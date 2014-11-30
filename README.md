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

## About

I wrote this as a joke about build scripts in c# (since at that time there wasn't any that we knew of).

No one else has done any pull requests to this project, so it's probably better to use some alternative.
    
## Alternatives

- rake with [albacore](https://github.com/Albacore/albacore)
- [Nake](https://github.com/yevhen/Nake) (I've not tried it)
- [Cake](https://github.com/cake-build/cake)
- grunt
