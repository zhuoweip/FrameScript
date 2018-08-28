﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

#region GameTool
public static class GameTool
{
    /// <summary>给子物体添加父对象</summary>
    public static void AddChildToParent(Transform parentTrans, Transform childTrans)
    {
        childTrans.parent = parentTrans;
        childTrans.localPosition = Vector3.zero;
        childTrans.localScale = Vector3.one;
    }

    /// <summary>查找子物体的Transform</summary>
    public static Transform FindTheChild(GameObject goParent, string childName)
    {
        Transform searchTrans = goParent.transform.Find(childName);
        if (searchTrans == null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childName);
                if (searchTrans != null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }

    /// <summary>查找子物体的GameObject</summary>
    public static GameObject FindTheChildGameObject(GameObject goParent, string childName)
    {
        return FindTheChild(goParent, childName).gameObject;
    }

    /// <summary>获取子物体上面的组件</summary>
    public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            return searchTrans.GetComponent<T>();
        }
        else
        {
            Debug.LogError("not exist this component");
            return null;
        }
    }

    /// <summary>获取激活子物体数量</summary>
    public static int GetActiveChildCount(this Transform transform)
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
                count++;
        }
        return count;
    }

    /// <summary>获取第一个子物体的组件(是否包括非激活的)</summary>
    public static T[] GetComponentsInFirstHierarchyChildren<T>(this Transform transform, bool includeInactive = true) where T : Component
    {
        List<T> components = new List<T>();
        foreach (Transform child in transform)
        {
            if (!includeInactive && !child.gameObject.activeSelf)
                continue;

            T component = child.GetComponent<T>();
            if (component != null)
                components.Add(component);
        }
        return components.ToArray();
    }

    /// <summary>获取子物体目录路径</summary>
    public static string GetPath(this Transform transform)
    {
        string path = transform.name;

        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }

    /// <summary>获取子物体下标</summary>
    public static int IndexInParent(this Transform transform)
    {
        if (transform.parent == null)
            return -1;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform)
                return i;
        }
        return -1;
    }

    /// <summary>设置子物体激活状态</summary>
    public static void SetActiveInChildren(this Transform transform, bool value)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(value);
        }
    }

    /// <summary>比较是否相等</summary>
    public static bool EqualsInHierarchy(this Transform transform, Transform other)
    {
        if (transform.childCount != other.childCount)
            return false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name != other.GetChild(i).name)
                return false;
            else if (!EqualsInHierarchy(transform.GetChild(i), other.GetChild(i)))
                return false;
        }
        return true;
    }

    /// <summary>删除所有子物体</summary>
    public static void DestroyChildren(this Transform transform)
    {
        for (int i = transform.childCount; i >= 0; i--)
            UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject);
    }

    /// <summary>设置子物体层级</summary>
    public static void SetChildrenLayer(Transform transform, LayerMask layer)
    {
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.layer = layer;
            SetChildrenLayer(childTransform, layer);
        }
    }

    public static T InvokeConstructor<T>(this Type type, Type[] paramTypes = null, object[] paramValues = null)
    {
        return (T)type.InvokeConstructor(paramTypes, paramValues);
    }

    public static object InvokeConstructor(this Type type, Type[] paramTypes = null, object[] paramValues = null)
    {
        if (paramTypes == null || paramValues == null)
        {
            paramTypes = new Type[] { };
            paramValues = new object[] { };
        }

        var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);

        return constructor.Invoke(paramValues);
    }

    public static T Invoke<T>(this object o, string methodName, params object[] args)
    {
        var value = o.Invoke(methodName, args);
        if (value != null)
        {
            return (T)value;
        }

        return default(T);
    }

    public static T Invoke<T>(this object o, string methodName, Type[] types, params object[] args)
    {
        var value = o.Invoke(methodName, types, args);
        if (value != null)
        {
            return (T)value;
        }

        return default(T);
    }

    public static object Invoke(this object o, string methodName, params object[] args)
    {
        Type[] types = new Type[args.Length];
        for (int i = 0; i < args.Length; i++)
            types[i] = args[i] == null ? null : args[i].GetType();

        return o.Invoke(methodName, types, args);
    }

    public static object Invoke(this object o, string methodName, Type[] types, params object[] args)
    {
        var type = o is Type ? (Type)o : o.GetType();
        var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, types, null);
        if (method == null)
            method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        return method.Invoke(o, args);
    }

    public static T GetFieldValue<T>(this object o, string name)
    {
        var value = o.GetFieldValue(name);
        if (value != null)
        {
            return (T)value;
        }

        return default(T);
    }

    public static object GetFieldValue(this object o, string name)
    {
        var field = o.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != null)
        {
            return field.GetValue(o);
        }

        return null;
    }

    public static void SetFieldValue(this object o, string name, object value)
    {
        var field = o.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != null)
        {
            field.SetValue(o, value);
        }
    }

    public static T GetPropertyValue<T>(this object o, string name)
    {
        var value = o.GetPropertyValue(name);
        if (value != null)
        {
            return (T)value;
        }

        return default(T);
    }

    public static object GetPropertyValue(this object o, string name)
    {
        var property = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (property != null)
        {
            return property.GetValue(o, null);
        }

        return null;
    }

    public static void SetPropertyValue(this object o, string name, object value)
    {
        var property = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (property != null)
        {
            property.SetValue(o, value, null);
        }
    }
}
#endregion

