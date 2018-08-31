using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UICore;
using UnityEngine.UI;

/// <summary>自动编码</summary>
public class AutoCoding : Editor
{
    [MenuItem("GameObject/Tool/一键注册 _%#_ Q",false,-1)]//设置-1可以在最上一级显示
    public static void Coding()
    {
        GameObject select = Selection.activeGameObject;
        Component[] cps = select.GetComponents<Component>();//获取身上所有组件

        RawImage rImg = GetCompoentByComponent<RawImage>(cps);
        Image img = GetCompoentByComponent<Image>(cps);
        Button btn = GetCompoentByComponent<Button>(cps);

        MonoBehaviour[] alltypes = select.GetComponentsInParent<MonoBehaviour>();
        MonoBehaviour baseUI = GetBaseUIType(alltypes);
        ////Debug.Log(baseUI.GetType().Name);
        if (baseUI == null)
        {
            Debug.LogError("不存在BaseUI");
            return;
        }

        //if (IsExistSameName(baseUI.transform, select.transform))
        //    return;

        GetFile(baseUI.GetType().Name, rImg, img, btn, select.name, GetParentPath(select.transform) + select.name);
    }

    /// <summary>是否已经存在相同名称,有则不注册</summary>
    private static bool IsExistSameName(Transform parent, Transform select)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name == select.name)//这里不能用Equal
            {
                Debug.LogError("存在相同名称物体");
                return true;
            }
            IsExistSameName(parent.GetChild(i), select);//有的话找一轮寻找即结束
            return false;
        }
        return false;
    }


    /// <summary>获取身上特定组件</summary>
    private static T GetCompoentByComponent<T>(Component[] cps) where T : Component
    {
        if (cps == null)
            return null;
        foreach (var item in cps)
        {
            if (item.GetType() == typeof(T))
                return item as T;
        }
        return null;
    }

    /// <summary>获取父物体上面继承baseUI的类</summary>
    private static MonoBehaviour GetBaseUIType(MonoBehaviour[] type)
    {
        foreach (var item in type)
        {
            if (item.GetType().IsSubclassOf(typeof(BaseUI)))
                return item;
        }
        return null;
    }

    private static void GetParentPath(Transform trans, List<string> list)
    {
        if (trans.parent != null && GetBaseUIType(trans.GetComponents<MonoBehaviour>()) == null)
        {
            list.Add(trans.parent.name);
            GetParentPath(trans.parent, list);
        }
    }

    /// <summary>获取父物体路径</summary>
    private static string GetParentPath(Transform trans)
    {
        List<string> list = new List<string>();
        GetParentPath(trans, list);
        return GetAddPath(list);
    }

    /// <summary>拼接路径字符串</summary>
    private static string GetAddPath(List<string> list)
    {
        string path = string.Empty;
        list.RemoveAt(list.Count - 1);
        for (int i = list.Count - 1; i >= 0; i--)
            path += list[i] + "/";
        return path;
    }

    /// <summary>找到Assets文件夹下该类</summary>
    private static void GetFile(string monoName, RawImage rImg, Image img, Button btn, string selectName, string parentPath)
    {
        string fullPath = Application.dataPath + "/Script" + "/";//加文件夹名减少计算
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".cs"))
                {
                    if (files[i].Name.Contains(monoName))
                    {
                        string path = files[i].FullName.Replace(@"\", "/");
                        int index = path.IndexOf("Assets");
                        path = path.Substring(index, path.Length - index);
                        AddMono(path, rImg, img, btn, selectName, parentPath);
                    }
                    continue;
                }
            }
        }
    }

    /// <summary>"private"</summary>
    public const string nonPublicStr = "private";
    /// <summary>"public"</summary>
    public const string publicStr = "public";
    /// <summary>"RawImage"</summary>
    public const string rawImageStr = "RawImage";
    /// <summary>"Image"</summary>
    public const string imgStr = "Image";
    /// <summary>"Button"</summary>
    public const string btnStr = "Button";
    /// <summary>"using UnityEngine.UI;"</summary>
    public const string usingUIStr = "using UnityEngine.UI;";
    /// <summary>"BaseUI"</summary>
    public const string baseUIStr = "void InitUiOnAwake()";
    /// <summary>"base.InitUiOnAwake();"</summary>
    public const string InitUiOnAwakeStr = "base.InitUiOnAwake();";
    /// <summary>"base.RegistBtns();"</summary>
    public const string RegistBtnsStr = "base.RegistBtns();";
    /// <summary>"GameToolGetTheChildComponent"</summary>
    public const string GetTheChildComponent = " = GameTool.GetTheChildComponent<";

    /// <summary>添加脚本</summary>
    private static void AddMono(string foldePath, RawImage rImg, Image img, Button btn, string selectName, string path)
    {
        string[] lines = File.ReadAllLines(foldePath);
        AddLine(lines, rImg, img, btn, selectName, path);

        File.WriteAllLines(foldePath, lines);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void AddLine(string[] lines, RawImage rImg, Image img, Button btn, string selectName, string path)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (rImg != null)
            {
                if (!IsHaveLine(lines, usingUIStr))
                {
                    lines[i] += Environment.NewLine + usingUIStr;
                }
            }
            if (lines[i].Contains(baseUIStr))
            {
                if (rImg != null && !IsHaveLine(lines, selectName + "rImg"))
                {
                    string line = rawImageStr + " " + selectName + "rImg";
                    lines[i - 1] += Environment.NewLine + nonPublicStr + " " + line + ";";//i - 1在上面添加行
                }
            }
            if (lines[i].Contains(baseUIStr))
            {
                if (btn != null && !IsHaveLine(lines, selectName + "btn"))
                {
                    string line = btnStr + " " + selectName + "btn";
                    lines[i - 1] += Environment.NewLine + nonPublicStr + " " + line + ";";
                }
            }
            if (lines[i].Contains(baseUIStr))
            {
                if (img != null && !IsHaveLine(lines, selectName + "Img"))
                {
                    string line = imgStr + " " + selectName + "Img";
                    lines[i - 1] += Environment.NewLine + nonPublicStr + " " + line + ";";
                }
            }
            if (lines[i].Contains(InitUiOnAwakeStr))
            {
                if (rImg != null && !IsHaveLine(lines, selectName + "rImg" + " = GameTool"))
                {
                    string line = selectName + "rImg" + GetTheChildComponent + rawImageStr + ">" +
                    "(gameObject," + "\"" + path + "\"" + ");";
                    lines[i] += Environment.NewLine + line;
                }
            }
            if (lines[i].Contains(InitUiOnAwakeStr))
            {
                if (img != null && !IsHaveLine(lines, selectName + "Img" + " = GameTool"))
                {
                    string line = selectName + "Img" + GetTheChildComponent + imgStr + ">" +
                    "(gameObject," + "\"" + path + "\"" + ");";
                    lines[i] += Environment.NewLine + line;
                }
            }
            if (lines[i].Contains(InitUiOnAwakeStr))
            {
                if (btn != null && !IsHaveLine(lines, selectName + "btn" + " = GameTool"))
                {
                    string line = selectName + "btn" + GetTheChildComponent + btnStr + ">" +
                    "(gameObject," + "\"" + path + "\"" + ");";
                    lines[i] += Environment.NewLine + line;
                }
            }
            if (lines[i].Contains(RegistBtnsStr))
            {
                if (btn != null && !IsHaveLine(lines, selectName + "btn" + ".onClick"))
                {
                    lines[i] += Environment.NewLine + selectName + "btn" + ".onClick.AddListener(() => { });";
                }
            }
        }
    }

    /// <summary>是否已经存在代码</summary>
    private static bool IsHaveLine(string[] lines, string line)
    {
        foreach (var item in lines)
        {
            if (item.Contains(line))
            {
                return true;
            }
        }
        return false;
    }
}

public enum ComponentType
{
    /// <summary>引用</summary>
    Using,
    /// <summary>RawImage私有变量</summary>
    RawImage,
    /// <summary>Image私有变量</summary>
    Image,
    /// <summary>Button私有变量</summary>
    Button,
    /// <summary>RawImage引用</summary>
    RawImageInitUi,
    /// <summary>Image引用</summary>
    ImageInitUi,
    /// <summary>Button引用</summary>
    ButtonInitUi,
    /// <summary>注册按钮事件</summary>
    RegistBtns,
}

