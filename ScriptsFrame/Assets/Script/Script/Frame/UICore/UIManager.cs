using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UICore
{
    public class UIManager :UnitySingleton<UIManager>
    {

        //缓存所有打开过的窗体
        private Dictionary<EUiId, BaseUI> dicAllUI;
        //缓存正在显示的窗体
        private Dictionary<EUiId, BaseUI> dicShowUI;
        //缓存跳转过来的窗体
        private BaseUI beforeUI = null;
        //新窗体显示出来后，缓存被隐藏的窗体ID
        private EUiId beforeHideUiId=EUiId.NullUI;
        //缓存画布
        private Transform canvas;
        //缓存所有窗体的父节点
        private Transform uiRoot;

        //当前ID
        [HideInInspector]
        public EUiId CurrentId = EUiId.NullUI;
        //当前窗体
        private BaseUI CurrnetUI = null;

        void Awake()
        {
            canvas = this.transform.parent;
            uiRoot = GameObject.Find("UiRoot").transform;
            dicAllUI = new Dictionary<EUiId, BaseUI>();
            dicShowUI = new Dictionary<EUiId, BaseUI>();
        }

        private void Start()
        {
            InitUIManager();
        }

        private void InitUIManager()
        {
            if (dicAllUI!=null)
            {
                dicAllUI.Clear();
            }
            if (dicShowUI!=null)
            {
                dicShowUI.Clear();
            }
            //切换场景的时候不让画布销毁
            DontDestroyOnLoad(canvas);
            //显示主窗体
            ShowUI(EUiId.MainUI);
        }

        public void AddUI(EUiId lastUiId,Transform parent, string EventTypeName = null, params object[] param)
        {
            BaseUI baseUI = JudgeShowUI(lastUiId, parent);
            if (baseUI != null)
            {
                if (EventTypeName != null)
                    SwanEngine.Events.Dispatcher.Instance.DispathEvent(EventTypeName, param);

                CurrentId = lastUiId;
                baseUI.ShowUI();
            }
        }

        public void ShowUI(EUiId lastUiId, EUiId currentUiId = EUiId.NullUI,string EventTypeName = null, params object[] param)
        {
            GameManager.Instance.PlayAudio();
            BaseUI currentUI = GetBaseUI(currentUiId);
            if (currentUI != null)
            {
                Debug.Log(currentUI.gameObject.name);
                Destroy(currentUI.gameObject);
            }
            if (dicShowUI.ContainsKey(currentUiId))
                dicShowUI.Remove(currentUiId);
            if (dicAllUI.ContainsKey(currentUiId))
                dicAllUI.Remove(currentUiId);
            
            BaseUI baseUI = JudgeShowUI(lastUiId);
            if (baseUI != null)
            {
                if (EventTypeName != null)
                    SwanEngine.Events.Dispatcher.Instance.DispathEvent(EventTypeName, param);

                CurrentId = lastUiId;
                baseUI.ShowUI();
            }
        }


        //显示窗体的方法（isSaveBeforeUiId代表是否要保存上一个切换过来的窗体ID,一般情况下是需要保存的，除了窗体的反向切换）
        public void ShowUI(EUiId UiId,bool isSaveBeforeUiId)//bool isSaveBeforeUiId=true
        {
            if (UiId==EUiId.NullUI)
            {
                UiId = EUiId.MainUI;
            }
            //1、判断窗体是否有显示过
            //如果有显示过，把窗体再次显示出来，隐藏当前窗体
            //从未显示过，先把窗体动态加载出来，然后再显示，接着隐藏当前窗体
            BaseUI baseUI = JudgeShowUI(UiId);
            if (baseUI!=null)
            {
                baseUI.ShowUI();
                if (isSaveBeforeUiId)
                {
                    baseUI.BeforeUiId = beforeHideUiId;
                }
            }
        }
        //反向切换
        public void ReturnUI(EUiId beforeUiId)
        {
            ShowUI(beforeUiId,false);
        }

        public BaseUI JudgeShowUI(EUiId uiId,Transform parent)
        {
            if (dicShowUI.ContainsKey(uiId))
            {
                return null;
            }
            BaseUI baseUI = GetBaseUI(uiId);
            if (baseUI == null)
            {
                if (GameDefine.dicPath.ContainsKey(uiId))
                {
                    string path = GameDefine.dicPath[uiId];
                    GameObject theUI = Resources.Load<GameObject>(path);
                    if (theUI != null)
                    {
                        GameObject willShowUI = Instantiate(theUI);
                        baseUI = willShowUI.GetComponent<BaseUI>();
                        if (baseUI == null)
                        {
                            Type type = GameDefine.GetUIScriptType(uiId);
                            baseUI = willShowUI.AddComponent(type) as BaseUI;
                        }
                        //把生成出来的窗体放在UiRoot下面
                        GameTool.AddChildToParent(parent, willShowUI.transform);
                        willShowUI.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                        willShowUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

                    }
                    else
                    {
                        Debug.LogError("在路径" + path + "下面找不到预制体" + uiId + ",请检查路径下面是否有该预制体");
                    }

                }
                else
                {
                    Debug.LogError("GameDefine下面没有窗体ID为" + uiId + "的加载路径");
                }
            }
            return baseUI;
        }


        public BaseUI JudgeShowUI(EUiId uiId)
        {
            if (dicShowUI.ContainsKey(uiId))
            {
                return null;
            }
            BaseUI baseUI = GetBaseUI(uiId);
            if (baseUI==null)//说明将要显示的窗体从未显示过
            {
                //去动态加载出来
                if (GameDefine.dicPath.ContainsKey(uiId))
                {
                    //有窗体的加载路径，就去加载
                    string path = GameDefine.dicPath[uiId];
                    GameObject theUI = Resources.Load<GameObject>(path);
                    if (theUI!=null)
                    {
                        GameObject willShowUI = Instantiate(theUI);
                        //判断显示出来的窗体上面是否有挂载UI脚本
                        baseUI = willShowUI.GetComponent<BaseUI>();
                        if (baseUI==null)//说明窗体上面没有挂载UI脚本
                        {
                            //自动挂载
                            Type type = GameDefine.GetUIScriptType(uiId);
                            baseUI = willShowUI.AddComponent(type) as BaseUI;
                        }
                        //把生成出来的窗体放在UiRoot下面
                        GameTool.AddChildToParent(uiRoot, willShowUI.transform);
                        willShowUI.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                        willShowUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                        dicAllUI.Add(uiId,baseUI);

                    }
                    else
                    {
                        Debug.LogError("在路径"+path+"下面找不到预制体"+ uiId+",请检查路径下面是否有该预制体");
                    }

                }
                else
                {
                    Debug.LogError("GameDefine下面没有窗体ID为"+ uiId+"的加载路径");
                }
            }
            UpdateDicShowUI(baseUI);
            return baseUI;
        }

        private void UpdateDicShowUI(BaseUI baseUI)
        {
            HideAllUI();
            dicShowUI.Add(baseUI.UiId,baseUI);
        }

        //判断窗体是否有显示过 
        private BaseUI GetBaseUI(EUiId uiId)
        {
            if (dicAllUI.ContainsKey(uiId))
            {
                return dicAllUI[uiId];
            }
            else
            {
                return null;
            }
        }
        //隐藏单个窗体
        public void HideSingleUI(EUiId uiId,DelAfterHideUI del)
        {
            if (!dicShowUI.ContainsKey(uiId))
            {
                return;
            }
            if (del!=null)
            {
                dicShowUI[uiId].HideUI(del);
            }
            else
            {
                dicShowUI[uiId].HideUI(null);
            }
            dicShowUI.Remove(uiId);
        }
        //隐藏所有窗体
        public void HideAllUI()
        {
            if (dicShowUI.Count > 0)
            {
                foreach (KeyValuePair<EUiId, BaseUI> item in dicShowUI)
                {
                    //隐藏正在显示的窗体
                    item.Value.HideUI(null);
                    //缓存上一个窗体的ID
                    beforeHideUiId = item.Key;
                }
                dicShowUI.Clear();
            }
        }
    }

}
