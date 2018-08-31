using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICore;
using UnityEngine.UI;

public class GameBUI : BaseUI
{


private RawImage r2rImg;
private Button r2btn;
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
r2rImg = GameTool.GetTheChildComponent<RawImage>(gameObject,"r2");
r2btn = GameTool.GetTheChildComponent<Button>(gameObject,"r2");
    }

    protected override void RegistBtns()
    {
        base.RegistBtns();
r2btn.onClick.AddListener(() => {
    UIManager.Instance.ShowUI(EUiId.MainUI);
});
    }
}