#region Linq拓展
public static class LinqUtil
{
    #region SelectAction_Test
    //private static bool SelectRayCast(RawImage rImg)
    //{
    //    return rImg.raycastTarget = true;
    //}

    //public static void SelectRimg()
    //{
    //    List<RawImage> list = new List<RawImage>();
    //    var select =  Selectarray<RawImage>(list, SelectRayCast);
    //}
    #endregion

    #region CustomIntersect_Test
    //static string[] AA = new string[5] { "1", "2", "3", "4", "5" };
    //static string[] BB = new string[5] { "1", "2", "3", "4", "6" };
    //static string[] CC = (string[])LinqUtil.CustomIntersect<string>(AA, BB);
    //static string[] DD = (string[])LinqUtil.CustomExcept(AA, BB);
    //static string[] EE = (string[])LinqUtil.CustomExcept(BB, AA);
    //static string[] GG = (string[])LinqUtil.CustomWhere(AA, x => x == "1").ToArray();
    //static bool FF = LinqUtil.IsArrayEqual(AA, BB);
    #endregion

    /// <summary>List_Skip往后选择指定位数的元素   Take从头开始选取固定数量的元素</summary>
    public static List<T> SubArray<T>(this List<T> list, int startIndex, int length = -1)
    {
        if (length == -1)
            return list.Skip(startIndex).ToList();

        return list.Skip(startIndex).Take(length).ToList();
    }

    /// <summary>选取符合条件的元素</summary>
    public delegate bool SelectAction<T>(T o);

    public static IEnumerable<T> Selectarray<T>(this List<T> list, SelectAction<T> selectAction)
    {
        return (from n in list where selectAction(n) select n);
    }

    /// <summary>取满足指定条件元素</summary>
    public static IEnumerable<T> TakeWhilearray<T>(this List<T> list, Func<T, bool> func)
    {
        return list.TakeWhile(func);
    }

    /// <summary>将满足指定条件元素过滤后选择剩余的所有元素</summary>
    public static IEnumerable<T> SkipWhilearray<T>(this List<T> list, Func<T, bool> func)
    {
        return list.SkipWhile(func);
    }

    /// <summary>自定义选择元素</summary>
    public static IEnumerable<T> CustomWhere<T>(T[] arrary, Func<T, bool> predicate)
    {
        return arrary.Where(predicate).ToArray();
    }

    /// <summary>比较数组并返回相同元素</summary>
    public static IEnumerable<T> CustomIntersect<T>(T[] array1, T[] array2)
    {
        return array1.Intersect(array2).ToArray();
    }

