using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Auxiliary;

public class test : MonoBehaviour {

    public List<GameObject> list = new List<GameObject>();
    
    public AnimationCurve curve;
    private int aa = -1;
    private int bb = 10;


    public Transform cc;
    public RawImage rImg;
    public float[] dd;

    public int a = 3;

    public List<int> list1 = new List<int>();

    private List<int> list2 = new List<int>();
    private int index;

    private Color ff = Color.white;

    private AuxUpdate anxUpdate;

	// Use this for initialization
	void Start () {
        anxUpdate = AuxUpdate.Instance;
        anxUpdate.callback += AA;

        //Debug.LogError(ColorUtil.ColorHSB.GetColorHSB(new Color(0.4f, 1f, 0.84f, 1f)));
        //transform.IsChildOf(cc);
        //EventTriggerListener.Get(rImg).onClick = OnClick;
        //EventTriggerListener.Get(rImg, true, 0.2f).onDoubleClick = OnDoubleClick;
        ////Debug.Log(MathHelpr.GetMaxLength(dd));

        //bool aa = a == 0 ? true : a > 3 ? true : false;
        //Debug.Log(aa);

        //var xxxx = LinqUtil.OrderBy<GameObject,string,Vector3,string>(list.ToArray(), item => item.name,item=>item.transform.localScale);
        //foreach (var item in xxxx)
        //{
        //    Debug.Log(item);
        //}z
    }

    private void AA()
    {
        Debug.LogError(123);
    }

    private void GetFrameTexture()
    {
        string a = "123";
        StringUtil.Reverse(a);
    }

    public GameObject[] ccc;

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
            index++;
            list2.Add(index);
            if (list2.Count > 4)
            {
                list2.Remove(list2[0]);
            }
            for (int i = 0; i < list2.Count; i++)
            {
                Debug.Log(list2[i]);
            }

            //LinqUtil.Forward(list1);
            //for (int i = 0; i < list1.Count; i++)
            //{
            //    Debug.Log(list1[i]);
            //}
            //Dictionary<string,int> dic =  StringUtil.CountWords(d, "A");
            //foreach (var item in dic)
            //{
            //    Debug.Log(item);
            //}

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
            LinqUtil.Back(list1,1);
            for (int i = 0; i < list1.Count; i++)
            {
                Debug.Log(list1[i]);
            }
            //Debug.LogError(MathHelpr.BaseStringToNumber(d));
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

    public Transform sp;
    public float offset;
    public AlginDir dir;

    private void LateUpdate()
    {
        //sp.AlginScreen(dir, offset, Camera.main);
    }

    private void OnApplicationQuit()
    {

    }
}
