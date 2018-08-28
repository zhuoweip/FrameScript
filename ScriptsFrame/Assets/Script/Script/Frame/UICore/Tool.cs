using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Tool : MonoBehaviour {

    public bool isRight;
    public Text text1;
    public Text text2;

    [SerializeField]
    public List<CustomData>  dialogTexs;

    public Transform leftPos;
    public Transform rightPos;
}

[Serializable]
public class CustomData
{
    public DataInfo[] dataInfo;
}

[Serializable]
public class DataInfo
{
    public bool isLeft;
    public Texture2D tex;
    public Texture2D detail;
}

