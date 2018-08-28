using UnityEngine;
using System.Drawing;
using System.IO;
public class LoadImage
{ 
    /// <summary>
    /// 根据图片路径返回Texture2d格式图片
    /// </summary>
    /// <param name="imgPath"></param>
    /// <returns></returns>
    public static Texture2D GetTexrture2DFromPath(string imgPath)
    {
        //读取文件
        FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
        int byteLength = (int)fs.Length;
        byte[] imgBytes = new byte[byteLength];
        fs.Read(imgBytes, 0, byteLength);
        fs.Close();
        fs.Dispose();
        //转化为Texture2D
        Image img = Image.FromStream(new MemoryStream(imgBytes));
        Texture2D t2d = new Texture2D(img.Width, img.Height);
        img.Dispose();
        t2d.LoadImage(imgBytes);
        t2d.Apply();
        return t2d;
    }

    //public static string[] GetFileName(string path)
    //{
    //    if (Directory.Exists(path))
    //    {
    //      DirectoryInfo info=new DirectoryInfo(path);
    //        return null;
    //    }
    //    else
    //    {
    //        Debug.LogError("Don't found the path!");
    //        return null;
    //    }
    //}
}
