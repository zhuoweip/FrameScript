using System.Collections.Generic;

namespace UICore
{
    public enum EUiId
    {
        NullUI,
        MainUI,
        GameAUI,
        GameBUI,
        FindUI,
		MalekUI,
    }

    public class GameDefine
    {
        public const string Event_SetDialogIndex = "Event_SetDialogIndex";

        public static Dictionary<EUiId, string> dicPath = new Dictionary<EUiId, string>
        {
			{ EUiId.MalekUI,"UIPrefab/"+"MalekUI"},
			{ EUiId.FindUI,"UIPrefab/"+"FindUI"},
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
    }
}