    /// <summary>比较数组并返回差异元素</summary>
    public static IEnumerable<T> CustomExcept<T>(T[] array1, T[] array2)
    {
        return array1.Except(array2).ToArray();
    }

    /// <summary>返回数组差异元素 注!!: 这个只能在确定array2比array1少并且其他元素一样的情况下才有比较好的结果</summary>
    public static IEnumerable<T> GetArrayDifference<T>(T[] array1, T[] array2)
    {
        return array1.Where(x => !array2.Contains(x)).ToArray();
    }

    /// <summary>比较数组每一个值是否相等</summary>
    public static bool IsArrayEqual<T>(T[] array1, T[] array2)
    {
        return Enumerable.SequenceEqual(array1, array2);
    }

    /// <summary>获取不相等随机数,HashSet</summary>
    public static List<int> GetRandomList(int startIndex,int Length)
    {
        HashSet<int> hs = new HashSet<int>();
        System.Random r = new System.Random();
        while (hs.Count < Length)
            hs.Add(r.Next(startIndex, startIndex + Length));
        return hs.ToList();
    }

    /// <summary>获取不相等随机数,种子</summary>
    public static void CreatRangdomList(List<int> setList, int Length)
    {
        setList.Clear();
        List<int> startList = new List<int>();
        for (int i = 0; i < Length; i++)
        {
            startList.Add(i);
        }
        int N = startList.Count;
        int[] resultArray = new int[startList.Count];
        for (int i = 0; i < N; i++)
        {
            int seed = UnityEngine.Random.Range(0, startList.Count - i);
            resultArray[i] = startList[seed];
            startList[seed] = startList[startList.Count - i - 1];
            setList.Add(resultArray[i]);
        }
    }

    /// <summary>获取array随机值</summary>
    public static T GetRandom<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
            return default(T);

        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    /// <summary>array_Skip往后选择指定位数的元素   Take从头开始选取固定数量的元素</summary>
    public static T[] SubArray<T>(this T[] array, int startIndex, int length = -1)
    {
        if (length == -1)
            return array.Skip(startIndex).ToArray();

        return array.Skip(startIndex).Take(length).ToArray();
    }

    /// <summary>获取List随机值</summary>
    public static T GetRandom<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
            return default(T);

        return list[UnityEngine.Random.Range(0, list.Count)];
    }


}

//alist = list.Distinct(new ListComparer<int>((p1, p2) => p1 == p2)).ToList();
/// <summary>
/// 去除重复元素
/// </summary>
public class ListComparer<T> : IEqualityComparer<T>
{
    public delegate bool EqualsComparer<F>(F x, F y);

    public EqualsComparer<T> equalsComparer;

    public ListComparer(EqualsComparer<T> _euqlsComparer)
    {
        this.equalsComparer = _euqlsComparer;
    }
    public bool Equals(T x, T y)
    {
        if (null != equalsComparer)
            return equalsComparer(x, y);
        else
            return false;
    }
    public int GetHashCode(T obj)
    {
        return obj.ToString().GetHashCode();
    }
}

#endregion

#region Texture拓展
public enum EncodeType
{
    JPG,
    PNG
}
public class TextureUtil
{
    /// <summary>Texture2D转Sprite</summary>
    public static Sprite Texture2DToSprite(Texture2D tex)
    {
        if (tex == null) { Debug.LogError("tex为空"); return null; }
        Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        return spr;
    }

    /// <summary>Texture转Texture2D</summary>
    public static Texture2D TextureToTexture2D(Texture tex)
    {
        if (tex == null) { Debug.LogError("tex为空"); return null; }
        return tex as Texture2D;
    }

