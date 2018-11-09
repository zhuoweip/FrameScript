using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {

    private void OnGUI()
    {
        if (GUI.skin.label.fontSize != 400)
            GUI.skin.label.fontSize = 400;
        ShowFPS();
    }

    private int lastFpsIndex;
    private int fpsIndex;

    private void ShowFPS()
    {
        if (Input.GetKeyDown(KeyCode.F10) && fpsIndex == lastFpsIndex)
            fpsIndex = 1 - lastFpsIndex;
        else if (Input.GetKeyUp(KeyCode.F10) && fpsIndex != lastFpsIndex)
            lastFpsIndex = fpsIndex;

        if (fpsIndex == 0) return;
        GUILayout.Label(string.Format("FPS:{0}", 1 / Time.smoothDeltaTime));
    }
}
