using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.Text;
using System.IO;
/// <summary>
/// 配置文件
/// </summary>
public class Configs : MonoSingleton<Configs> {
	private void Awake()
    {
        init();
    }

    private string[] keys;
    private JsonData jsonData;
    private Dictionary<string, string> dic = new Dictionary<string, string>();
    private Dictionary<string, JsonData> jsonObject = new Dictionary<string, JsonData>();

    string filepath;
    //初始化json
    void init()
    {
        //string filepath;
#if UNITY_EDITOR
        filepath =Application.dataPath + "/StreamingAssets" + "/Config.txt";
#elif UNITY_STANDALONE_WIN
				filepath =Application.dataPath + "/StreamingAssets" + "/Config.txt";
#elif UNITY_IPHONE
				filepath = Application.dataPath + "/Raw" + "/Config.txt";	
#elif UNITY_ANDROID
				filepath = "jar:Application.dataPath + "!/assets" + "/Config.txt";
#endif
        bool isContains = FileHandle.instance.isExistFile(filepath);

        if (isContains)
        {
            string str = FileHandle.instance.FileToString(filepath, Encoding.Default);
            jsonObject = JsonMapper.ToObject<Dictionary<string, JsonData>>(str);
            jsonData = JsonMapper.ToObject(str);
            #region  key 遍历
            //foreach (var item in jsonObject)
            //    Debug.Log(item.Key + "  " + item.Value.ToJson());
            IDictionary jsonDic = jsonData as IDictionary;
            foreach (string item in jsonDic.Keys)
                dic.Add(item, jsonDic[item].ToString());
            foreach (var item in jsonObject)
                dic[item.Key] = item.Value.ToJson();
            #endregion
        }
    }

   /// <summary>
   /// 外部调用加载图片
   /// </summary>
   /// <param name="name"></param>
   /// <returns></returns>
   public List<Texture2D> LoadTexture(string name)
    {
        ArrayList ar = new ArrayList();
        //Debug.Log("name: " + jsonData[name]);
        string path = (string)jsonData[name]["texpath"];
        ar = getRes.instance.getResTex(path);
        List<Texture2D> list = new List<Texture2D>();
        foreach (Texture2D temp in ar)
            list.Add(temp);
        return list;
    }
 
    /// <summary>
    /// 外部调用获取文字信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="info"></param>
    /// <returns></returns>
   public string LoadText(string name,string info)
   {
       string str = (string)jsonData[name][info];
       //Debug.Log(str);
       return str;
   }

    /// <summary>
    /// 修改Config并保存，这种做法需要修改特定字符串，可以使用但并不建议
    /// </summary>
    /// <param name="name"></param>
    /// <param name="info"></param>
    /// <param name="changStr"></param>
    /// Configs.Instance.ChangText("隐藏鼠标","false/true","false");
    public void ChangText(string name, string info, string changStr)
    {
        string data = dic[name];
        int index = data.LastIndexOf(":");
        string value = data.Substring(index, data.Length - index);
        string headStr = data.Replace(value, string.Empty);
        value = value.Replace(":", string.Empty).Replace("\"", string.Empty).Replace("}", string.Empty);
        value = changStr;
        string result = headStr + ":" + "\"" + value + "\"" + "}";
        dic[name] = result;
        string values = JsonMapper.ToJson(dic);
        values = values.Replace("\"" + "{" + "\"", "{" + "\"").Replace("\"" + "}" + "\"", "\"" + "}");

        StreamWriter sw;
        sw = new StreamWriter(filepath, false, Encoding.UTF8);
        sw.WriteLine(values);
        sw.Flush();
        sw.Close();
        sw.Dispose();
    }

    /// <summary>
    /// 获取子节点个数
    /// </summary>
    /// <returns></returns>
    public int GetJsonCount()
    {
        return jsonData.Count;
    }
	
    /// <summary>
    /// 获取子节点下子节点个数
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int GetJsonCount(string name)
    {
        return jsonData[name].Count;
    }
}