    /// <summary>Texture转byte[]</summary>
    public static byte[] GetPhotoPixel(Texture tex, EncodeType encodeType)
    {
        if (tex == null) { Debug.LogError("tex为空"); return null; }
        Texture2D texture = tex as Texture2D;
        byte[] bytes = null;
        switch (encodeType)
        {
            case EncodeType.JPG:
                bytes = texture.EncodeToJPG();
                break;
            case EncodeType.PNG:
                bytes = texture.EncodeToPNG();
                break;
            default:
                break;
        }
        return bytes;
    }
}
#endregion

#region Transform拓展
public static class TransformUtil
{
    public static void SetX(this Transform transform, float x)
    {
        Vector3 newPosition = new Vector3(x, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }

    public static void SetY(this Transform transform, float y)
    {
        Vector3 newPosition = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = newPosition;
    }

    public static void SetZ(this Transform transform, float z)
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, z);
        transform.position = newPosition;
    }

    public static void SetXY(this Transform transform, Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    public static void SetXZ(this Transform transform, Vector3 position)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.z);
    }

    public static void SetPos(this Transform transform, Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, pos.z);
    }

    public static void SetLocalPos(this Transform transform, Vector3 pos)
    {
        transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
    }

    public static void SetLocalX(this Transform transform, float x)
    {
        Vector3 newPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = newPosition;
    }

    public static void SetLocalY(this Transform transform, float y)
    {
        Vector3 newPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        transform.localPosition = newPosition;
    }

    public static void SetLocalZ(this Transform transform, float z)
    {
        Vector3 newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        transform.localPosition = newPosition;
    }

    public static void SetSizeDelta(this RectTransform rectTransfrom, Vector2 size)
    {
        rectTransfrom.sizeDelta = new Vector2(size.x, size.y);
    }

    public static void SetLocalScale(this Transform transform, Vector3 scale)
    {
        transform.localScale = new Vector3(scale.x, scale.y, scale.z);
    }

    public static void SetLocalScaleX(this Transform transform, float x)
    {
        Vector3 newScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        transform.localScale = newScale;
    }

    public static void SetLocalScaleY(this Transform transform, float y)
    {
        Vector3 newScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        transform.localScale = newScale;
    }

    public static void SetLocalScaleZ(this Transform transform, float z)
    {
        Vector3 newScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
        transform.localScale = newScale;
    }

    public static void SetLocalAngleX(this Transform transform, float x)
    {
        transform.localEulerAngles = new Vector3(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    public static void SetLocalAngleY(this Transform transform, float y)
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
    }

    public static void SetLocalAngleZ(this Transform transform, float z)
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);
    }
}
#endregion

#region 颜色拓展
/// <summary>文字颜色</summary>
public enum STRING_COLOR
{
    /// <summary>蓝色</summary>
    Blue,
    /// <summary>绿色</summary>
    Green,
    /// <summary>红色</summary>
    Red,
    /// <summary>灰色</summary>
    Gray,
    /// <summary>橙色</summary>
    Orange,
    /// <summary>淡灰</summary>
    ThinGray,
    /// <summary>深绿</summary>
    DeepGreen,
    /// <summary>深绿ex</summary>
    DeepDGreen,
    /// <summary>浅蓝</summary>
    ThinBule,
    /// <summary>棕色</summary>
    Brown,
    /// <summary>灰白</summary>
    CGray
}

