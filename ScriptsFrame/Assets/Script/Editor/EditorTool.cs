using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;

public class EditorTool : Editor {

    [MenuItem("Assets/Tool/对比两个文件夹图片")]
    static void CheckTwoFolderTexName()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        if (objects.Length != 2)
        {
            Debug.LogError(string.Format("{0}", "文件夹个数不为2"));
            return;
        }
        
        List<string>[] lists = new List<string>[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            lists[i] = new List<string>();
            DirectoryInfo directInfo = new DirectoryInfo(AssetDatabase.GetAssetPath(objects[i]));
            
            string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
            string[] ImageType = imgtype.Split('|');
            for (int j = 0; j < ImageType.Length; j++)
            {
                FileInfo[] files = directInfo.GetFiles(ImageType[j], SearchOption.AllDirectories);
                for (int k = 0; k < files.Length; k++)
                {
                    lists[i].Add(files[k].Name);
                    //Debug.Log(string.Format("{0}\n,{1}\n,{2}\n", files[k].DirectoryName, files[k].FullName, files[k].Name));
                }
            }
        }

        List<string> sameList = LinqUtil.CustomIntersect(lists[0],lists[1]);
        List<string> differenceList = LinqUtil.CustomExcept(lists[0], lists[1]);

        for (int i = 0; i < sameList.Count; i++)
            Debug.Log("sameList = " + sameList[i]);

        for (int i = 0; i < differenceList.Count; i++)
            Debug.Log("differenceList = " + differenceList[i]);
    }

    [MenuItem("Assets/Tool/移除图片前后空格")]
    static void TrimTextureName()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        for (int i = 0; i < objects.Length; i++)
        {
            DirectoryInfo directInfo = new DirectoryInfo(AssetDatabase.GetAssetPath(objects[i]));

            string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
            string[] ImageType = imgtype.Split('|');
            for (int j = 0; j < ImageType.Length; j++)
            {
                FileInfo[] files = directInfo.GetFiles(ImageType[j], SearchOption.AllDirectories);
                for (int k = 0; k < files.Length; k++)
                {
                    string texName = files[k].Name.TrimStart().TrimEnd(); ;
                    files[k].MoveTo(texName);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
    }

    /// <summary>快捷键 Ctrl+Shift + C ===>复制选中两个游戏对象之间的查找路径x ：transform.FindChild("路径x")
    /// </summary>
    [MenuItem("GameObject/Tool/Copy Find Child Path _%#_ C",false,-1)]
    static void CopyFindChildPath()
    {
        //Object[] objAry = Selection.objects;
        //if (objAry.Length == 2)
        //{
        //    GameObject gmObj0 = (GameObject)objAry[0];
        //    GameObject gmObj1 = (GameObject)objAry[1];
        //    List<Transform> listGameParent0 = new List<Transform>(gmObj0.transform.GetComponentsInParent<Transform>(true));
        //    List<Transform> listGameParent1 = new List<Transform>(gmObj1.transform.GetComponentsInParent<Transform>(true));
        //    System.Text.StringBuilder strBd = new System.Text.StringBuilder("");
        //    if (listGameParent0.Contains(gmObj1.transform))
        //    {
        //        int startIndex = listGameParent0.IndexOf(gmObj1.transform);
        //        Debug.Log(startIndex);
        //        for (int i = startIndex; i >= 0; i--)
        //        {
        //            if (i != startIndex)
        //                strBd.Append(listGameParent0[i].gameObject.name).Append(i != 0 ? "/" : "");
        //        }
        //    }

        //    if (listGameParent1.Contains(gmObj0.transform))
        //    {
        //        int startIndex = listGameParent1.IndexOf(gmObj0.transform);
        //        for (int i = startIndex; i >= 0; i--)
        //        {
        //            if (i != startIndex)
        //                strBd.Append(listGameParent1[i].gameObject.name).Append(i != 0 ? "/" : "");
        //        }
        //    }
        //    CopyStrInfo(strBd.ToString());
        //}

        /*------------上面一种方法太复杂,直接用原生API,Selection是无法判断选择的父子级关系的,所以要正反判断两次-----------------------*/
        string str = string.Empty;
        GameObject[] gameObjs = Selection.gameObjects;
        if (gameObjs.Length == 2)
        {
            Transform target = gameObjs[0].transform;
            Transform root = gameObjs[1].transform;
            if (IsChild(target, root))
                str = AnimationUtility.CalculateTransformPath(target, root);
            if (IsChild(root, target))
                str = AnimationUtility.CalculateTransformPath(root, target);
        }
        else
            Debug.LogError("所选对象超过2个");
        CopyStrInfo(str);
    }

    private static bool IsChild(Transform target, Transform root)
    {
        List<Transform> transList = LinqUtil.ToList(root.GetComponentsInChildren<Transform>());
        if (transList.Contains(target))
            return true;
        return false;
    }

    private static void CopyStrInfo(string str)
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = "\"" + str + "\"";// "hello world";
        textEditor.OnFocus();
        textEditor.Copy();
        string colorStr = str.Length > 0 ? "<color=green>" : "<color=red>";
        if (string.IsNullOrEmpty(str))
            Debug.Log(colorStr + "选中对象没有父子级关系" + "</color>");
        else
        {
            Debug.Log(colorStr + "复制：【\"" + str + "\"】" + "</color>");
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);//无bom编码
            StreamWriter sw = new StreamWriter("E://UGUI_Temp.txt", false, utf8WithoutBom);
            sw.Write("transform.Find(" + str + ")");
            sw.Flush();
            sw.Close();
            sw.Dispose();
            //System.Diagnostics.Process.Start("notepad++", "E://UGUI_Temp.txt");
        }
    }


    [MenuItem("GameObject/Tool/对齐相机和Canvas ", false, -1)]
    static void CameraCanvasAlign()
    {
        GameObject[] gameObjs = Selection.gameObjects;
        if (gameObjs.Length != 2)
        {
            Debug.LogError("selectionObjs is Not 2");
            return;
        }
        Camera camera = GetComponent<Camera>(gameObjs);
        Canvas canvas = GetComponent<Canvas>(gameObjs);
        if (!camera || !canvas)
        {
            Debug.LogError("No camera or canvas");
            return;
        }
 
        camera.transform.position = canvas.transform.position;
        camera.orthographic = true;
        camera.orthographicSize = canvas.GetComponent<RectTransform>().sizeDelta.y / 2;
    }

    static T GetComponent<T>(GameObject[] objs) where T:Component
    {
        T t = null;
        foreach (var item in objs)
        {
            t = item.GetComponent<T>();
            if (t != null)
            {
                return t;
            }
        }
        return t;
    }
}
