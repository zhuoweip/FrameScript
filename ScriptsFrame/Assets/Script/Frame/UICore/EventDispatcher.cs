
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SwanEngine.Events
{
    public delegate void EventCallBack(BaseEvent e);

    /// <summary>
    /// 事件派发器
    /// by lijia
    /// </summary>
    public class EventDispatcher
    {
        private Dictionary<string, EventCallBack> dic = new Dictionary<string, EventCallBack>();

        public void AddEventListener(string type, EventCallBack callBack)
        {
            if (dic.ContainsKey(type))
            {
                dic[type] -= callBack;
                dic[type] += callBack;
            }
            else
            {
                dic[type] = callBack;
            }
        }

        public void RemoveEventListener(string key, EventCallBack callBack)
        {
            if (!dic.ContainsKey(key) || dic[key] == null)
            {
                return;
            }
            dic[key] -= callBack;
        }

        public void DispathEvent(BaseEvent e)
        {
            if (!dic.ContainsKey(e.type) || dic[e.type] == null)
            {
                return;
            }
            dic[e.type](e);
        }

        public void DispathEvent(string type)
        {
            DispathEvent(type, null);
        }

        public void DispathEvent(string type, params object[] parm)
        {
            BaseEvent e = new BaseEvent(type, parm);
            DispathEvent(e);
        }

        public void RemoveAllEvent()
        {
            dic.Clear();
        }
    }
}