public class ColorUtil
{
    public static string SetStringColor(string str, STRING_COLOR strColor)
    {
        //基色
        string ColourPrimaries = "";
        StringBuilder sb = new StringBuilder();
        sb.Capacity = 127;
        switch (strColor)
        {
            case STRING_COLOR.Blue:
                ColourPrimaries = "#226DDD";
                break;
            case STRING_COLOR.Green:
                ColourPrimaries = "#11EE11";
                break;
            case STRING_COLOR.CGray:
                ColourPrimaries = "#E4DDBB";
                break;
            case STRING_COLOR.Red:
                ColourPrimaries = "#EE1111";
                break;
            case STRING_COLOR.Gray:
                ColourPrimaries = "#B1ADAA";
                break;
            case STRING_COLOR.Orange:
                ColourPrimaries = "#EEA652";
                break;
            case STRING_COLOR.ThinGray:
                ColourPrimaries = "#D8D6C1";
                break;
            case STRING_COLOR.DeepGreen:
                ColourPrimaries = "#05B142";
                break;
            case STRING_COLOR.DeepDGreen:
                ColourPrimaries = "#1EB01A";
                break;
            case STRING_COLOR.ThinBule:
                ColourPrimaries = "#0096ff";
                break;
            case STRING_COLOR.Brown:
                ColourPrimaries = "#725137";
                break;
        }
        sb.Append("<color=");
        sb.Append(ColourPrimaries);
        sb.Append(">");
        sb.Append(str);
        sb.Append("</color>");
        //string strc =  + ColourPrimaries + ">" + str + "</color>";
        return sb.ToString(0, sb.Length);
    }

    public static string SetStringColor(string str, string strColor)
    {
        string strc = "<color=" + strColor + ">" + str + "</color>";
        return strc;
    }

    /// <summary>半角转全角</summary>
    public static string ToSBC(string input)
    {
        char[] array = input.ToCharArray();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == 32)
            {
                array[i] = (char)12288;
                continue;
            }
            if (array[i] < 127)
            {
                array[i] = (char)(array[i] + 65248);
            }
        }
        return new string(array);
    }

    /// <summary>全角转半角</summary>
    public static string ToDBC(string input)
    {
        char[] array = input.ToCharArray();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == 12288)
            {
                array[i] = (char)32;
                continue;
            }
            if (array[i] > 65280 && array[i] < 65375)
            {
                array[i] = (char)(array[i] - 65248);
            }
        }
        return new string(array);
    }

    /// <summary>色码转颜色</summary>
    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
}
#endregion

#region 字符串拓展
public class StringUtil
{
    /// <summary>获得字符下标</summary>
    public static int GetStrngIndex(string str, char charStr)
    {
        int index = 0;

        foreach (char item in str)
        {
            if (item != charStr)
            {
                index++;
            }
            else
            {
                break;
            }
        }
        return index;
    }

    /// <summary>统计string中的字符类型和个数</summary>
    public static void CollectionChrCount(string str)
    {
        int iAllChr = 0; //字符总数：不计字符'\n'和'\r'
        int iChineseChr = 0; //中文字符计数
        int iChinesePnct = 0;//中文标点计数
        int iEnglishChr = 0; //英文字符计数
        int iEnglishPnct = 0;//英文标点计数
        int iNumber = 0;  //数字字符：0-9
        foreach (char ch in str)
        {
            if (ch != '\n' && ch != '\r') iAllChr++;
            if ("～！＠＃￥％…＆（）—＋－＝".IndexOf(ch) != -1 ||
             "｛｝【】：“”；‘'《》，。、？｜＼".IndexOf(ch) != -1) iChinesePnct++;
            if (ch >= 0x4e00 && ch <= 0x9fbb) iChineseChr++;
            if ("`~!@#$%^&*()_+-={}[]:\";'<>,.?/\\|".IndexOf(ch) != -1) iEnglishPnct++;
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')) iEnglishChr++;
            if (ch >= '0' && ch <= '9') iNumber++;
        }
        string sStats = string.Format(string.Concat(
         "字符总数：{0}\r\n", "中文字符数：{1}\r\n", "中文标点数：{2}\r\n",
         "英文字符数：{3}\r\n", "英文标点数：{4}\r\n", "数字字符数：{5}\r\n"),
         iAllChr.ToString(), iChineseChr.ToString(), iEnglishChr.ToString(),
         iEnglishChr.ToString(), iEnglishPnct.ToString(), iNumber.ToString());
        Debug.Log(sStats);
    }

