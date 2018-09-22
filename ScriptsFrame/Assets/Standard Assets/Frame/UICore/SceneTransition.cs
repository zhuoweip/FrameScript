using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneTransType
{
    Null,
    /// <summary>百叶窗</summary>
    Blinds,
    Cartoon,
    /// <summary>电影</summary>
    Cinema,
    Fade,
    /// <summary>报纸</summary>
    Newspaper,
    /// <summary>剪刀</summary>
    Ninja,
    Pixelate,
    Plain,
    /// <summary>俄罗斯方块</summary>
    Tetris,
    Tiles
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
    };

    public static void ShowTranstion(SceneTransType type = SceneTransType.Null, System.Action inAction = null, System.Action holdAciton = null)
    {
        if (type != SceneTransType.Null)
        {
            SMTransition smtranstion = GameObject.Instantiate(Resources.Load<GameObject>(sceneDic[type])).GetComponent<SMTransition>();
            smtranstion.InAction = inAction;
            smtranstion.HoldAction = holdAciton;
        }
        else
        {
            if (holdAciton != null)
                holdAciton();
        }
    }
}
