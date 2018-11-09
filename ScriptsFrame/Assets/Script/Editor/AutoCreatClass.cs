using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UICore;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class AutoCreatClass : Editor {

    [MenuItem("GameObject/Tool/创建类 _%#_ P", false, -1)]//跟自动化构建代码距离较远，以免误触
    public static void JudgeHaveClass()
    {
        GameObject select = Selection.activeGameObject;
        string name = select.name;
        if (IsHaveSelectUIType(name))
            return;
        else
        {
            className = name;
            GetPath(name);
        }
    }

    private static string className;

    private static void CreateScript(string path, string msg)
    {
        StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8);
        sw.Write(msg);
        sw.Flush();
        sw.Close();
        Debug.Log("ok----------");
        AssetDatabase.Refresh();
        RegistGameDefine();
        AssetDatabase.Refresh();
    }

    private static void RegistGameDefine()
    {
        GetFile("GameDefine");
    }

    private static void GetFile(string monoName)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath);
        GetAllFiles(dir, "UICore", ref gameDefinePath);
        string fullPath = gameDefinePath;

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
                        //Debug.Log("path = " + path);
                        ChangMono(path);
                    }
                    continue;
                }
            }
        }
    }

    private const string EUiIdStr = "public enum EUiId";
    private static void ChangMono(string foldePath)
    {
        string[] lines = File.ReadAllLines(foldePath);
        List<int> indexList = new List<int>();
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("}"))
            {
                indexList.Add(i);
            }
        }
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == indexList[0])
            {
                lines[indexList[0] - 1] += Environment.NewLine + "\t\t" + className + ",";
            }
            else if (i == indexList[1])
            {
                lines[indexList[1] - 1] += Environment.NewLine + "\t\t\t" +  "{ EUiId." + className + "," + "\"UIPrefab/\"" + "+" + "\"" + className + "\"" + "},";
            }
        }
        File.WriteAllLines(foldePath, lines);
        AssetDatabase.SaveAssets();
    }

    private static StringBuilder GetClassInfo(string name)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using UICore;");
        sb.AppendLine("using UnityEngine");
        sb.AppendLine("using UnityEngine.UI;");
        sb.AppendLine();

        sb.AppendFormat("public class {0} : BaseUI", name);
        sb.AppendLine();
        sb.AppendLine("{");

        sb.AppendLine("\t" + "protected override void InitUiOnAwake()");
        sb.AppendLine("\t" + "{");
        sb.AppendLine("\t\t" + "base.InitUiOnAwake();");
        sb.AppendLine();
        sb.AppendLine("\t" + "}");

        sb.AppendLine("\t" + "protected override void InitDataOnAwake()");
        sb.AppendLine("\t" + "{");
        sb.AppendLine();
        sb.AppendLine("\t" + "}");

        sb.AppendLine("\t" + "protected override void RegistBtns()");
        sb.AppendLine("\t" + "{");
        sb.AppendLine("\t\t" + " base.RegistBtns();");
        sb.AppendLine();
        sb.AppendLine("\t" + "}");

        sb.AppendLine();
        sb.AppendLine("}");

        return sb;

    }

    private static void GetPath(string name)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath);
        GetAllFiles(dir, "View",ref classPath);
        Debug.Log(classPath);
        CreateScript(classPath + "/" + name + ".cs", GetClassInfo(name).ToString());
    }

    static string classPath;
    static string gameDefinePath;

    private static void GetAllFiles(DirectoryInfo dir,string foldName, ref string dataPath)
    {
        FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();
        foreach (var i in fileInfo)
        {
            if (i is DirectoryInfo)
            {
                string[] strArrary = i.FullName.Split('\\');
                if (strArrary[strArrary.Length - 1] == foldName)
                {
                    string fullName = i.FullName;
                    int index = fullName.IndexOf("Assets");
                    string path = fullName.Substring(index, fullName.Length - index).Replace("\\", "/");
                    dataPath = path;
                    return;
                }
                else
                {
                    GetAllFiles((DirectoryInfo)i, foldName, ref dataPath);
                }
            }
        }
    }

    private static bool IsHaveSelectUIType(string name)
    {
        Type[] types = Assembly.GetAssembly(typeof(BaseUI)).GetTypes();
        foreach (var item in types)
        {
            if (item.Name == name)
                return true;
        }
        return false;
    }

}