    ///<summary>飘字提示字符串生成</summary>
    public static string GetFomateString(string str, params string[] parm)
    {
        // 获取参数字符长度
        int destLen = 0;
        for (int i = 0; i < parm.Length; i++)
        {
            destLen += parm[i].Length;
        }

        // 获取替换位置的索引
        List<int> indexArr = new List<int>();
        char[] charArr = str.ToCharArray();
        for (int j = 0; j < charArr.Length; j++)
        {
            if (charArr[j] == '%' && (charArr.Length > j + 1) && charArr[j + 1] == 's')
            {
                indexArr.Add(j);
                j++;
            }
        }

        // 创建替换后的字符数组
        char[] destCharArr = new char[destLen + charArr.Length - indexArr.Count * 2];

        // 生成替换后的字符数组
        int indexEnd = 0;
        int destIndex = 0;
        for (int k = 0; k < indexArr.Count; k++)
        {
            int index = indexArr[k];
            int len = index - indexEnd;

            int p = -1;
            while (++p < len)
            {
                destCharArr[destIndex + p] = charArr[indexEnd + p];
            }
            indexEnd = index + 2;
            destIndex += len;

            p = -1;
            //char[] srcCharArr = parm[k].ToCharArray();
            int charArrLen = parm[k].Length;
            while (++p < charArrLen)
            {
                destCharArr[destIndex + p] = parm[k][p];
            }

            destIndex += parm[k].Length;
        }
        // 剩余长度
        int residueLen = charArr.Length - indexEnd;
        int pl = -1;
        while (++pl < residueLen)
        {
            destCharArr[destIndex + pl] = charArr[indexEnd + pl];
        }

        string strFomate = new string(destCharArr);
        return strFomate;
    }

