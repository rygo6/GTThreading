UniThread
=========

Currently Unity possesses these issues:
- Thread.Priority does not work on IL2CPP.
- Threading.Thread is entirely missing when you try and build Universal Windows 10 Store apps.
- The way you cancel threads can also have issues depending on platforms.

Due to this, when building an app targeting Android, iOS and Windows Store you actually cannot rely on System.Threading, it simply will not work for all platforms.

Thus, UniThread.

This is a higher level abstraction which will use System.Threading when it can, and the System.Threading.ThreadPool for Windows Store apps to replicate similar functionality to System.threading.

Also utilized is a C method which will change the priority of a thread through low level Posix Thread functionality on iOS. This will be used automatically on iOS rather to get around the issue of Thread.Priority not working on IL2CPP.

Usage is as follows:
```
EC.UniThread.LoopThread.Create(MethodDelegate, UniThreadPriority, CycleTimeMS);
```

All my current usages of threading in Unity rely creating game loops on an alternate thread. So thus far this library only contains LoopThread. Which will internally call a passed MethodDelegate on repeat, constrained to CycleTimeMS which is an int in Milliseconds.

Also implemented is a Wait() method with platform idiosyncratic implementation to pause a thread, as Thread.Sleep does not work on Windows Store Apps.

Usage is as follows:
```
LoopThread _thread;

void Start() {
  _thread = LoopThread.Create(RunMethod, UniThreadPriority.Normal);
}

void RunMethod() {
  //Do Stuff
  _thread.Wait(10);
}
```

# Potentially Useful Future Features

- UniThread abstraction which does not assume the thread will be looping.
- Functionality to call code from a Threaded method on MainThread.

Please do consider forking my repo, and adding to it any solutions to Unity idiosyncratic System.Threading issues then submitting a pull request. Such a common, base level functionality I do not believe belongs on the Asset Store with a price.

License under the MIT License
