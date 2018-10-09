using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UICore;
using System;
using System.Drawing;

//104键键盘按键码对照
//https://www.cnblogs.com/smartstone/p/5559796.html

public class KeyBoardEvent
{
    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    public static extern void keybd_event(
        byte bvk,//虚拟键值 ESC键对应的是27
        byte bScan,//0
        int dwFlags,//0为按下，1按住，2释放
        int dwExtraInfo//0
        );

    /// <summary>
    /// 按下F N 键
    /// </summary>
    /// <param name="num"></param>
    public static void ClickFNum(byte num)
    {
        keybd_event(num, 0, 0, 0);
        keybd_event(num, 0, 1, 0);
        keybd_event(num, 0, 2, 0);
    }

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(ref Point lpPoint);
    [DllImport("user32.dll")]
    private static extern int SetCursorPos(int x, int y);
    [DllImport("user32.dll")]
    static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

    /// <summary>
    /// 鼠标操作标志位集合
    /// </summary>
    [Flags]
    enum MouseEventFlag : uint
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        /// <summary>
        /// 设置鼠标坐标为绝对位置（dx,dy）,否则为距离最后一次事件触发的相对位置
        /// </summary>
        Absolute = 0x8000
    }

    // Unity屏幕坐标从左下角开始，向右为X轴，向上为Y轴
    // Windows屏幕坐标从左上角开始，向右为X轴，向下为Y轴

    /// <summary>
    /// 移动鼠标到指定位置（使用Unity屏幕坐标而不是Windows屏幕坐标）
    /// </summary>
    public static bool MoveTo(float x, float y)
    {
        if (x < 0 || y < 0 || x > Screen.width || y > Screen.height)
            return true;

        if (!Screen.fullScreen)
        {
            Debug.LogError("只能在全屏状态下使用！");
            return false;
        }

        SetCursorPos((int)x, (int)(Screen.height - y));
        return true;
    }

    // 左键单击
    public static void LeftClick(float x = -1, float y = -1)
    {
        if (MoveTo(x, y))
        {
            mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
        }
    }

    // 右键单击
    public static void RightClick(float x = -1, float y = -1)
    {
        if (MoveTo(x, y))
        {
            mouse_event(MouseEventFlag.RightDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.RightUp, 0, 0, 0, UIntPtr.Zero);
        }
    }

    // 中键单击
    public static void MiddleClick(float x = -1, float y = -1)
    {
        if (MoveTo(x, y))
        {
            mouse_event(MouseEventFlag.MiddleDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.MiddleUp, 0, 0, 0, UIntPtr.Zero);
        }
    }

    // 左键按下
    public static void LeftDown(float x = -1, float y = -1)
    {
        if (MoveTo(x, y))
            mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
    }

    // 左键抬起
    public static void LeftUp(float x = -1, float y = -1)
    {
        if (MoveTo(x, y))
            mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
    }

    // 右键按下
    public static void RightDown(float x = -1, float y = -1)
    {
        if (MoveTo(x, y))
            mouse_event(MouseEventFlag.RightDown, 0, 0, 0, UIntPtr.Zero);
    }

    // 右键抬起
    public static void RightUp(float x = -1, float y = -1)
    {
        if (MoveTo(x, y))
            mouse_event(MouseEventFlag.RightUp, 0, 0, 0, UIntPtr.Zero);
    }

    // 中键按下
    public static void MiddleDown(float x = -1, float y = -1)
    {
        if (MoveTo(x, y))
            mouse_event(MouseEventFlag.MiddleDown, 0, 0, 0, UIntPtr.Zero);
    }

    // 中键抬起
    public static void MiddleUp(float x = -1, float y = -1)
    {
        if (MoveTo(x, y))
            mouse_event(MouseEventFlag.MiddleUp, 0, 0, 0, UIntPtr.Zero);
    }

    // 滚轮滚动
    public static void ScrollWheel(float value)
    {
        mouse_event(MouseEventFlag.Wheel, 0, 0, (uint)value, UIntPtr.Zero);
    }

    public static Point lastPoint;
    public static void DoMouseClick(int x, int y)
    {
        GetCursorPos(ref lastPoint);
        int dx = (int)((double)x / Screen.width * 65535); //屏幕分辨率映射到0~65535(0xffff,即16位)之间
        int dy = (int)((double)y / Screen.height * 0xffff); //转换为double类型运算，否则值为0、1
        mouse_event(MouseEventFlag.Move | MouseEventFlag.LeftDown | MouseEventFlag.LeftUp | MouseEventFlag.Absolute, dx, dy, 0, new UIntPtr(0)); //点击
    }
}
