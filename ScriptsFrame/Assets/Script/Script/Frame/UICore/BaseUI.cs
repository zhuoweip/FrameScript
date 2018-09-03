using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UICore
{
    public delegate void DelAfterHideUI();
    public class BaseUI : IBaseUI
    {
        protected Transform thisTransform;
        //当前窗体的ID
        protected EUiId uiId = EUiId.NullUI;
        //上一个跳转过来的窗体ID
        protected EUiId beforeUiId = EUiId.NullUI;

        public EUiId UiId { get { return uiId; } }

        public EUiId BeforeUiId { get { return beforeUiId; } set { beforeUiId = value; } }

        public BaseUI()
        {
            uiId = (EUiId)Enum.Parse(typeof(EUiId), this.GetType().Name);
        }

        #region 注册按钮动画
        /// <summary>
        /// 调用先继承，再在RegistBtns()调用方法
        /// </summary>
        protected virtual void RegistBtnsAnimation()
        {
            Button[] btns = transform.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < btns.Length; i++)
            {
                EventTriggerListener.Get(btns[i]).onDown = RegistDownAni;
                EventTriggerListener.Get(btns[i]).onUp = RegistUpAni;
            }
        }

        private void RegistDownAni(GameObject go)
        {
            DoTweenManager.Instance.DoScaleTween(go.transform, 0.8f, 0.1f);
        }

        private void RegistUpAni(GameObject go)
        {
            DoTweenManager.Instance.DoScaleTween(go.transform, 1f, 0.1f);
        }
        #endregion

        //显示
        public virtual void ShowUI()
        {
            this.gameObject.SetActive(true);
        }

        //隐藏
        public virtual void HideUI(DelAfterHideUI del)
        {
            this.gameObject.SetActive(false);
            if (del != null) { del(); }
            Save();
        }

        //保存
        protected virtual void Save()
        { }
    }
}

