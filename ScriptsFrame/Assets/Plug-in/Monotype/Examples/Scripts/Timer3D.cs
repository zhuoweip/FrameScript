using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MonotypeUnityTextPlugin;

public class Timer3D : MonoBehaviour {

    public MP3DTextComponent textTimer;
    public float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f3");

        textTimer.Text = minutes + ":" + seconds;
    }
}
