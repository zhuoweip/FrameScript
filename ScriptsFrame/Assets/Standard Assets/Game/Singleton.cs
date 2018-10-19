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
    public class UnitySingleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance = null;

        public static T Instance
        {
            get {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(T)) as T;

                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        return instance;
                    }

                    if (instance == null)
                    {
                        string instanceName = typeof(T).Name;
                        GameObject instanceGO = GameObject.Find(instanceName);

                        if (instanceGO == null)
                            instanceGO = new GameObject(instanceName);
                        instance = instanceGO.AddComponent<T>();
                        DontDestroyOnLoad(instanceGO);  //保证实例不会被释放
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        { }

        protected virtual void OnEnable()
        { }

        protected virtual void OnDisable()
        { }

        protected virtual void Start()
        { }

        protected virtual void LateUpdate()
        { }

        protected virtual void Update()
        { }

        protected virtual void OnGUI()
        { }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}

