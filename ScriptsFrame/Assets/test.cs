using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public List<GameObject> list = new List<GameObject>();
    public AnimationCurve curve;
    private int aa = -1;
    private int bb = 0;
	// Use this for initialization
	void Start () {
		
	}
	

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("a = " + LinqUtil.GetRandom(-5, 5,ref aa));
            Debug.LogError("b = " + LinqUtil.GetRandom(0, 5, 2, ref bb));
            //Debug.Log(LinqUtil.GetRandom(curve));
            //list.Forward(2);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    Debug.Log(list[i].name);
            //}
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            list.Back(2);
            for (int i = 0; i < list.Count; i++)
            {
                Debug.Log(list[i].name);
            }
        }
	}
}
