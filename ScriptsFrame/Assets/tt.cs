using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class tt : SerializedMonoBehaviour
{
    public List<List<int>> list = new List<List<int>>();
    public List<int[]> a = new List<int[]>();
    public int[,] aa;

    int index;
    void AA()
    {
        Debug.Log(index);
        index++;
    }

    private void Update()
    {
        AA();
    }

}
