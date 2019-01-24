using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
/*xbb
 * 系统方法类
 * */
public class WindowsTools {

    #region 任务栏
    private const int SW_HIDE = 0;  //hied task bar
    private const int SW_RESTORE = 9;//show task bar
    /// <summary>
    /// 显示任务栏
    /// </summary>
    public static void ShowTaskBar()
    {
        ShowWindow(FindWindow("Shell_TrayWnd", null), SW_RESTORE);
    }
    /// <summary>
    /// 隐藏任务栏
    /// </summary>
    public static void HideTaskBar()
    {
        ShowWindow(FindWindow("Shell_TrayWnd", null), SW_HIDE);
    }
    #endregion

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hPos, int x, int y, int cx, int cy, uint nflags);

    [DllImport("User32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    /// <summary>
    /// 设置窗口边框
    /// </summary>
    [DllImport("User32.dll", EntryPoint = "SetWindowLong")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("User32.dll", EntryPoint = "GetWindowLong")]
    private static extern int GetWindowLong(IntPtr hWnd, int dwNewLong);

    [DllImport("User32.dll", EntryPoint = "MoveWindow")]
    private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

    /// <summary>
    /// 设置当前窗口的显示状态
    /// </summary>
    [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
    public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wP, IntPtr IP);

    [DllImport("user32.dll", EntryPoint = "SetParent", CharSet = CharSet.Auto)]
    public static extern IntPtr SetParent(IntPtr hChild, IntPtr hParent);

    [DllImport("user32.dll", EntryPoint = "GetParent", CharSet = CharSet.Auto)]
    public static extern IntPtr GetParent(IntPtr hChild);

    [DllImport("User32.dll", EntryPoint = "GetSystemMetrics")]
    public static extern IntPtr GetSystemMetrics(int nIndex);

    /// <summary>
    /// 设置此窗体为活动窗体
    /// </summary>
    [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    /// 获取当前激活窗口
    /// </summary>
    [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
    public static extern System.IntPtr GetForegroundWindow();

    /// <summary>
    /// 设置窗口位置，大小
    /// </summary>
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    /// <summary>
    /// 窗口拖动
    /// </summary>
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    public enum AppStyle
    {
        FullScreen = 0,
        WindowedFullScreen = 1,
        Windowed = 2,
        WindowedWithoutBorder = 3,
    }
    public AppStyle AppWindowStyle = AppStyle.WindowedFullScreen;

    public enum zDepth
    {
        Normal = 0,
        Top = 1,
        TopMost = 2,
    }

    //边框参数
    const int SW_SHOWMINIMIZED = 2;//(最小化窗口)
    IntPtr currentWindow;
    public zDepth ScreenDepth = zDepth.Normal;
    public int windowLeft = 0;
    public int windowTop = 0;
    private int windowWidth = Screen.width;
    private int windowHeight = Screen.height;
    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int GWL_WNDPROC = -4;
    const int GWL_HINSTANCE = -6;
    const int WS_BORDER = 1;
    private Rect screenPosition;
    private const int GWL_EXSTYLE = (-20);
    private const int WS_CAPTION = 0xC00000;
    private const int WS_POPUP = 0x800000;
    IntPtr HWND_TOP = new IntPtr(0);
    IntPtr HWND_TOPMOST = new IntPtr(-1);
    IntPtr HWND_NORMAL = new IntPtr(-2);
    private const int SM_CXSCREEN = 0x00000000;
    private const int SM_CYSCREEN = 0x00000001;
    int Xscreen;
    int Yscreen;
    public bool StartAuto = false;

    public enum ScreenDirection
    {
        defaultDirection,
        horizontal,
        vertical,
    }

    public ScreenDirection CurDirection = ScreenDirection.defaultDirection;

    /// <summary>
    /// 最小化窗口
    /// </summary>
    public void SetMinWindows()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
        //具体窗口参数看这     https://msdn.microsoft.com/en-us/library/windows/desktop/ms633548(v=vs.85).aspx
    }

    /// <summary>
    /// 设置无边框，并设置框体大小，位置
    /// </summary>
    /// <param name="rect"></param>
    public void SetNoFrameWindow(Rect rect)
    {
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);
        bool result = SetWindowPos(GetForegroundWindow(), 0, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, SWP_SHOWWINDOW);
    }

    /// <summary>
    /// 拖动窗口
    /// </summary>
    /// <param name="window"></param>
    public static void DragWindow(IntPtr window)
    {
        ReleaseCapture();
        SendMessage(window, 0xA1, 0x02, 0);
        SendMessage(window, 0x0202, 0, 0);
    }
}
