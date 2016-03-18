using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;

namespace EC.Threading {
public abstract class LoopThread {
		
	public int CycleTimeMS { get; set; }
	protected bool RunThread { get; set; }
	protected readonly string ThreadName;
	protected readonly Action Method;
	protected readonly Priority Priority;

	public LoopThread(Action method, string threadName, Priority priority, int cycleTimeMS) {
		RunThread = true;
		Method = method;
		ThreadName = threadName;
		Priority = priority;
		CycleTimeMS = cycleTimeMS;
	}

	protected void RunThreadLoop() {
		Stopwatch stopWatch = new Stopwatch();
		stopWatch.Start();
		int startTime;
		int deltaTime;
		while (RunThread) {
			startTime = (int)stopWatch.ElapsedMilliseconds;
			Method();
			deltaTime = (int)stopWatch.ElapsedMilliseconds - startTime;
			if (deltaTime < CycleTimeMS) {
				Wait(CycleTimeMS - deltaTime);
			}
			stopWatch.Reset();
		}
	}

	public abstract void Start();

	public abstract void Stop();

	public abstract void Wait(int ms);

	/// <summary>
	/// Returns UniLoopThread for proper platform.
	/// </summary>
	/// <param name="cycleTimeMS">Cycle time in Milliseconds.</param>
	/// <param name="debugCoroutineThread">Optional flag to run thread through coroutine for debuggin purposes.</param>
	static public LoopThread Create(Action method, string threadName, Priority priority, int cycleTimeMS = 0, bool debugCoroutineThread = false) {
		if (debugCoroutineThread)
			return new CoroutineLoopThread(method);
#if UNITY_WSA_10_0 && !UNITY_EDITOR
		return new UW10LoopThread(method, threadName, priority, cycleTimeMS);
#else
		return new StandardLoopThread(method, threadName, priority, cycleTimeMS);
#endif
	}
}
}