    /// <summary>获取字符串中的中文个数</summary>
    public static int GetStringChineseCounts(string str)
    {
        int count = 0;
        Regex regex = new Regex(@"^[\u4E00-\u9FA5]{0,}$");

        for (int i = 0; i < str.Length; i++)
        {
            if (regex.IsMatch(str[i].ToString()))
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>检查index位置是否存在匹配子字符串sub</summary>
    public static bool CheckStringMatch(string sub, string baseStr, int index)
    {
        int i = 0;
        while (i < sub.Length)
        {
            if (sub[i] != baseStr[index + i])
            {
                return false;
            }
            i++;
        }
        return true;
    }
}
#endregion

#region 时间拓展
/// <summary>
/// 闰年平年
/// </summary>
public enum Leap_Common_Year
{
    Leap_Year = 29,
    Common_Year = 28
}

public class TimeUtil
{
    /// <summary>获取润平年</summary>
    public static Leap_Common_Year GetLerpCommonYaer()
    {
        if (DateTime.IsLeapYear(DateTime.Now.Date.Year))
        {
            return Leap_Common_Year.Leap_Year;
        }
        return Leap_Common_Year.Common_Year;
    }

    /// <summary>获取 0天00时00分</summary>
    public static string GetTimeDdHhmmss2(int leftTime)
    {
        int day = leftTime / 86400;
        int hour = (leftTime - day * 86400) / 3600;
        int min = (leftTime - day * 86400 - hour * 3600) / 60;
        return day + "天" + (hour < 10 ? "0" + hour : hour + "") +
            "时" + (min < 10 ? "0" + min : min + "") + "分";
    }

    /// <summary>获取 0时00分</summary>
    public static string GetTimeHhmm2(uint leftTime)
    {
        uint hour = leftTime / 3600;
        uint min = (leftTime - hour * 3600) / 60;
        return (hour > 0 ? hour + "时" : "") + min + "分";
    }

    /// <summary>大于一天显示天数，小于一天显示 时：分</summary>
    public static string GetTimeDdHhmm(int leftTime)
    {
        int day = leftTime / 86400;
        if (day > 0) return day + "天";
        leftTime = leftTime - day * 86400;
        int hour = leftTime / 3600;
        int min = (leftTime - hour * 3600) / 60;
        return (hour < 10 ? "0" + hour : hour + "") + ":" + (min < 10 ? "0" + min : min + "");
    }

    /// <summary>00时00分00秒 格式的时间字符串  leftTime(秒)</summary>
    public static string GetTimeHhmmss4(int leftTime)
    {
        int day = leftTime / 86400;
        int hour = (leftTime - day * 86400) / 3600;
        int min = (leftTime - day * 86400 - hour * 3600) / 60;
        int sec = (leftTime - day * 86400 - hour * 3600 - min * 60);
        return (hour < 10 ? "0" + hour : hour + "") +
            "时" + (min < 10 ? "0" + min : min + "") + "分" + (sec < 10 ? "0" + sec : sec + "") + "秒";
    }

    /// <summary>获取  00:00   00时00分 格式的时间字符串  leftTime(秒)</summary>
    public static string GetTimeHhmm(int leftTime)
    {
        int hour = leftTime / 3600;
        int min = leftTime / 60;
        int sec = (leftTime - min * 60);
        min -= hour * 60;
        if (sec > 0)
        {
            min += 1;
            if (min > 59)
            {
                min = 59;
            }
        }
        return (hour < 10 ? "0" + hour : hour + "") + ":" + (min < 10 ? "0" + min : min + "");
    }

    /// <summary>获取  00:00   00分00秒 格式的时间字符串  leftTime(秒)</summary>
    public static string GetTimeMmss(int leftTime)
    {
        int min = leftTime / 60;
        int sec = (leftTime - min * 60);
        return (min < 10 ? "0" + min : min + "") + ":" + (sec < 10 ? "0" + sec : sec + "");
    }

    /// <summary>获取  00:00:00   00时00分00秒 格式的时间字符串  leftTime(秒)</summary>
    public static string GetTimeHhmmss(int leftTime)
    {
        int hour = leftTime / 3600;
        int min = leftTime / 60;
        int sec = (leftTime - min * 60);
        min -= hour * 60;
        return (hour < 10 ? "0" + hour : hour + "") + ":" + (min < 10 ? "0" + min : min + "") + ":" + (sec < 10 ? "0" + sec : sec + "");
    }

    /// <summary>显示xx时xx分xx秒</summary>
    public static string GetTimeHhmmss2(int leftTime)
    {
        int hour = leftTime / 3600;
        int min = (leftTime - hour * 3600) / 60;
        int sec = leftTime % 60;
        if (hour > 0)
        {
            return hour + "时" + (min < 10 ? "0" + min : min + "") + "分" +
(sec < 10 ? "0" + sec : sec + "") + "秒";
        }
        else if (min > 0)
        {
            return (min < 10 ? "0" + min : min + "") + "分" +
(sec < 10 ? "0" + sec : sec + "") + "秒";
        }
        else
        {
            return (sec < 10 ? sec.ToString() : sec + "") + "秒";
        }
    }

    /// <summary>显示XX分XX秒 不足1分只显示XX秒</summary>
    public static string GetTimeHhmmss3(int leftTime)
    {
        int min = leftTime / 60;
        int sec = leftTime % 60;
        if (min > 0)
        {
            return min + "分" +
                (sec < 10 ? "0" + sec : sec + "") + "秒";
        }
        else
        {
            return sec + "秒";
        }
    }

    /// <summary>大于一天显示天数，小于一天显示 时:分:秒  00:00:00  格式的时间字符串  leftTime(秒)</summary>
    public static string GetTimeDdHhmmss(int leftTime)
    {
        int day = leftTime / 86400;
        if (day > 0) return day + "天";
        int hour = leftTime / 3600;
        int min = leftTime / 60;
        int sec = (leftTime - min * 60);
        min -= hour * 60;
        return (hour < 10 ? "0" + hour : hour + "") + ":" + (min < 10 ? "0" + min : min + "") + ":" + (sec < 10 ? "0" + sec : sec + "");
    }

    /// <summary>获取 0天（向上取整）</summary>
    public static string GetDays(int leftTime)
    {
        int day = leftTime / 86400;
        int hour = (leftTime - day * 86400) / 3600;
        int min = (leftTime - day * 86400 - hour * 3600) / 60;
        if (min > 0)
        {
            day += 1;
        }
        return day + "天";
    }

    /// <summary>获取离线时间</summary>
    public static string GetleveTime(uint status)
    {
        string time = "";
        if (status == 0)
        {
            time = "在线";
        }
        else
        {
            uint nowtime = GetSecondTime();
            uint lvtime = 0;
            if (nowtime > status)
                lvtime = nowtime - status;
            if (lvtime < 3600)
            {
                time = (lvtime / 60).ToString() + "分钟前";
            }
            else if (3600 < lvtime && lvtime < 86400)
            {
                time = (lvtime / 3600).ToString() + "小时前";
            }
            else if (lvtime > 86400 && lvtime < 2592000)
            {
                time = (lvtime / 86400).ToString() + "天前";

            }
            else
            {
                time = "30天前";
            }
        }
        return time;
    }

    public static uint GetSecondTime()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        return Convert.ToUInt32(ts.TotalSeconds);
    }

    public static uint GetMillisecondsTime()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        return (uint)ts.TotalMilliseconds;
    }
}
#endregion

