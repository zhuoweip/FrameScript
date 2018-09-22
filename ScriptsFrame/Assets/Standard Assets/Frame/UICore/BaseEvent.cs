using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwanEngine.Events
{
    public class BaseEvent
    {
        public string type;
        public object[] parm;

        public BaseEvent() { }

        public BaseEvent(string type)
        {
            this.type = type;
        }

        public BaseEvent(string type, params object[] parm)
        {
            this.type = type;
            this.parm = parm;
        }

        // 判断事件参数列表时要调用(JS会自动赋值这个列表 导致不为空)
        public bool IsHaveParm()
        {
            if (this.parm == null) return false;
            if (this.parm.Length == 0) return false;
            return true;
        }
    }
}