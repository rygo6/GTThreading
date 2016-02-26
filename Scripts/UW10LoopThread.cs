using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace EC.UniThread {
public sealed class UW10LoopThread : LoopThread {

	#if UNITY_WSA_10_0 && !UNITY_EDITOR
    Windows.Foundation.IAsyncAction _async;
    System.Threading.ManualResetEvent _wait = new System.Threading.ManualResetEvent(false);
#endif

	public UW10LoopThread(Action method, string threadName, UniThreadPriority priority, int cycleTimeMS = 0) :
		base(method, threadName, priority, cycleTimeMS) {
		UnityEngine.Debug.Log("UW10LoopThread Created " + method.ToString() + " " + threadName + " " + priority + " " + cycleTimeMS);
	}

	public override void Start() {
#if UNITY_WSA_10_0 && !UNITY_EDITOR
		switch (Priority) {
		case UniThreadPriority.Normal:
			_async = Windows.System.Threading.ThreadPool.RunAsync((workItem) => { RunThreadLoop(); });
			break;
		case UniThreadPriority.Low:
			_async = Windows.System.Threading.ThreadPool.RunAsync((workItem) => { RunThreadLoop(); }, Windows.System.Threading.WorkItemPriority.Low);
			break;
		case UniThreadPriority.High:
			_async = Windows.System.Threading.ThreadPool.RunAsync((workItem) => { RunThreadLoop(); }, Windows.System.Threading.WorkItemPriority.High);
			break;
		}
#endif
	}

	public override void Stop() {
#if UNITY_WSA_10_0 && !UNITY_EDITOR
    	_async.Cancel();
        _async.Close();
#endif
	}

	public override void Wait(int ms) {
#if UNITY_WSA_10_0 && !UNITY_EDITOR
		_wait.WaitOne(ms);
#endif
	}


}
}