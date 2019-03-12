using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorTool
{
    public static T[] FindAssets<T>(string search = "") where T : Object
    {
        return AssetDatabase.FindAssets(string.Format("{0} {1}{2}", search, "t:", typeof(T).Name)/*$"{search} t:{typeof(T).Name}"*/)
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<T>)
            .ToArray();
    }

    [MenuItem("Assets/Tool/对比两个文件夹图片")]
    public static void CheckTwoFolderTexName()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        if (objects.Length != 2)
        {
            Debug.LogError(string.Format("{0}", "文件夹个数不为2"));
            return;
        }

        List<string>[] lists = new List<string>[objects.Length];
        List<FileInfo>[] fileLists = new List<FileInfo>[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            lists[i] = new List<string>();
            fileLists[i] = new List<FileInfo>();

            DirectoryInfo directInfo = new DirectoryInfo(AssetDatabase.GetAssetPath(objects[i]));

            string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
            string[] ImageType = imgtype.Split('|');
            for (int j = 0; j < ImageType.Length; j++)
            {
                FileInfo[] files = directInfo.GetFiles(ImageType[j], SearchOption.AllDirectories);
                for (int k = 0; k < files.Length; k++)
                {
                    lists[i].Add(files[k].Name);
                    fileLists[i].Add(files[k]);
                    //Debug.Log(string.Format("{0}\n,{1}\n,{2}\n", files[k].DirectoryName, files[k].FullName, files[k].Name));
                }
            }
        }

        List<string> sameList = LinqUtil.CustomIntersect(lists[0], lists[1]);
        List<string> differenceList = LinqUtil.CustomExcept(lists[0], lists[1]);

        for (int i = 0; i < sameList.Count; i++)
            Debug.Log("sameList = " + sameList[i]);

        for (int i = 0; i < differenceList.Count; i++)
            Debug.Log("differenceList = " + differenceList[i]);

        /*如果加了这句，就可以将两个不同的名字改为相同的名字
        for (int i = 0; i < differenceList.Count; i++)
        {
            for (int j = 0; j < fileLists[0].Count; j++)
            {
                if (fileLists[0][j].Name == differenceList[i])
                {
                    string oldName = GetPathByFullName(fileLists[0][j].FullName);
                    AssetDatabase.RenameAsset(oldName, fileLists[1][j].Name);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }*/
    }

    /// <summary>
    /// AssetDatabase改名
    /// </summary>
    /// <param name="fullName"></param>
    /// <returns></returns>
    public static string GetPathByFullName(string fullName)
    {
        string name = fullName.Replace(@"\", "/").Replace(Application.dataPath, string.Empty);
        return "Assets" + name;
    }

    [MenuItem("Assets/Tool/移除图片前后空格")]
    public static void TrimTextureName()
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
    [MenuItem("GameObject/Tool/Copy Find Child Path _%#_ C", false, -1)]
    public static void CopyFindChildPath()
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
    public static void CameraCanvasAlign()
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
        Vector3 pos = camera.transform.position;
        camera.transform.position = new Vector3(pos.x, pos.y, -10);
        camera.orthographic = true;
        camera.orthographicSize = canvas.GetComponent<RectTransform>().sizeDelta.y / 2;
    }

    static T GetComponent<T>(GameObject[] objs) where T : Component
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

    /// <summary>
    /// RawImage 转 Image
    /// </summary>
    [MenuItem("GameObject/Tool/RawImage->Image ", false, -1)]
    public static void ReplaceRawImageToImage()
    {
        Dictionary<GameObject, string> dic = new Dictionary<GameObject, string>();
        GameObject[] gos = Selection.gameObjects;
        for (int i = 0; i < gos.Length; i++)
        {
            RawImage[] rImgs = gos[i].GetComponentsInChildren<RawImage>();
            for (int j = 0; j < rImgs.Length; j++)
            {
                dic.Add(rImgs[j].gameObject, AssetDatabase.GetAssetPath(rImgs[j].texture));
            }
        }
        foreach (var item in dic)
        {
            Undo.DestroyObjectImmediate(item.Key.GetComponent<RawImage>());
            Object newImg = AssetDatabase.LoadAssetAtPath(item.Value, typeof(Sprite));
            Undo.AddComponent<Image>(item.Key).sprite = newImg as Sprite;
        }
    }

    /// <summary>
    /// Image 转 RawImage
    /// </summary>
    [MenuItem("GameObject/Tool/Image->RawImage ", false, -1)]
    public static void ReplaceImageToRawImage()
    {
        Dictionary<GameObject, string> dic = new Dictionary<GameObject, string>();
        GameObject[] gos = Selection.gameObjects;
        for (int i = 0; i < gos.Length; i++)
        {
            Image[] rImgs = gos[i].GetComponentsInChildren<Image>();
            for (int j = 0; j < rImgs.Length; j++)
            {
                dic.Add(rImgs[j].gameObject, AssetDatabase.GetAssetPath(rImgs[j].sprite));
            }
        }
        foreach (var item in dic)
        {
            Undo.DestroyObjectImmediate(item.Key.GetComponent<Image>());
            Object newImg = AssetDatabase.LoadAssetAtPath(item.Value, typeof(Texture));
            Undo.AddComponent<RawImage>(item.Key).texture = newImg as Texture;
        }
    }

    /// <summary>
    /// 删除丢失脚本
    /// </summary>
    [MenuItem("GameObject/Tool/RemoveMissingScripts", false, -1)]
    public static void RemoveMissingScript()
    {
        Transform[] selectionObjs = Selection.GetTransforms(SelectionMode.Deep);
        for (int i = 0; i < selectionObjs.Length; i++)
        {
            Debug.Log("<color=blue>" + selectionObjs[i].name + "</color>");
            var gameObject = selectionObjs[i].gameObject;
            var components = gameObject.GetComponents<Component>();
            var serializeObject = new SerializedObject(gameObject);
            var prop = serializeObject.FindProperty("m_Component");

            int r = 0;
            for (int j = 0; j < components.Length; j++)
            {
                if (components[j] == null)
                {
                    prop.DeleteArrayElementAtIndex(j - r);
                    r++;
                }
            }
            serializeObject.ApplyModifiedProperties();
        }
    }
}
