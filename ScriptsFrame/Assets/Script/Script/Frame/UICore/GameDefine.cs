using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UICore
{
    public enum EUiId
    {
        NullUI,
        MainUI,
        GameAUI,
        GameBUI
    }

    public enum EMessageType
    {
        SetDialogIndex,
        BuyGoods,
        GetCoin
    }

    public class GameDefine
    {
        public const string Event_SetDialogIndex = "Event_SetDialogIndex";

        public static Dictionary<EUiId, string> dicPath = new Dictionary<EUiId, string>
        {
            { EUiId.MainUI,"UIPrefab/"+"MainUI"},
            { EUiId.GameAUI,"UIPrefab/"+"GameAUI"},
            { EUiId.GameBUI,"UIPrefab/"+"GameBUI"},
        };

        public static Dictionary<int, string> gameDic = new Dictionary<int, string>
        {
            {0,"logo临水9" },
            {1,"logo珍酒12" },
            {2,"logo榆树钱16" },
        };

        public static Type GetUIScriptType(EUiId uiId)
        {
            Type scriptType = null;
            switch (uiId)
            {
                case EUiId.MainUI:
                    scriptType = typeof(MainUI);
                    break;
                default:
                    break;
            }
            return scriptType;
        }
    }
}

