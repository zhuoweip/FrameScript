using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;
using System.Threading;

public class test1 : MonoBehaviour {

    public RawImage rimg;

    void Start()
    {
        var stream1 = Observable.Start(() =>
        {
            Thread.Sleep(4000);//4000毫秒
            print("等待了4秒");
            return 1;
        }); //线程1
        var stream2 = Observable.Start(() =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            print("等待2秒");
            return 2;
        }); //线程2


        Observable.WhenAll(stream1, stream2).ObserveOnMainThread().Subscribe(_ => //当线程执行完毕，将其他线程中的值，返回到主线程中
        {
            print(_[0]);
            print(_[1]);
        });
    }


    private IEnumerator Wait()
    {
        yield return Observable.Timer(TimeSpan.FromSeconds(1)).ToYieldInstruction();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
