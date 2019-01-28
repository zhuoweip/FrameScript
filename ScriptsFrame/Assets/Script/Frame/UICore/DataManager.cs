using System.Collections;
using System.Collections.Generic;
using UICore;
using UnityEngine;

public class DataManager : UnitySingleton<DataManager> {

    private GUIStyle style;
    public GUIStyle Style { get { return style; } }

    protected override void Awake()
    {
        base.Awake();

        style = new GUIStyle();
        style.fontSize = 120;
        style.normal.textColor = Color.red;

        Cursor.visible = bool.Parse(Configs.Instance.LoadText("开启鼠标", "false/true"));
        //Debug.logger.logEnabled = bool.Parse(Configs.Instance.LoadText("开启日志", "false/true"));
        Debug.logger.filterLogType = LogType.Error;//仅保留错误日志
    }
}
