using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UICore;
using UnityEngine.UI;
using System.Reflection;

/// <summary>自动编码</summary>
public class AutoCoding : Editor
{
    [MenuItem("GameObject/Tool/一键注册 _%#_ Q", false, -1)]//设置-1可以在最上一级显示,其他位置直接按顺序数
    public static void Coding()
    {
        GameObject select = Selection.activeGameObject;

        RawImage rImg = select.GetComponent<RawImage>();
        Image img = select.GetComponent<Image>();
        Button btn = select.GetComponent<Button>();
        Text text = select.GetComponent<Text>();

        Transform parent = null;
        Type baseUI = GetBaseUIType(select.transform, out parent);

        if (parent == null)
        {
            Debug.LogError("不存在BaseUI");
            return;
        }
        else
        {
            if (IsExistSameName(parent, select.transform))
                return;
            GetFile(baseUI.Name, text, rImg, img, btn, select.name, GetParentPath(select.transform, parent));
        }
    }

    /// <summary>是否已经存在相同名称,有则不注册</summary>
    private static bool IsExistSameName(Transform parent, Transform select)
    {
        Transform[] childTrans = (Transform[])LinqUtil.CustomWhere(parent.GetComponentsInChildren<Transform>(), 
            x => x.name == select.name);
        if (childTrans.Length> 1)
        {
            Debug.LogError("存在相同名称物体");
            return true;
        }
        return false;
    }

    private static Type GetBaseUIType(Transform selectTrans,out Transform parent)
    {
        parent = null;
        Transform[] parents = selectTrans.GetComponentsInParent<Transform>();
        List<Tuple<string, Transform>> pList = new List<Tuple<string, Transform>>();
        foreach (var item in parents)
        {
            Tuple<string, Transform> tuple = new Tuple<string,Transform>(item.name,item);
            pList.Add(tuple);
        }
        Type[] types = Assembly.GetAssembly(typeof(BaseUI)).GetTypes();
        foreach (var item in types)
        {
            Transform parentTrans = GetParent(pList, item.Name);
            parent = parentTrans;
            if (item.IsSubclassOf(typeof(BaseUI)) && parentTrans)
                return item;
        }
        return null;
    }

    private static Transform GetParent(List<Tuple<string, Transform>> pList, string name)
    {
        foreach (var item in pList)
        {
            if (item.item1.Contains(name))
                return item.item2;
        }
        return null;
    }

    /// <summary>获取父物体路径</summary>
    private static string GetParentPath(Transform trans, Transform parent = null)
    {
        return AnimationUtility.CalculateTransformPath(trans, parent);
    }

    /// <summary>找到Assets文件夹下该类</summary>
    private static void GetFile(string monoName, Text text, RawImage rImg, Image img, Button btn, string selectName, string parentPath)
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
                        AddMono(path, text, rImg, img, btn, selectName, parentPath);
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
    /// <summary>Text</summary>
    public const string textStr = "Text";
    /// <summary>"RawImage"</summary>
    public const string rawImageStr = "RawImage";
    /// <summary>"Image"</summary>
    public const string imgStr = "Image";
    /// <summary>"Button"</summary>
    public const string btnStr = "Button";
    /// <summary>"Transform"</summary>
    public const string transformStr = "Transform";
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

    public const string FindTheChild = " = GameTool.FindTheChild";