#region 链式拓展

#region GameObject
public static class GameObjectExtension
{
    public static GameObject Show(this GameObject selfObj)
    {
        selfObj.SetActive(true);
        return selfObj;
    }

    public static GameObject Hide(this GameObject selfObj)
    {
        selfObj.SetActive(false);
        return selfObj;
    }

    public static GameObject SetLocalPos(this GameObject selfObj, Vector3 pos)
    {
        TransformUtil.SetLocalPos(selfObj.transform, pos);
        return selfObj;
    }

    public static GameObject SetScale(this GameObject selfObj, Vector3 scale)
    {
        TransformUtil.SetLocalScale(selfObj.transform, scale);
        return selfObj;
    }

    public static GameObject SetParent(this GameObject selfObj, Transform parent)
    {
        GameTool.AddChildToParent(parent, selfObj.transform);
        return selfObj;
    }
}
#endregion

#region Graphic
public static class MaskableGraphicExtension
{
    public static MaskableGraphic SetSizeDelta(this MaskableGraphic graphic, Vector2 sizeDelta)
    {
        TransformUtil.SetSizeDelta(graphic.rectTransform, sizeDelta);
        return graphic;
    }
}
#endregion

#region Component
public static class ComponentExtension
{
    /// <summary>获取第一级子物体的组件</summary>
    public static T[] GetComponentsInFirstChildren<T>(this Transform selfTrans) where T : Component
    {
        List<T> list = new List<T>();
        for (int i = 0; i < selfTrans.childCount; i++)
        {
            T t = selfTrans.GetChild(i).GetComponent<T>();
            if (t != null)
                list.Add(selfTrans.GetChild(i).GetComponent<T>());
        }
        return list.ToArray();
    }
}
#endregion

#region Action
public static class ActionExtension
{
    //        this.InvokeRepeating(()=>{ AA("",2,3); },0.5f,5);
    /// <summary>
    /// 重复执行某一方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <param name="action"></param>
    /// <param name="delayTime"></param>
    /// <param name="repeatCount"></param>
    /// <returns></returns>
    public static T InvokeRepeating<T>(this T t, Action action, float delayTime, int repeatCount = 0) where T : MonoBehaviour
    {
        t.StartCoroutine(WaitExcuteAction(t, action, delayTime, repeatCount));
        return t;
    }

    public static IEnumerator WaitExcuteAction(MonoBehaviour mono, Action action, float delayTime, int repeatCount)
    {
        if (repeatCount == -1)
        {
            yield return new WaitForSeconds(delayTime);
            action();
            mono.StartCoroutine(WaitExcuteAction(mono, action, delayTime, repeatCount));
        }
        else
        {
            for (int i = 0; i < repeatCount; i++)
            {
                yield return new WaitForSeconds(delayTime);
                action();
            }
        }
    }
}
#endregion

#endregion



