using UnityEngine;
using System.Collections.Generic;
using CommandTerminal;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;

namespace UICore
{
    /// <summary>
    /// 需要挂载
    /// </summary>
    [RequireComponent(typeof(Terminal))]
    public class CmdManager : UnitySingleton<CmdManager>
    {
        private Process[] processes;

        protected override void Update()
        {
            base.Update();
            #region TimeScale
            if (Input.GetKeyDown(KeyCode.F12)) { Time.timeScale = 50; }
            else if (Input.GetKeyUp(KeyCode.F12)) { Time.timeScale = 1; }
            else if (Input.GetKeyDown(KeyCode.F11)){Time.timeScale = 0; }
            #endregion

            #region QualitySettings
            if (Input.GetKeyDown(KeyCode.F1)) {
                if (QualitySettings.GetQualityLevel() == 5)
                    return;
                QualitySettings.IncreaseLevel();
            }
            else if (Input.GetKeyDown(KeyCode.F2)) {
                if (QualitySettings.GetQualityLevel() == 0)
                    return;
                QualitySettings.DecreaseLevel();
            }
            #endregion

            #region KeyBoard
            processes = Process.GetProcessesByName("osk");
            if (Input.GetKeyDown(KeyCode.F7))
            {
                if (processes.Length == 0)
                    Process.Start(@"C:\Windows\System32\osk.exe");
            }
            #endregion

            #region Win10触摸键盘
            if (Input.GetKeyDown(KeyCode.F3))
            {
                //先关闭再打开，这样可以关闭后台的进程，因为如果在外部点×直接把键盘关掉，进程一直存在，再次打开的话会打不开
                HideTouchKeyboard();
                ShowTouchKeyboard();
            }
            #endregion
        }

        [DllImport("user32")]
        static extern IntPtr FindWindow(String sClassName, String sAppName);
        [DllImport("user32")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        /// <summary>
        /// 显示屏幕键盘
        /// </summary>
        public void ShowTouchKeyboard()
        {
            try
            {
                ExternalCall("C:\\Program Files\\Common Files\\Microsoft Shared\\ink\\tabtip.exe", null, false);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
        }
        /// <summary>
        /// 隐藏屏幕键盘
        /// </summary>
        public void HideTouchKeyboard()
        {
            try
            {
                uint WM_SYSCOMMAND = 274;
                int SC_CLOSE = 61536;
                IntPtr ptr = FindWindow("IPTip_Main_Window", null);
                if (ptr == IntPtr.Zero)
                    return;
                PostMessage(ptr, WM_SYSCOMMAND, SC_CLOSE, 0);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
        }
        private static Process ExternalCall(string filename, string arguments, bool hideWindow)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = filename;
            startInfo.Arguments = arguments;
            // if just command, we don't want to see the console displayed
            if (hideWindow)
            {
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
            }
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            return process;
        }

        public const UInt32 WS_VISIBLE = 0X94000000;
        /// <summary>
        /// Specifies we wish to retrieve window styles.
        /// </summary>
        public const int GWL_STYLE = -16;

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>
        /// Gets the window handler for the virtual keyboard.
        /// </summary>
        /// <returns>The handle.</returns>
        public static IntPtr GetKeyboardWindowHandle()
        {
            return FindWindow("IPTip_Main_Window", null);
        }

        /// <summary>
        /// 检查触摸键盘是否可见
        /// </summary>
        /// <returns>True if visible.</returns>
        public static bool IsKeyboardVisible()
        {
            IntPtr keyboardHandle = GetKeyboardWindowHandle();
            bool visible = false;
            if (keyboardHandle != IntPtr.Zero)
            {
                UInt32 style = GetWindowLong(keyboardHandle, GWL_STYLE);
                visible = (style == WS_VISIBLE);
            }
            return visible;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            if (GUI.skin.label.fontSize != 40)
                GUI.skin.label.fontSize = 40;

            DebugOnGUI();
            ShowFPS();
            DragWindow();
        }

        private void DragWindow()
        {
            if (Input.GetKeyDown(KeyCode.F8) && windowsIndex == lastWindowIndex)
                windowsIndex = 1 - lastWindowIndex;
            else if (Input.GetKeyUp(KeyCode.F8) && windowsIndex != lastWindowIndex)
                lastWindowIndex = windowsIndex;

            if (windowsIndex == 0) return;

            if (GUI.RepeatButton(new Rect(0, 0, Screen.currentResolution.width, Screen.currentResolution.height), string.Empty))
                WindowsTools.DragWindow(WindowsTools.GetForegroundWindow());
        }

        private Dictionary<GameDebugLog.LogLevel, GameDebugLog.LevelSetting> levelSettings;
        private int lastLogIndex;
        private int logIndex;
        private int lastFpsIndex;
        private int fpsIndex;
        private int lastWindowIndex;
        private int windowsIndex;

        #region FPS输出
        private void ShowFPS()
        {
            if (Input.GetKeyDown(KeyCode.F10) && fpsIndex == lastFpsIndex)
                fpsIndex = 1 - lastFpsIndex;
            else if (Input.GetKeyUp(KeyCode.F10) && fpsIndex != lastFpsIndex)
                lastFpsIndex = fpsIndex;

            if (fpsIndex == 0) return;
            GUILayout.Label(string.Format("FPS:{0}", 1 / Time.smoothDeltaTime));
        }
        #endregion

        #region Debug输出
        private void DebugOnGUI()
        {
            if (Input.GetKeyDown(KeyCode.F9) && logIndex == lastLogIndex)//加锁，不然会执行多次
                logIndex = 1 - logIndex;
            else if (Input.GetKeyUp(KeyCode.F9) && logIndex != lastLogIndex)
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
    }
}


   

