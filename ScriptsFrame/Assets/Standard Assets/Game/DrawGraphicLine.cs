using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawGraphicLine : MonoBehaviour {

    private MaskableGraphic graphic;
    private RectTransform rectTransform;
    public float detectDistance;
	// Use this for initialization
	void Awake () {
        graphic = GetComponent<MaskableGraphic>();
        rectTransform = graphic.rectTransform;

    }
	
	// Update is called once per frame
	void Update () {
        if (graphic == null)
            return;

	}

    private void OnDrawGizmos()
    {
        if (rectTransform == null)
            return;

        Gizmos.color = Color.red;
        Vector2 pos = transform.position;
  
        float x = rectTransform.sizeDelta.x / 2;
        float y = rectTransform.sizeDelta.y / 2;

        Gizmos.DrawLine(pos + new Vector2(x, 0), pos + new Vector2(detectDistance, 0) + new Vector2(x, 0));
        Gizmos.DrawLine(pos + new Vector2(0, y), pos + new Vector2(0, detectDistance) + new Vector2(0, y));
        Gizmos.DrawLine(pos - new Vector2(x, 0), pos - new Vector2(detectDistance, 0) - new Vector2(x, 0));
        Gizmos.DrawLine(pos - new Vector2(0, y), pos - new Vector2(0, detectDistance) - new Vector2(0, y));
    }
}
