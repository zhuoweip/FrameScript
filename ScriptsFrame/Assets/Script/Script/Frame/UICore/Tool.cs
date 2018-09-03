using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Tool : SerializedMonoBehaviour {

    public bool isRight;
    public Text text1;
    public Text text2;

    public List<DataInfo[]> dialogTexs = new List<DataInfo[]>();
    public Dictionary<int, string> dic;

    public Transform leftPos;
    public Transform rightPos;
}

public class DataInfo
{
    public bool isLeft;
    public Texture2D tex;
    public Texture2D detail;
}

