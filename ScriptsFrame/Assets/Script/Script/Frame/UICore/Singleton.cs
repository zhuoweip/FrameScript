using UnityEngine;
using System.Collections;

namespace UICore
{    
    //不继承于Mono的单例模式
    public class Singleton<T> where T : new()
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
        protected Singleton()
        {

        }
    }
    //继承于Mono的单例模式
    public class  UnitySingleton<T> : MonoBehaviour where T:Component
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance==null)
                {
                    GameObject go = GameObject.Find(typeof(T).Name);
                    if (go==null)
                    {
                        Debug.LogError("场景里面找不到名称为"+ typeof(T).Name+"的物体");
                    }
                    _instance = go.GetComponent<T>();
                }
                return _instance;
            }
        }
        protected UnitySingleton()
        {

        }
    }
}

