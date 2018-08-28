using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UICore;
//消息中心（观察者模式）
public class MessageCenter
{
    public delegate void DelCallBack(object obj);
    //存放所有监听的字典
    public static Dictionary<EMessageType, DelCallBack> dicMessageType = new Dictionary<EMessageType, DelCallBack>();
    //添加监听
    public static void AddListener(EMessageType messageType,DelCallBack handler)
    {
        if (!dicMessageType.ContainsKey(messageType))
        {
            dicMessageType.Add(messageType,null);
        }
        dicMessageType[messageType] += handler;
    }
    //取消监听
    public static void RemoveListener(EMessageType messageType, DelCallBack handler)
    {
        if (dicMessageType.ContainsKey(messageType))
        {
            dicMessageType[messageType] -= handler;
        }
    }
    //取消所有监听
    public static void RemoveAllListener()
    {
        dicMessageType.Clear();
    }
    //分发消息
    public static void SendMessage(EMessageType messageType,object obj=null)
    {
        DelCallBack del;
        if (dicMessageType.TryGetValue(messageType,out del))
        {
            if (del!=null)
            {
                del(obj);
            }
        }
    }
}
