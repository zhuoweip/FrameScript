using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QR_Code : MonoBehaviour
{
    private Texture2D encoded;
    private string Lastresult;
    private RawImage img;

    private void Awake()
    {
        img = this.GetComponent<RawImage>();
        encoded = new Texture2D(256, 256);
        lastUrl = GetData(30);
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    private void ShowSaveInfo()
    {
        string textForEncoding = Lastresult;
        if (textForEncoding != null)
        {
            Color32[] color32 = Encode(textForEncoding, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
        }
        img.texture = encoded;
        img.enabled = true;
    }

    string url = "http://sq.gzcloudbeingbu.com/Webpage/MyScreenshots.aspx";
    string lastUrl = "http://www.qq.com";

    public void UpLoad(int score)
    {
        StartCoroutine(UploadPNG(score));
    }

    //生成本地二维码
    public void UploadLocal(Texture2D logo)
    {
        string textForEncoding = lastUrl;
        if (textForEncoding != null)
        {
            //二维码写入图片
            Color32[] color32 = Encode(textForEncoding, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            img.texture = encoded;
        }
    }

    private const string gameName = "现代-品牌与财富区域";
    private string dateTime;
    private string guid;

    private string GetData(int score)
    {
        string postData = string.Empty;
        guid = Guid.NewGuid().ToString();
        dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        postData = guid + "|" + gameName + "|" + dateTime + "|" + score.ToString();
        return postData;
    }

    //上传图片扫描二维码识别
    private IEnumerator UploadPNG(int score)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(GetData(score));

        WWW www = new WWW(url, bytes);//上传到服务器
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
            Lastresult = "http://sq.gzcloudbeingbu.com/Picture/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + guid + ".png";
            ShowSaveInfo();
        }
    }

    public long GetTimeStamp(bool bflag = true)
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        long ret;
        if (bflag)
            ret = Convert.ToInt64(ts.TotalSeconds);
        else
            ret = Convert.ToInt64(ts.TotalMilliseconds);
        return ret;
    }

}