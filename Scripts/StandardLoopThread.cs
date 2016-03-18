using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EC.Threading {
public sealed class StandardLoopThread : LoopThread {

#if !UNITY_WSA_10_0
	Thread _thread;
#endif

	public StandardLoopThread(Action method, string threadName, Priority priority, int cycleTimeMS = 0) :
		base(method, threadName, priority, cycleTimeMS) {
		UnityEngine.Debug.Log("StandardLoopThread Created " + method.ToString() + " " + threadName + " " + priority + " " + cycleTimeMS);
	}

	public override void Start() {
#if !UNITY_WSA_10_0
		_thread = new Thread(RunThreadLoop);
		_thread.Name = ThreadName;
		if (Priority == Priority.Low) 
			_thread.Priority = System.Threading.ThreadPriority.Lowest;
		else if (Priority == Priority.High)
			_thread.Priority = System.Threading.ThreadPriority.Highest;
		_thread.Start();
#endif
#if UNITY_IOS && !UNITY_EDITOR
		SetIOSThreadPriority(_thread.Name, (int)Priority);
#endif
	}

	public override void Stop() {
		RunThread = false;
#if !UNITY_WSA_10_0
		while (_thread.IsAlive) {
		}
#endif
	}

	public override void Wait(int ms) {
#if !UNITY_WSA_10_0
		Thread.Sleep(ms);
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	static extern void SetIOSThreadPriority(string threadName, int priority);
#endif
}
}