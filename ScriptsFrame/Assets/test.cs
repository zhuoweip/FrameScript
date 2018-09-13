using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public List<GameObject> list = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            list.Forward(2);
            for (int i = 0; i < list.Count; i++)
            {
                Debug.Log(list[i].name);
            }
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
