using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UICore;

public class tt : SerializedMonoBehaviour
{
    public List<List<int>> list = new List<List<int>>();
    public List<int[]> a = new List<int[]>();
    public int[,] aa;

    int index;

    private void Start()
    {
        
    }

    void AA()
    {
        //Debug.Log(index);
        index++;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AudioManager.Instance.PlayBg(MusicType.NormalBg); 
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            AudioManager.Instance.PlayAudio().OnComplete(()=>
            {
                Debug.Log(22);
            });
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            AudioManager.Instance.PlayAudio(MusicType.ClickRight,1,true).OnComplete(()=>
            {
                Debug.Log(11); 
            });
        }
    }
}