    /// <summary>添加脚本</summary>
    private static void AddMono(string foldePath, Text text, RawImage rImg, Image img, Button btn, string selectName, string path)
    {
        string[] lines = File.ReadAllLines(foldePath);
        AddLine(lines, text, rImg, img, btn, selectName, path);

        File.WriteAllLines(foldePath, lines);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void AddLine(string[] lines,Text text, RawImage rImg, Image img, Button btn, string selectName, string path)
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
                if (text != null && !IsHaveLine(lines, selectName + "text") && btn == null)
                {
                    string line = textStr + " " + selectName + "text";
                    lines[i - 1] += Environment.NewLine + "\t" + nonPublicStr + " " + line + ";";//i - 1在上面添加行
                }
            }
            if (lines[i].Contains(baseUIStr))
            {
                if (rImg != null && !IsHaveLine(lines, selectName + "rImg") && btn == null)
                {
                    string line = rawImageStr + " " + selectName + "rImg";
                    lines[i - 1] += Environment.NewLine + "\t" + nonPublicStr + " " + line + ";";//i - 1在上面添加行
                }
            }
            if (lines[i].Contains(baseUIStr))
            {
                if (btn != null && !IsHaveLine(lines, selectName + "btn"))
                {
                    string line = btnStr + " " + selectName + "btn";
                    lines[i - 1] += Environment.NewLine + "\t" + nonPublicStr + " " + line + ";";
                }
            }
            if (lines[i].Contains(baseUIStr))
            {
                if (img != null && !IsHaveLine(lines, selectName + "Img") && btn == null)
                {
                    string line = imgStr + " " + selectName + "Img";
                    lines[i - 1] += Environment.NewLine + "\t" + nonPublicStr + " " + line + ";";
                }
            }
            if (lines[i].Contains(baseUIStr))
            {
                if (!IsHaveLine(lines,selectName + "Trans") && btn == null && text == null && rImg == null && img == null)
                {
                    string line = transformStr + " " + selectName + "Trans";
                    lines[i - 1] += Environment.NewLine + "\t" + nonPublicStr + " " + line + ";";
                }
            }
            if (lines[i].Contains(InitUiOnAwakeStr))
            {
                if (text != null && !IsHaveLine(lines, selectName + "text" + " = GameTool") && btn == null)
                {
                    string line = selectName + "text" + GetTheChildComponent + textStr + ">" +
                    "(gameObject," + "\"" + path + "\"" + ");";
                    lines[i] += Environment.NewLine + "\t\t" + line;
                }
            }
            if (lines[i].Contains(InitUiOnAwakeStr))
            {
                if (rImg != null && !IsHaveLine(lines, selectName + "rImg" + " = GameTool") && btn == null)
                {
                    string line = selectName + "rImg" + GetTheChildComponent + rawImageStr + ">" +
                    "(gameObject," + "\"" + path + "\"" + ");";
                    lines[i] += Environment.NewLine + "\t\t" + line;
                }
            }
            if (lines[i].Contains(InitUiOnAwakeStr))
            {
                if (img != null && !IsHaveLine(lines, selectName + "Img" + " = GameTool") && btn == null)
                {
                    string line = selectName + "Img" + GetTheChildComponent + imgStr + ">" +
                    "(gameObject," + "\"" + path + "\"" + ");";
                    lines[i] += Environment.NewLine + "\t\t" + line;
                }
            }
            if (lines[i].Contains(InitUiOnAwakeStr))
            {
                if (btn != null && !IsHaveLine(lines, selectName + "btn" + " = GameTool"))
                {
                    string line = selectName + "btn" + GetTheChildComponent + btnStr + ">" +
                    "(gameObject," + "\"" + path + "\"" + ");";
                    lines[i] += Environment.NewLine + "\t\t" + line;
                }
            }
            if (lines[i].Contains(InitUiOnAwakeStr))
            {
                if (!IsHaveLine(lines, selectName + "Trans" + " = GameTool") && btn == null && text == null && rImg == null && img == null)
                {
                    string line = selectName + "Trans" + FindTheChild +
                    "(gameObject," + "\"" + path + "\"" + ");";
                    lines[i] += Environment.NewLine + "\t\t" + line;
                }
            }
            if (lines[i].Contains(RegistBtnsStr))
            {
                if (btn != null && !IsHaveLine(lines, selectName + "btn" + ".onClick"))
                {
                    lines[i] += Environment.NewLine + "\t\t" + selectName + "btn" + ".onClick.AddListener(() => { });";
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

