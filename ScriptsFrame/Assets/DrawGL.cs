using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class DrawGL : MonoBehaviour
{
    #region 选定线框截图
    /*
    public Color rColor = Color.green;
    [Range(0,1)]
    public float alpha;

    private Vector3 start = Vector3.zero;
    private Vector3 end = Vector3.zero;
    private Material rMat = null;
    private bool drawFlag = false;
    private Rect rect;
    private Texture2D cutImage;
    // Use this for initialization
    void Start()
    {
        rMat = new Material(Shader.Find("Sprites/Default"));//生成画线的材质
        rMat.hideFlags = HideFlags.HideAndDontSave;
        rMat.shader.hideFlags = HideFlags.HideAndDontSave;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            drawFlag = true;//如果鼠标左键按下 设置开始画线标志
            start = Input.mousePosition;//记录按下位置
            end = Input.mousePosition;//鼠标当前位置
        }
        else if (Input.GetMouseButtonUp(0))
            drawFlag = false;//如果鼠标左键放开 结束画线

        if (Input.GetMouseButtonDown(1))
            StartCoroutine(CutImage());
    }

    //绘制框选
    void OnPostRender()
    {//画线这种操作推荐在OnPostRender（）里进行 而不是直接放在Update，所以需要标志来开启
        if (drawFlag)
            end = Input.mousePosition;//鼠标当前位置

        GL.PushMatrix();//保存摄像机变换矩阵

        if (!rMat)
            return;

        rMat.SetPass(0);
        GL.LoadPixelMatrix();//设置用屏幕坐标绘图
        GL.Begin(GL.QUADS);
        GL.Color(new Color(rColor.r, rColor.g, rColor.b, alpha));//设置颜色和透明度，方框内部透明
        GL.Vertex3(start.x, start.y, 0);
        GL.Vertex3(end.x, start.y, 0);
        GL.Vertex3(end.x, end.y, 0);
        GL.Vertex3(start.x, end.y, 0);
        GL.End();
        GL.Begin(GL.LINES);
        GL.Color(rColor);//设置方框的边框颜色 边框不透明
        GL.Vertex3(start.x, start.y, 0);
        GL.Vertex3(end.x, start.y, 0);
        GL.Vertex3(end.x, start.y, 0);
        GL.Vertex3(end.x, end.y, 0);
        GL.Vertex3(end.x, end.y, 0);
        GL.Vertex3(start.x, end.y, 0);
        GL.Vertex3(start.x, end.y, 0);
        GL.Vertex3(start.x, start.y, 0);
        GL.End();
        GL.PopMatrix();//恢复摄像机投影矩阵
    }

    IEnumerator CutImage()
    {
        string date = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        //图片大小  
        cutImage = new Texture2D((int)(end.x - start.x), (int)(start.y - end.y), TextureFormat.RGB24, true);
        //坐标左下角为0  
        rect = new Rect((int)start.x, Screen.height - (int)(Screen.height - end.y), (int)(end.x - start.x), (int)(start.y - end.y));
        yield return new WaitForEndOfFrame();
        cutImage.ReadPixels(rect, 0, 0, true);
        cutImage.Apply();
        yield return cutImage;
        byte[] byt = cutImage.EncodeToPNG();
        //保存截图  
        File.WriteAllBytes(Application.streamingAssetsPath + "/CutImage" + date + ".png", byt);
        Debug.Log("Finish Capture Screenshot");
    }
    */
    #endregion

    #region 画曲线
    /*
    private Material rMat;
    private List<Vector3> lineInfo;
    private bool startDraw = false;
    Event e;

    void Start()
    {
        rMat = new Material(Shader.Find("Sprites/Default"));
        //初始化鼠标线段链表  
        lineInfo = new List<Vector3>();
    }

    void OnGUI()
    {
        e = Event.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (e == null)
            return;

        if (e.type == EventType.MouseDown)
            startDraw = true;
        if (e.type == EventType.MouseDrag)
        {
            if (startDraw == true)
            {
                //将每次鼠标经过的位置存储进链表  
                lineInfo.Add(Input.mousePosition);
            }
        }
        if (e.type == EventType.MouseUp)
        {
            startDraw = false;
            lineInfo.Clear();
        }
    }

    //GL的绘制方法系统回调  
    void OnPostRender()
    {
        if (!rMat)
            return;

        rMat.SetPass(0);
        //绘制2D图像  
        GL.LoadOrtho();
        //得到鼠标点信息总数量  
        GL.Begin(GL.LINES);
        //遍历鼠标点的链表  
        int size = lineInfo.Count;
        for (int i = 0; i < size - 1; i++)
        {
            Vector3 start = lineInfo[i];
            Vector3 end = lineInfo[i + 1];
            //绘制线  
            DrawLineFun(start.x, start.y, end.x, end.y);
        }
        //结束绘制  
        GL.End();
    }

    void DrawLineFun(float x1, float y1, float x2, float y2)
    {
        //绘制线段  
        GL.Vertex(new Vector3(x1 / Screen.width, y1 / Screen.height, 0));
        GL.Vertex(new Vector3(x2 / Screen.width, y2 / Screen.height, 0));
    }
    */
    #endregion

    #region 绘制多条线段，右键开始重新绘制，有一个问题就是第二次绘制的时候要第二笔才开始
    public Color rColor = Color.green;
    private Material rMat;
    private Vector3 pos1, pos2;
    private bool IsReady = false;
    private ArrayList pointList;
    private ArrayList breakpointList;
    private int index = 0;

    void Start()
    {
        rMat = new Material(Shader.Find("Sprites/Default"));
        pointList = new ArrayList();
        breakpointList = new ArrayList();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsReady)
            {
                pos1 = Input.mousePosition;
                pointList.Add(pos1);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            pos2 = Input.mousePosition;
            IsReady = true;
            pointList.Add(pos2);
        }
        if (Input.GetMouseButtonUp(1))
        {
            breakpointList.Add(pointList.Count - 1);
        }
    }
    void OnPostRender()
    {
        if (IsReady)
        {
            if (!rMat)
                return;

            GL.PushMatrix();
            rMat.SetPass(0);
            GL.LoadOrtho();
            for (int i = 0; i < pointList.Count - 1; i++)
            {
                for (int j = 0; j < breakpointList.Count; j++)
                {
                    if (i == (int)(breakpointList[j]))
                    {
                        i++;
                    }
                }
                GL.Begin(GL.LINES);
                GL.Color(rColor);
                GL.Vertex3(((Vector3)pointList[i]).x / Screen.width, ((Vector3)pointList[i]).y / Screen.height, ((Vector3)pointList[i]).z);
                if (i + 1 <= pointList.Count - 1)
                    GL.Vertex3(((Vector3)pointList[i + 1]).x / Screen.width, ((Vector3)pointList[i + 1]).y / Screen.height, ((Vector3)pointList[i + 1]).z);
                GL.End();
            }
            GL.PopMatrix();
        }
    }
    #endregion
}
