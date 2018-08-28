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

        protected override void Awake()
        {
            base.Awake();
            /// <summary>注册按钮动画</summary>
            RegistBtnsAnimation();
        }

        #region Button动画
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

        protected override void Update()
        {
            base.Update();
            #region TimeScale
            if (Input.GetKeyDown(KeyCode.F12)) { Time.timeScale = 50; }
            else if (Input.GetKeyUp(KeyCode.F12)) { Time.timeScale = 1; }
            #endregion
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            DebugOnGUI();
        }

        private Dictionary<GameDebugLog.LogLevel, GameDebugLog.LevelSetting> levelSettings;
        private int lastLogIndex;
        private int logIndex;

        #region Debug输出
        private void DebugOnGUI()
        {
            if (Input.GetKeyDown(KeyCode.F11) && logIndex == lastLogIndex)//加锁，不然会执行多次
                logIndex = 1 - logIndex;
            else if (Input.GetKeyUp(KeyCode.F11) && logIndex != lastLogIndex)
                lastLogIndex = logIndex;

            if (logIndex == 0) return;

            if (!Application.isPlaying) return;

            if (levelSettings == null)
            {
                levelSettings = new Dictionary<GameDebugLog.LogLevel, GameDebugLog.LevelSetting>();

                levelSettings[GameDebugLog.LogLevel.Normal] = new GameDebugLog.LevelSetting();
                levelSettings[GameDebugLog.LogLevel.Normal].Title = "Log: ";
                levelSettings[GameDebugLog.LogLevel.Normal].TitleColor = Color.blue;

                levelSettings[GameDebugLog.LogLevel.Warning] = new GameDebugLog.LevelSetting();
                levelSettings[GameDebugLog.LogLevel.Warning].Title = "Warning: ";
                levelSettings[GameDebugLog.LogLevel.Warning].TitleColor = new Color(1f, 0.5f, 0f, 1f);

                levelSettings[GameDebugLog.LogLevel.Error] = new GameDebugLog.LevelSetting();
                levelSettings[GameDebugLog.LogLevel.Error].Title = "Error: ";
                levelSettings[GameDebugLog.LogLevel.Error].TitleColor = new Color(1f, 0f, 0f, 1f);

                levelSettings[GameDebugLog.LogLevel.Exception] = new GameDebugLog.LevelSetting();
                levelSettings[GameDebugLog.LogLevel.Exception].Title = "Exception: ";
                levelSettings[GameDebugLog.LogLevel.Exception].TitleColor = new Color(1f, 0f, 0.5f, 1f);

                GameDebugLog.logPos = Vector2.zero;
                GameDebugLog.logRect = new Rect(Screen.width - 402, Screen.height - 268, 400, 266);
            }

            GUI.skin.label.fontSize = 40;
            Color backgroundColor = GUI.backgroundColor;
            Color oldContentColor = GUI.contentColor;
            GUI.backgroundColor = new Color(0.5f, 0.0f, 0.0f, 0.5f);
            GUI.Box(GameDebugLog.logRect, "");
            GUILayout.BeginArea(GameDebugLog.logRect);
            GameDebugLog.logPos = GUILayout.BeginScrollView(GameDebugLog.logPos, GUILayout.Width(400), GUILayout.Height(266));

            if (GameDebugLog.logs != null)
            {
                for (int i = 0, imax = GameDebugLog.logs.Count; i < imax; ++i)
                {
                    GUI.contentColor = levelSettings[GameDebugLog.logs[i].Level].TitleColor;
                    //GUILayout.BeginHorizontal();
                    GUILayout.Label(levelSettings[GameDebugLog.logs[i].Level].Title, GUILayout.Width(200));
                    GUI.contentColor = Color.green;
                    GUILayout.Label(GameDebugLog.logs[i].Msg);
                    //GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            GUI.contentColor = oldContentColor;
            GUI.backgroundColor = backgroundColor;
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

