using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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

        private Type[] baseUiSubTypes;

        //获得所有BaseUI的子类
        private void InitGetBaseUiSubType()
        {
            baseUiSubTypes = (Type[])LinqUtil.CustomWhere(Assembly.GetExecutingAssembly().GetTypes(),t => t.IsSubclassOf(typeof(BaseUI)));
        }

        private Type GetTypeByUiId(EUiId eUiId)
        {
            foreach (var item in baseUiSubTypes)
            {
                if (item.Name == Enum.GetName(eUiId.GetType(), eUiId))
                    return item;
            }
            return null;
        }

        void Awake()
        {
            canvas = this.transform.parent;
            uiRoot = GameObject.Find("UIRoot").transform;
            dicAllUI = new Dictionary<EUiId, BaseUI>();
            dicShowUI = new Dictionary<EUiId, BaseUI>();
            InitGetBaseUiSubType();
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

        public void ShowUI(EUiId nextUiId, Transform parent = null, string EventTypeName = null, params object[] param)
        {
            AudioManager.Instance.Play_Audio();
            BaseUI currentUI = GetBaseUI(CurrentId);

            if (currentUI != null && parent == null)
            {
                //父级baseUI
                EUiId parentUiId = EUiId.NullUI;
                BaseUI parentUI = currentUI.GetComponentInParentNotInCludSelf<BaseUI>();
                if (parentUI != null)
                    parentUiId = parentUI.UiId;

                BaseUI[] childUI = currentUI.GetComponentsInFirstHierarchyChildren<BaseUI>(true);

                //如果父级有baseUI并且直接打开的是别的UI的话就删除
                if (parentUiId != EUiId.NullUI && nextUiId != parentUiId)
                    Destroy(parentUI.gameObject);
                else
                    Destroy(currentUI.gameObject);

                RemoveUiId(CurrentId);
                if (childUI != null)
                {
                    for (int i = 0; i < childUI.Length; i++)
                    {
                        EUiId chiidUiId = childUI[i].UiId;
                        RemoveUiId(chiidUiId);
                    }
                }
                if (parentUiId != EUiId.NullUI && nextUiId != parentUiId)
                    RemoveUiId(parentUiId);
            }

            BaseUI baseUI = JudgeShowUI(nextUiId, parent);
            if (baseUI != null)
            {
                if (EventTypeName != null)
                    SwanEngine.Events.Dispatcher.Instance.DispathEvent(EventTypeName, param);
                CurrentId = nextUiId;
                baseUI.ShowUI();
            }
        }

        private void RemoveUiId(EUiId uiId)
        {
            if (dicShowUI.ContainsKey(uiId))
                dicShowUI.Remove(uiId);
            if (dicAllUI.ContainsKey(uiId))
                dicAllUI.Remove(uiId);
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
                            Type type = GetTypeByUiId(uiId);
                            baseUI = willShowUI.AddComponent(type) as BaseUI;
                        }
                        //把生成出来的窗体放在UiRoot下面
                        willShowUI.SetParent(parent == null ? uiRoot : parent);
                        willShowUI.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                        willShowUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                        dicAllUI.Add(uiId, baseUI);
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
            UpdateDicShowUI(baseUI, !parent);
            return baseUI;
        }

        private void UpdateDicShowUI(BaseUI baseUI,bool isHideAllUI = true)
        {
            if (isHideAllUI)
                HideAllUI();
            dicShowUI.Add(baseUI.UiId,baseUI);
        }

        //隐藏所有窗体
        public void HideAllUI()
        {
            if (dicShowUI.Count > 0)
            {
                foreach (var item in dicShowUI)
                {
                    //隐藏正在显示的窗体
                    item.Value.HideUI(null);
                    //缓存上一个窗体的ID
                    beforeHideUiId = item.Key;
                }
                dicShowUI.Clear();
            }
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
    }
}
