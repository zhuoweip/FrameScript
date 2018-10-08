using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class KeyBoardEvent : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    public static extern void Keybd_event(
        byte bvk,//虚拟键值 ESC键对应的是27
        byte bScan,//0
        int dwFlags,//0为按下，1按住，2释放
        int dwExtraInfo//0
        );

    void Start()
    {
        Keybd_event(27, 0, 0, 0);
        Keybd_event(27, 0, 0x1, 0);
        Keybd_event(27, 0, 0x2, 0);

        //Keybd_event(17, 0, 0, 0);
        //Keybd_event(32, 0, 0, 0);
        //Keybd_event(17, 0, 1, 0);
        //Keybd_event(32, 0, 1, 0);
        //Keybd_event(17, 0, 2, 0);
        //Keybd_event(32, 0, 2, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("按下了ESC键");
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            print("按住了ESC键");
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            print("松开了ESC键");
        }
    }
}
