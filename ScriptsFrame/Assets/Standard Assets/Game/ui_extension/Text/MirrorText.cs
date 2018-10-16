using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Text倒影
/// </summary>
public class MirrorText : BaseMeshEffect
{
    //距离，限制范围0-30
    [Range(0, 30)]
    public float distance;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0)
        {
            return;
        }
        List<UIVertex> vertexs = new List<UIVertex>();
        vh.GetUIVertexStream(vertexs);
        UIVertex vt;
        int count = vertexs.Count;
        float miny = vertexs[0].position.y;
        float maxy = vertexs[0].position.y;
        for (int i = 1; i < count; i++)
        {
            if (vertexs[i].position.y < miny)
            {
                miny = vertexs[i].position.y;
            }
            else if (vertexs[i].position.y > maxy)
            {
                maxy = vertexs[i].position.y;
            }
        }
        float uiElementHeight = maxy - miny;
        float mirrorMinY = -maxy + 2 * miny - distance;
        Color32 top = GetComponent<Text>().color;
        Color32 bottom = new Color(top.r, top.g, top.b, 0);
        for (int i = 0; i < count; i++)
        {
            vt = vertexs[i];
            vertexs.Add(vt);
            Vector3 v = vt.position;
            v.y = -v.y + 2 * miny - distance;
            vt.position = v;

            //透明度效果
            vt.color = Color32.Lerp(bottom, top, (vt.position.y - mirrorMinY) / uiElementHeight);
            vertexs[i + count] = vt;
        }
        vh.Clear();
        vh.AddUIVertexTriangleStream(vertexs);
    }
}