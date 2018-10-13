using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEditor;

public class LoomEditor
{
    /// <summary>
    /// 当前是否有unity任务需要执行
    /// </summary>
    static bool hasUnityAction = true;

    private static Thread loomThread;

    /// <summary>
    /// unity任务表
    /// </summary>
    private List<Action> actions = new List<Action>();

    #region 单例 注册update事件
    private static LoomEditor _instance;
    private static readonly object lockObj = new object();
    public static LoomEditor Current
    {
        get
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new LoomEditor();
                    }

                }
            }
            return _instance;
        }
    }
    private LoomEditor()
    {
        EditorApplication.update += Update;

    }
    #endregion

    /// <summary>
    /// 子线程启动一个任务
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public Thread RunAsync(Action a)
    {
        if (loomThread != null)
        {
            Stop();
            throw new Exception("任务仅运行一次");
        }
        loomThread = new Thread(new ParameterizedThreadStart(RunAction));
        loomThread.Name = "LoomEditor线程";
        loomThread.Priority = System.Threading.ThreadPriority.Lowest;
        loomThread.Start(a);
        return loomThread;
    }
    /// <summary>
    /// 加入一个任务到主线程队列
    /// </summary>
    /// <param name="action"></param>
    public void QueueOnMainThread(Action action)
    {
        if (Current != null && Thread.CurrentThread == loomThread)
        {
            hasUnityAction = true;
            lock (Current.actions)
            {
                Current.actions.Add(action);
            }
            while (hasUnityAction)
            {
                loomThread.Priority = System.Threading.ThreadPriority.Lowest;
                Thread.Sleep(10);
            }
        }

    }

    /// <summary>
    /// 延迟子线程
    /// </summary>
    /// <param name="time"></param>
    public void Sleep(int time)
    {
        if (Current != null && Thread.CurrentThread == loomThread)
        {
            Thread.Sleep(time);

        }
    }

    /// <summary>
    /// 停止任务
    /// </summary>
    public void Stop()
    {
        EditorApplication.update -= Update;
        try
        {
            loomThread.Abort();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            loomThread = null;
            _instance = null;
        }

    }



    private void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch
        {
        }

    }

    List<Action> _currentActions = new List<Action>();

    static void Update()
    {
        try
        {


            if (!hasUnityAction) return;

            lock (Current.actions)
            {
                Current._currentActions.Clear();
                Current._currentActions.AddRange(Current.actions);
                Current.actions.Clear();
            }
            for (int i = 0; i < Current._currentActions.Count; i++)
            {
                Debug.LogError("主线程任务");
                Current._currentActions[i]();

            }
            hasUnityAction = false;
        }
        catch
        {
            Debug.LogError("主线程任务失败");
        }
    }
}