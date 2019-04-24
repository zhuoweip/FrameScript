 using UICore;
using UnityEngine;

/// <summary>
/// 加载器,加载保存数据防止重复加载
/// </summary>
public class Loader : UnitySingleton<Loader>
{
    private Object[] dataObjs;
    private Object dataObj;

    /// <summary>
    /// LoadAll
    /// </summary>
    /// <param name="_objs"></param>
    /// <param name="path"></param>
    public T[] LoadObjects<T>(string path) where T : Object
    {
        if (dataObjs == null || dataObjs.Length == 0)
            dataObjs = Resources.LoadAll<T>(path);
        return dataObjs as T[];
    }

    /// <summary>
    /// Load
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="path"></param>
    public T LoadObject<T>(string path) where T : Object
    {
        if (dataObj == null)
            dataObj = Resources.Load<T>(path);
        return dataObj as T;
    }
}
