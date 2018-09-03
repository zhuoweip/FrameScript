using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using System.Linq;

public class tt : MonoBehaviour {

    int[] a = new int[5] { 1, 2, 3, 4, 5 };
    int[] b = new int[5] { 2, 3, 1, 4, 5 };


    public Image image;
    public Image _resultImage;
    private Vector3 pos;
    private Vector3[] poses;
    public Canvas canvas;

    public Button[] btns;

    // Use this for initialization
    void Start () {
        //image.GetScreenPos(out pos);
        //Debug.Log(pos);
        //image.GetSpaceCorners(out poses);
        //foreach (var item in poses)
        //{
        //    Debug.Log(item);
        //}

        //for (int i = 0; i < btns.Length; i++)
        //{
        //    int index = i;
        //    btns[index].onClick.AddListener(() =>
        //    {
        //        Debug.Log(btns[index].name);
        //    });
        //}
        //poses = image.rectTransform.get

    }




    public int[] values = new int[6] { -1, -1, -1, -1, -1, -1 };
    int GetRandomIndex(int userindex)
    {
        int modelindex = Random.Range(0, 6);
        if (values[userindex] != modelindex)//确保此次取得与原先的值不一样
        {
            //返回的是一个类似数组
            var a = from k in values where k == modelindex select k;
            if (a.ToArray().Length <= 1)  //Linq确保数组内只能有2个值相同，改叭改叭允许有几个值重复你说了算哈
            {
                return modelindex;
            }
        }
        return GetRandomIndex(userindex);
    }
	


    private Color pixelColor;

    public int index;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(GetRandomIndex(index));
            //btns[0].GetComponent<Image>().color = Color.clear;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            btns[1].GetComponent<Image>().color = Color.clear;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            btns[2].GetComponent<Image>().color = Color.clear;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            btns[0].GetComponent<Image>().color = Color.black;
            btns[1].GetComponent<Image>().color = Color.black;
            btns[2].GetComponent<Image>().color = Color.black;
        }

        //image.GetRectPixelColor(out pixelColor);
        //_resultImage.color = pixelColor;
    }
}
