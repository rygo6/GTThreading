GTThreading
=========

Currently Unity possesses these issues:
- Thread.Priority does not work on IL2CPP.
- Threading.Thread is entirely missing when you try and build Universal Windows 10 Store apps.

Due to this, when building an app targeting Android, iOS and Windows Store you actually cannot rely on System.Threading, it simply will not work for all platforms.

Thus, GTThreading

This is a higher level abstraction which will use System.Threading when it can, and the System.Threading.ThreadPool for Windows Store apps to replicate similar functionality to System.Threading.Thread.

Also utilized is a C method which will change the priority of a thread through low level Posix Thread functionality on iOS. This will be used automatically on iOS to get around the issue of Thread.Priority not working on IL2CPP.

Threads are created through this method:
```
GT.Threading.LoopThread.Create(Action method, string threadName, Priority priority, int cycleTimeMS);
```

All my current usages of threading in Unity rely creating game loops on an alternate thread. So thus far this library only contains LoopThread. Which will internally call a passed Action method on repeat, constrained to cycleTimeMS, which can be left out if you do not want time constraining. A string threadName must be provided so that the thread can be found through Posix methods on iOS.

Also implemented is a Wait() method with platform idiosyncratic implementation to pause a thread, as Thread.Sleep does not work on Windows Store Apps.

Usage is as follows:
```
LoopThread _thread;

void Start() {
  _thread = LoopThread.Create(RunMethod, "MyThread", Priority.Normal);
}

void RunMethod() {
  //Do Stuff
  _thread.Wait(10);
}
```

## Installation

The structure of this repo is set up to allow you to add it as a submodule directly into your Unity project repo. It should go in Assets/Plugins/ECThreading

So from the root of your project execute the following command.

    git submodule add git@github.com:rygo6/GTThreading.git Assets/Plugins/GTThreading

## Potentially Useful Future Features

- GTThreading abstraction which does not assume the thread will be looping.
- Functionality to call code from a Threaded method on MainThread.
- Make low level Posix thread priority C function work for Android.
