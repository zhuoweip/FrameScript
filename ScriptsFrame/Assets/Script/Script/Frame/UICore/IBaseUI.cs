using UnityEngine;

namespace UICore
{
    public class IBaseUI : MonoBehaviour
    {
        protected virtual void Awake()
        {
            /// <summary>初始化界面元素</summary>
            InitUiOnAwake();
            /// <summary>初始化界面数据</summary>
            InitDataOnAwake();
            /// <summary>注册按钮事件</summary>
            RegistBtns();
        }

        protected virtual void InitUiOnAwake()
        { }

        protected virtual void InitDataOnAwake()
        { }

        protected virtual void RegistBtns()
        { }

        protected virtual void Start()
        {
            InitStart();
        }

        protected virtual void InitStart()
        { }

        protected virtual void OnEnable()
        {
            AddListener();
        }

        protected virtual void AddListener()
        { }

        protected virtual void OnDisable()
        {

        }

        protected virtual void RemoveListener()
        { }

        protected virtual void Update()
        { }

        protected virtual void LateUpdate()
        { }

        protected virtual void OnGUI()
        { }

        //因为实现转场效果的时候要隐藏BaseUI，所以RemoveListener放在OnDestoty
        protected virtual void OnDestroy()
        {
            RemoveListener();
        }
    }
}

