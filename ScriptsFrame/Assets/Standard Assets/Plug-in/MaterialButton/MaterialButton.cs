using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MaterialButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    private Vector2 point;
    private Material mat;
    private float radius = 0;
    private bool isUp;
    public Color color;

    public MaterialButton()
    {
        color = new Color(1, 1, 1, 200f / 255f);
    }

    void Awake()
    {
        mat = new Material(Shader.Find("Custom/Wave"));
        if (GetComponent<Image>() != null)
            GetComponent<Image>().material = mat;
        else if (GetComponent<RawImage>() != null) 
            GetComponent<RawImage>().material = mat;
    }

    IEnumerator normal()
    {
        while (true)
        {
            if (isUp) break;

            radius *= 1.05f;
            mat.SetFloat("_Radius", radius);

            Vector4 v4 = new Vector4(point.x, point.y, 0, 0);
            mat.SetVector("_Point1", v4);

            if (radius == 0) break;

            yield return null;
        }
    }

    IEnumerator fast()
    {
        float timer = color.a;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            radius *= 1.5f;
            mat.SetFloat("_Radius", radius);

            Vector4 col = new Vector4(1, 1, 1, timer);
            mat.SetVector("_Color", col);

            yield return null;
        }
        isUp = false;
        radius = 0;
        mat.SetFloat("_Radius", radius);
    }

    #region Interface

    void IPointerDownHandler.OnPointerDown(PointerEventData data)
    {
        StopAllCoroutines();
        isUp = false;
        radius = 500;
        point = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        Vector4 col = color;//new Vector4(1, 0, 0, 1);
        mat.SetVector("_Color", col);

        StartCoroutine(normal());
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData data)
    {
        isUp = true; //to break normal coroutine
        StartCoroutine(fast());
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData data)
    {
        StartCoroutine(fast());
    }

#endregion 

}
