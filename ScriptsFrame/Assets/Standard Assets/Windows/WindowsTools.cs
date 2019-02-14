using System;
using System.Runtime.InteropServices;
using UnityEngine;
/*xbb
 * 系统方法类  http://www.office-cn.net/t/api/api_content.htm
 * */
public class WindowsTools
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hPos, int x, int y, int cx, int cy, uint nflags);

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

    [DllImport("user32.dll")]
    private static extern bool EnableWindow(IntPtr hwnd, bool enable);

    /// <summary>
    /// 设置当前窗口的显示状态
    /// </summary>
    [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
    private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    private static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wP, IntPtr IP);

    [DllImport("user32.dll", EntryPoint = "SetParent", CharSet = CharSet.Auto)]
    private static extern IntPtr SetParent(IntPtr hChild, IntPtr hParent);

    [DllImport("user32.dll", EntryPoint = "GetParent", CharSet = CharSet.Auto)]
    private static extern IntPtr GetParent(IntPtr hChild);

    [DllImport("User32.dll", EntryPoint = "GetSystemMetrics")]
    private static extern IntPtr GetSystemMetrics(int nIndex);

    /// <summary>
    /// 设置此窗体为活动窗体
    /// </summary>
    [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    /// 获取当前激活窗口
    /// </summary>
    [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
    public static extern System.IntPtr GetForegroundWindow();

    /// <summary>
    /// 设置窗口位置，大小
    /// </summary>
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    /// <summary>
    /// 窗口拖动
    /// </summary>
    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    #region 最小化恢复最小化
    /// <summary>
    /// 恢复一个最小化的程序，并将其激活
    /// </summary>
    /// <param name="hwnd"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    private static extern bool OpenIcon(IntPtr hwnd);

    /// <summary>
    /// 最小化指定的窗口。窗口不会从内存中清除
    /// </summary>
    /// <param name="hwnd"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    private static extern bool CloseWindow(IntPtr hwnd);
    #endregion

    private int windowLeft = 0;
    private int windowTop = 0;
    private int windowWidth = Screen.width;
    private int windowHeight = Screen.height;
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const int GWL_STYLE = -16;
    private const int GWL_WNDPROC = -4;
    private const int GWL_HINSTANCE = -6;
    private const int WS_BORDER = 1;
    private const int GWL_EXSTYLE = (-20);
    private const int WS_CAPTION = 0xC00000;
    private const int WS_POPUP = 0x800000;
    private const int SM_CXSCREEN = 0x00000000;
    private const int SM_CYSCREEN = 0x00000001;

    public enum HwndPos
    {
        HWND_TOP,
        HWND_TOPMOST,
        HWND_NORMAL,
    }

    public enum NCmdShow
    {
        /// <summary>
        /// 隐藏窗口，活动状态给令一个窗口
        /// </summary>
        SW_HIDE,
        /// <summary>
        /// 最小化窗口，活动状态给令一个窗口
        /// </summary>
        SW_MINIMIZE,
        /// <summary>
        /// 用原来的大小和位置显示一个窗口，同时令其进入活动状态
        /// </summary>
        SW_RESTORE,
        /// <summary>
        /// 用当前的大小和位置显示一个窗口，同时令其进入活动状态 ------最大化------
        /// </summary>
        SW_SHOW,
        /// <summary>
        /// 最大化窗口，并将其激活 ------没用------
        /// </summary>
        SW_SHOWMAXIMIZED,
        /// <summary>
        /// 最小化窗口，并将其激活 ------没用------
        /// </summary>
        SW_SHOWMINIMIZED,
        /// <summary>
        /// 最小化一个窗口，同时不改变活动窗口 ------最小化------
        /// </summary>
        SW_SHOWMINNOACTIVE,
        /// <summary>
        /// 用当前的大小和位置显示一个窗口，不改变活动窗口
        /// </summary>
        SW_SHOWNA,
        /// <summary>
        /// 用最近的大小和位置显示一个窗口，同时不改变活动窗口
        /// </summary>
        SW_SHOWNOACTIVATE,
        /// <summary>
        /// 与SW_RESTORE相同
        /// </summary>
        SW_SHOWNORMAL,
    }

    public enum ScreenDirection
    {
        DefaultDirection,
        Horizontal,
        Vertical,
    }
    public ScreenDirection CurDirection = ScreenDirection.DefaultDirection;

    public enum AppStyle
    {
        FullScreen = 0,
        WindowedFullScreen = 1,
        Windowed = 2,
        WindowedWithoutBorder = 3,
    }
    public AppStyle AppWindowStyle = AppStyle.WindowedFullScreen;

    public enum ZDepth
    {
        Normal = 0,
        Top = 1,
        TopMost = 2,
    }
    public ZDepth ScreenDepth = ZDepth.Normal;

    public static IntPtr FindUnityWindow { get { return FindWindow(null, Application.productName); } }

    public static void ShowWindows(IntPtr hWnd, NCmdShow nCmdShow)
    {
        ShowWindow(hWnd, (int)nCmdShow);
    }

    /*---------示例: SetWindowsPos(FindUnityWindow, HwndPos.HWND_NORMAL, 0, 0, 1920, 1080);---------------------------*/

    public static void SetWindowsPos(IntPtr hWnd, HwndPos hwndPos, int x, int y, int cx, int cy)
    {
        SetWindowPos(hWnd, new IntPtr(-1 * (int)hwndPos), x, y, cx, cy, SWP_SHOWWINDOW);
    }

    /// <summary>
    /// 最小化窗口
    /// </summary>
    public static void SetMinWindows()
    {
        ShowWindow(GetForegroundWindow(), (int)NCmdShow.SW_MINIMIZE);
        //具体窗口参数看这     https://msdn.microsoft.com/en-us/library/windows/desktop/ms633548(v=vs.85).aspx
    }

    /// <summary>
    /// 设置无边框，并设置框体大小，位置
    /// </summary>
    /// <param name="rect"></param>
    public static void SetNoFrameWindow(Rect rect)
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

    /// <summary>
    /// 指定的窗口里允许或禁止所有鼠标及键盘输入，在关闭之后必须在Application.Quit或者多少秒后重新开启，不然无法响应
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="enable"></param>
    /// <param name="action"></param>
    public static void EnableWindows(IntPtr hwnd, bool enable, Action action = null)
    {
        EnableWindow(hwnd, enable);
        if (action != null)
            action();
    }

    #region 任务栏
    private const string taskBarWindowName = "Shell_TrayWnd";
    /// <summary>
    /// 显示任务栏
    /// </summary>
    public static void ShowTaskBar()
    {
        ShowWindow(FindWindow(taskBarWindowName, null), (int)NCmdShow.SW_RESTORE);
    }
    /// <summary>
    /// 隐藏任务栏
    /// </summary>
    public static void HideTaskBar()
    {
        ShowWindow(FindWindow(taskBarWindowName, null), (int)NCmdShow.SW_HIDE);
    }
    #endregion
}
