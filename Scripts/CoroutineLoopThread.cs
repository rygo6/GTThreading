using UnityEngine;
using System;
using System.Collections;

namespace EC.UniThread {
public sealed class CoroutineLoopThread : LoopThread {

	MonoBehaviour _monoBehaviour;

	public CoroutineLoopThread(Action method) :
		base(method, UniThreadPriority.Low, 0) {
	}

	public override void Start() {
		GameObject gameObject = new GameObject("ThreadCoroutine");
		_monoBehaviour = gameObject.AddComponent<MonoBehaviour>();
		_monoBehaviour.StartCoroutine(ThreadCoroutine(Method));
	}

	public override void Stop() {
		_monoBehaviour.StopAllCoroutines();
	}

	public override void Wait(int ms) {
		//There is no way to implement without forcing the user
		//to create the thread Action Method; an IENumerator
		//But thats ok for now, as I only intend CoroutineThread
		//to be used for optional debugging
	}

	IEnumerator ThreadCoroutine(Action method) {
		while (true) {
			Method();
			yield return null;
		}
	}
}
}