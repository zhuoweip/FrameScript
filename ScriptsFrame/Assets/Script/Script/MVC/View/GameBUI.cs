using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICore;
using UnityEngine.UI;

public class GameBUI : BaseUI
{


private RawImage r2rImg;
private Button r2btn;
private RawImage r3rImg;
private Button r3btn;
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
r3rImg = GameTool.GetTheChildComponent<RawImage>(gameObject,"r3");
r3btn = GameTool.GetTheChildComponent<Button>(gameObject,"r3");
r2rImg = GameTool.GetTheChildComponent<RawImage>(gameObject,"r2");
r2btn = GameTool.GetTheChildComponent<Button>(gameObject,"r2");
    }

    protected override void RegistBtns()
    {
        base.RegistBtns();
r3btn.onClick.AddListener(() => {UIManager.Instance.ShowUI(EUiId.MainUI); });
    r2btn.onClick.AddListener(() => {
    
    
    UIManager.Instance.ShowUI(EUiId.GameAUI);
});
    }
}
