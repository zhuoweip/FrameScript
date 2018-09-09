using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 旋转卡牌
/// </summary>
public class RotateCard : MonoBehaviour
{
    public Transform root; //Canvas
    public Image items;//一张50*100image
    Image[] item;
    public int length = 10;
    public float width = 35f;
    public float angl = 10;
    void Start()
    {
        init();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sort();
        }
    }

    void init()
    {
        item = new Image[length];
        for (int i = 0; i < length; i++)
        {
            item[i] = Instantiate(items) as Image;
            item[i].transform.parent = root;
            Debug.Log(item[i].rectTransform.position);
            item[i].rectTransform.localPosition = new Vector3(-(width * ((float)(length - 1) / 2 - i)), 0, 0);
            item[i].color = Color.white - (i * new Color(0, 0.1f, 0.1f, 0));
        }
    }
    void sort()
    {
        Debug.Log(Mathf.Tan(Mathf.Deg2Rad * angl));
        for (int i = 0; i < length; i++)
        {
            item[i].rectTransform.localPosition = new Vector3(-(width * ((float)(length - 1) / 2 - i)), -(width * ((float)(length - 1) / 2 - i)) / Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i)) + (1f / (Mathf.Tan(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i))) * (width * ((float)(length - 1) / 2 - i))), 0);
            item[i].rectTransform.eulerAngles = new Vector3(0, 0, angl * ((float)(length - 1) / 2 - i));
        }
    }

}