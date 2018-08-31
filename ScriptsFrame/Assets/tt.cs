using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        image.GetScreenPos(out pos);
        Debug.Log(pos);
        image.GetSpaceCorners(out poses);
        foreach (var item in poses)
        {
            Debug.Log(item);
        }

        for (int i = 0; i < btns.Length; i++)
        {
            int index = i;
            btns[index].onClick.AddListener(() =>
            {
                Debug.Log(btns[index].name);
            });
        }
        //poses = image.rectTransform.get

	}
	
    private Color pixelColor;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            btns[0].GetComponent<Image>().color = Color.clear;
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
