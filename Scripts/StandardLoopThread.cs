using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EC.UniThread {
public sealed class StandardLoopThread : LoopThread {

	Thread _thread;

	public StandardLoopThread(Action method, UniThreadPriority priority, int cycleTimeMS = 0) :
		base(method, priority, cycleTimeMS) {
	}

	public override void Start() {
		_thread = new Thread(RunThreadLoop);
		_thread.Name = Method.ToString();
		if (Priority == UniThreadPriority.Low) 
			_thread.Priority = System.Threading.ThreadPriority.Lowest;
		else if (Priority == UniThreadPriority.High)
			_thread.Priority = System.Threading.ThreadPriority.Highest;
		_thread.Start();
#if UNITY_IOS && !UNITY_EDITOR
		SetIOSThreadPriority(_thread.Name, (int)Priority);
#endif
	}

	public override void Stop() {
		RunThread = false;
		while (_thread.IsAlive) {
		}
	}

	public override void Wait(int ms) {
		Thread.Sleep(ms);
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	static extern void SetIOSThreadPriority(string threadName, int priority);
#endif
}
}