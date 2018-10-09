using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public enum SceneTransType
{
    Null,

    /*----------GUI--------------------------*/
    /// <summary>百叶窗</summary>
    [Description("GUI")]
    Blinds,

    [Description("GUI")]
    Cartoon,

    /// <summary>电影</summary>
    [Description("GUI")]
    Cinema,

    [Description("GUI")]
    Fade,

    /// <summary>报纸</summary>
    [Description("GUI")]
    Newspaper,

    /// <summary>剪刀</summary>
    [Description("GUI")]
    Ninja,

    [Description("GUI")]
    Pixelate,

    [Description("GUI")]
    Plain,

    /// <summary>俄罗斯方块</summary>
    [Description("GUI")]
    Tetris,

    [Description("GUI")]
    Tiles,

    /*----------UGUI--------------------------*/
    /// <summary>圆形缩放</summary>
    [Description("UGUI")]
    CircleWipe,
    /// <summary>360旋转</summary>
    [Description("UGUI")]
    ClockWipe,
    /// <summary>从左到右</summary>
    [Description("UGUI")]
    LinearWipe,
    /// <summary>从左到右</summary>
    [Description("UGUI")]
    MaskTexWipe,
    /// <summary>百叶窗可以上下左右</summary>
    [Description("UGUI")]
    ShutterWipe,
}


public class SceneTransition
{
    public const string transpath = "Transitions/";

    public static Dictionary<SceneTransType, string> sceneDic = new Dictionary<SceneTransType, string>
    {
        { SceneTransType.Blinds,transpath +     "SMBlindsTransition"},
        { SceneTransType.Cartoon,transpath +    "SMCartoonTransition"},
        { SceneTransType.Cinema,transpath +     "SMCinemaTransition"},
        { SceneTransType.Fade,transpath +       "SMFadeTransition"},
        { SceneTransType.Newspaper,transpath +  "SMNewspaperTransition"},
        { SceneTransType.Ninja,transpath +      "SMNinjaTransition"},
        { SceneTransType.Pixelate,transpath +   "SMPixelateTransition"},
        { SceneTransType.Plain,transpath +      "SMPlainTransition"},
        { SceneTransType.Tetris,transpath +     "SMTetrisTransition"},
        { SceneTransType.Tiles,transpath +      "SMTilesTransition"},

        //{ SceneTransType.CircleWipe,transpath +  "SMPostEffectsTransition"},
    };

    public const string UguiNormalPath = transpath + "SMPostEffectsTransition";
    public const string UguiMaskTexPath = transpath + "SMMaskTexTransition";
    public const string UGUI = "UGUI";
    public const string GUI = "GUI";

    public static void ShowTranstion(SceneTransType type = SceneTransType.Null, System.Action inAction = null, System.Action holdAciton = null)
    {
        if (type != SceneTransType.Null)
        {
            //FieldInfo fieldInfo = type.GetType().GetField(type.ToString());
            //object[] attribArray = fieldInfo.GetCustomAttributes(false);
            //if (attribArray.Length > 0)
            //{
            //    DescriptionAttribute attrib = (DescriptionAttribute)attribArray[0];
            //    //通过枚举描述来判断生成类型
            //    if (attrib.Description == GUI)
            //    {
            //        SMTransition smtranstion = GameObject.Instantiate(Resources.Load<GameObject>(sceneDic[type])).GetComponent<SMTransition>();
            //        smtranstion.InAction = inAction;
            //        smtranstion.HoldAction = holdAciton;
            //    }
            //    else
            //    {
            //        SMPostEffectsTransition smpostransition = GameObject.Instantiate(Resources.Load<GameObject>(sceneDic[type])).GetComponent<SMPostEffectsTransition>();
            //        smpostransition.ShaderName = type.ToString();
            //        smpostransition.InAction = inAction;
            //        smpostransition.HoldAction = holdAciton;
            //    }
            //}

            if (sceneDic.ContainsKey(type))
            {
                SMTransition smtranstion = GameObject.Instantiate(Resources.Load<GameObject>(sceneDic[type])).GetComponent<SMTransition>();
                smtranstion.InAction = inAction;
                smtranstion.HoldAction = holdAciton;
            }
            else
            {
                string path = UguiNormalPath;
                if (type == SceneTransType.MaskTexWipe)
                    path = UguiMaskTexPath;
                SMPostEffectsTransition smpostransition = GameObject.Instantiate(Resources.Load<GameObject>(path)).GetComponent<SMPostEffectsTransition>();
                smpostransition.ShaderName = type.ToString();
                smpostransition.InAction = inAction;
                smpostransition.HoldAction = holdAciton;
            }
        }
        else
        {
            if (holdAciton != null)
                holdAciton();
        }
    }
}
