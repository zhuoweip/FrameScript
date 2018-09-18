using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour {

    public List<GameObject> list = new List<GameObject>();
    public AnimationCurve curve;
    private int aa = -1;
    private int bb = 0;


    public Transform cc;
    public RawImage rImg;
    public float[] dd;
	// Use this for initialization
	void Start () {
        //transform.IsChildOf(cc);
        EventTriggerListener.Get(rImg).onClick = OnClick;
        EventTriggerListener.Get(rImg, true, 0.2f).onDoubleClick = OnDoubleClick;
        //Debug.Log(MathHelpr.GetMaxLength(dd));
    }

    private void OnClick(GameObject go)
    {
        Debug.Log("单击");
    }

    private void OnDoubleClick(GameObject go)
    {
        Debug.Log("双击");
    }

    public int c;
    public string d;
    public string f;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Dictionary<string,int> dic =  StringUtil.CountWords(d, "A");
            foreach (var item in dic)
            {
                Debug.Log(item);
            }

            //LinqUtil.ArrayPop(ref dd);
            //foreach (var item in dd)
            //{
            //    Debug.Log(item);
            //}
            //Debug.Log(a);
            //Debug.Log(MathHelpr.NumberToBaseString(c));
            //Debug.Log("a = " + LinqUtil.GetRandom(-5, 5,ref aa));
            //Debug.LogError("b = " + LinqUtil.GetRandom(0, 5, 2, ref bb));
            //Debug.Log(LinqUtil.GetRandom(curve));
            //list.Forward(2);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    Debug.Log(list[i].name);
            //}
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            Debug.LogError(MathHelpr.BaseStringToNumber(d));
            //list.Back(2);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    Debug.Log(list[i].name);
            //}
        }

        if (Input.GetMouseButton(0))
        {
            //trans.RotateToTarget(Input.mousePosition);
            //RotateYToTransform(trans, trans2.position);
        }
	}

    public RectTransform trans;
    public RectTransform trans2;

    
}
