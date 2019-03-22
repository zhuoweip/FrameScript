﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;
using System.Threading;

public class test1 : MonoBehaviour {

    public RawImage rimg;
    public MaskableGraphic graphic;

    [Serializable]
    public struct dataTool
    {
        public string a;
        public int b;
    }

    public dataTool dataTools = new dataTool();

    private void _Update()
    {
        Debug.LogError(44444);
    }
    void Start()
    {
        Auxiliary.AuxUpdate.Instance.callback += _Update;
        //var stream1 = Observable.Start(() =>
        //{
        //    Thread.Sleep(4000);//4000毫秒
        //    print("等待了4秒");
        //    return 1;
        //}); //线程1
        //var stream2 = Observable.Start(() =>
        //{
        //    Thread.Sleep(TimeSpan.FromSeconds(2));
        //    print("等待2秒");
        //    return 2;
        //}); //线程2


        //Observable.WhenAll(stream1, stream2).ObserveOnMainThread().Subscribe(_ => //当线程执行完毕，将其他线程中的值，返回到主线程中
        //{
        //    print(_[0]);
        //    print(_[1]);
        //});
        //graphic.RegisterDirtyLayoutCallback(() =>
        //{
        //    Debug.LogError(1);
        //});

        //CoroutineUtil.StartCoroutine("aa", AA());

        //CoroutineUtil.StartCoroutine("bb", BB());

    }


    private IEnumerator AA()
    {
        yield return new WaitForSeconds(2);
        Debug.LogError(1234);
    }

    private IEnumerator BB()
    {
        yield return new WaitForSeconds(2);
        Debug.LogError(5678);
    }


    private IEnumerator Wait()
    {
        yield return Observable.Timer(TimeSpan.FromSeconds(1)).ToYieldInstruction();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CoroutineUtil.StopCoroutine("aa");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            CoroutineUtil.StopCoroutine("bb");
        }
	}
}
