using UnityEngine;
using WindowsInput;
using WindowsInput.Native;

/// <summary>
/// 模拟键盘鼠标工具 https://github.com/michaelnoonan/inputsimulator
/// </summary>
public class InputSimulatorExamples : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
            SayHello();
	}

    public void OpenWindowsExplorer()
    {
        var sim = new InputSimulator();
        sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_E);
    }

    public void SayHello()
    {
        var sim = new InputSimulator();
        sim.Keyboard
           .ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R)
           .Sleep(1000)
           .TextEntry("notepad")
           .Sleep(1000)
           .KeyPress(VirtualKeyCode.RETURN)
           .Sleep(1000)
           .TextEntry("These are your orders if you choose to accept them...")
           .TextEntry("This message will self destruct in 5 seconds.")
           .Sleep(5000)
           .ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE)
           .KeyPress(VirtualKeyCode.DOWN)
           .KeyPress(VirtualKeyCode.RETURN);

        var i = 10;
        while (i-- > 0)
        {
            sim.Keyboard.KeyPress(VirtualKeyCode.DOWN).Sleep(100);
        }

        sim.Keyboard
           .KeyPress(VirtualKeyCode.RETURN)
           .Sleep(1000)
           .ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4)
           .KeyPress(VirtualKeyCode.VK_N);
    }

    public void AnotherTest()
    {
        var sim = new InputSimulator();
        sim.Keyboard.KeyPress(VirtualKeyCode.SPACE);

        sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R)
           .Sleep(1000)
           .TextEntry("mspaint")
           .Sleep(1000)
           .KeyPress(VirtualKeyCode.RETURN)
           .Sleep(1000)
           .Mouse
           .LeftButtonDown()
           .MoveMouseToPositionOnVirtualDesktop(65535 / 2, 65535 / 2)
           .LeftButtonUp();

    }

    public void TestMouseMoveTo()
    {
        var sim = new InputSimulator();
        sim.Mouse
           .MoveMouseTo(0, 0)
           .Sleep(1000)
           .MoveMouseTo(65535, 65535)
           .Sleep(1000)
           .MoveMouseTo(65535 / 2, 65535 / 2);
    }
}
