using System.Collections;
using System.Collections.Generic;
using UICore;
using UnityEngine;

public class ConfigManager : UnitySingleton<ConfigManager>
{
    protected override void Awake()
    {
        base.Awake();
        Debug.logger.logEnabled = bool.Parse(Configs.Instance.LoadText("开启Debug", "false/true"));
    }
}
