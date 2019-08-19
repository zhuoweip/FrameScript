using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UICore;

#region 改变系统声音
#endregion

[DisallowMultipleComponent]
public class SoundManger : UnitySingleton<SoundManger>
{
    #region System
    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);

    [DllImport("user32.dll")]
    static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);

    private const byte VK_VOLUME_MUTE = 0xAD;
    private const byte VK_VOLUME_DOWN = 0xAE;
    private const byte VK_VOLUME_UP = 0xAF;
    private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;
    private const UInt32 KEYEVENTF_KEYUP = 0x0002;

    /// <summary>
    /// 改变系统音量大小，增加
    /// </summary>
    public void SystemVolumeUp()
    {
        keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
        keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
    }

    /// <summary>
    /// 改变系统音量大小，减小
    /// </summary>
    public void SystemVolumeDown()
    {
        keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);
        keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
    }

    /// <summary>
    /// 改变系统音量大小，静音
    /// </summary>
    public void SystemMute()
    {
        keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);
        keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
    }
    #endregion

    #region Program
    [DllImport("Winmm.dll")]
    private static extern int waveOutSetVolume(int hwo, System.UInt32 pdwVolume);

    [DllImport("Winmm.dll")]
    private static extern uint waveOutGetVolume(int hwo, out System.UInt32 pdwVolume);

    private const int volumeMinScope = 0;
    private const int volumeMaxScope = 100;

    private int lastVolume = 0;
    private int volume = 100;

    public int Volume
    {
        get { return volume; }
        set
        {
            volume = value;
            SetCurrentVolume();
        }
    }
    private void SetCurrentVolume()
    {
        volume = volume < volumeMinScope ? volumeMinScope : volume;
        volume = volume > volumeMaxScope ? volumeMaxScope : volume;
        //先把trackbar的value值映射到0x0000～0xFFFF范围
        System.UInt32 Value = (System.UInt32)((double)0xffff * volume / (volumeMaxScope - volumeMinScope));

        //限制value的取值范围
        Value = Value < 0 ? 0 : Value;
        Value = Value > 0xffff ? 0xffff : Value;

        System.UInt32 left = Value;     //左声道音量
        System.UInt32 right = Value;        //右声道音量
        waveOutSetVolume(0, left << 16 | right);        //"<<"左移，“|”逻辑或运算
    }

    /// <summary>
    /// 改变程序音量，增加
    /// </summary>
    /// <param name="value">增加的幅度</param>
    public void ProgramVolumeUp(int value = 2)
    {
        Volume += value;
    }

    /// <summary>
    /// 改变程序音量，减少
    /// </summary>
    /// <param name="value">减少的幅度</param>
    public void ProgramVolumeDown(int value = 2)
    {
        Volume -= value;
    }

    /// <summary>
    /// 改成程序音量，静音
    /// </summary>
    public void ProgramMute()
    {
        int temp = lastVolume;
        lastVolume = volume;
        Volume = temp;
    }
    #endregion

    public enum MangerType
    {
        Program,
        System,
    }

    public MangerType mangerType = MangerType.Program;

    //protected override void Update()
    //{
    //    base.Update();
    //    switch (mangerType)
    //    {
    //        case MangerType.Program:
    //            if (Input.GetKeyDown(KeyCode.UpArrow))
    //                ProgramVolumeUp();
    //            if (Input.GetKeyDown(KeyCode.DownArrow))
    //                ProgramVolumeDown();
    //            if (Input.GetKeyDown(KeyCode.Space))
    //                ProgramMute();
    //            break;
    //        case MangerType.System:
    //            if (Input.GetKeyDown(KeyCode.UpArrow))
    //                SystemVolumeUp();
    //            if (Input.GetKeyDown(KeyCode.DownArrow))
    //                SystemVolumeDown();
    //            if (Input.GetKeyDown(KeyCode.Space))
    //                SystemMute();
    //            break;
    //    }
    //}
}