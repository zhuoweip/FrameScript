//编辑器下使用Loom https://www.cnblogs.com/ferryqiu/p/8434762.html

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

/// <summary>
/// 使用比不使用的速度快，例如调用加载多图比不调用大概快500倍,这个是在编辑器运行的情况下，实际打包出来好像还要慢一点，待验证
/// 调用方法如下
/// Loom.RunAsync(() =>
///        {
///            Thread thread = new Thread(() =>
///            {
///                Loom.QueueOnMainThread(() =>
///                {
///                    Texture tex = Resources.Load<Texture>("white");
///                });
///            });
///            thread.Start();
///        });
/// </summary>
public class Loom : MonoBehaviour
{
    public static int maxThreads = 8;
    static int numThreads;

    private static Loom _current;
    public static Loom Current
    {
        get
        {
            Initialize();
            return _current;
        }
    }
    //####去除Awake
    //  void Awake()  
    //  {  
    //      _current = this;  
    //      initialized = true;  
    //  }  

    static bool initialized;

    //####作为初始化方法自己调用，可在初始化场景调用一次即可
    public static void Initialize()
    {
        if (!initialized)
        {

            if (!Application.isPlaying)
                return;
            initialized = true;
            GameObject g = new GameObject("Loom");
            //####永不销毁
            DontDestroyOnLoad(g);
            _current = g.AddComponent<Loom>();
        }

    }

    private List<Action> _actions = new List<Action>();
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    public static void QueueOnMainThread(Action action)
    {
        QueueOnMainThread(action, 0f); 
    }

    /// <summary>
    /// 回到主线程继续运行
    /// </summary>
    /// <param name="action"></param>
    /// <param name="time"></param>
    public static void QueueOnMainThread(Action action, float time)
    {
        if (time != 0)
        {
            if (Current != null)
            {
                lock (Current._delayed)
                {
                    Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
        }
        else
        {
            if (Current != null)
            {
                lock (Current._actions)
                {
                    Current._actions.Add(action);
                }
            }
        }
    }

    /// <summary>
    /// 操作延时函数
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Thread RunAsync(Action a)
    {
        Initialize();
        while (numThreads >= maxThreads)
        {
            Thread.Sleep(1);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    }

    private static void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch
        {
        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }

    }


    void OnDisable()
    {
        if (_current == this)
        {

            _current = null;
        }
    }



    // Use this for initialization  
    void Start()
    {

    }

    List<Action> _currentActions = new List<Action>();

    // Update is called once per frame  
    void Update()
    {
        lock (_actions)
        {
            _currentActions.Clear();
            _currentActions.AddRange(_actions);
            _actions.Clear();
        }
        foreach (var a in _currentActions)
        {
            a();
        }
        lock (_delayed)
        {
            _currentDelayed.Clear();
            _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
            foreach (var item in _currentDelayed)
                _delayed.Remove(item);
        }
        foreach (var delayed in _currentDelayed)
        {
            delayed.action();
        }



    }
}