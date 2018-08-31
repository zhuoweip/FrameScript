using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICore;
using UnityEngine.UI;

public class GameAUI : BaseUI {

private RawImage r1rImg;
private Button r1btn;
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        r1rImg = GameTool.GetTheChildComponent<RawImage>(gameObject,"r1");
        r1btn = GameTool.GetTheChildComponent<Button>(gameObject,"r1");
    }

    // Use this for initialization
    protected override void RegistBtns()
    {
        base.RegistBtns();
        r1btn.onClick.AddListener(() => {
            UIManager.Instance.AddUI(EUiId.GameBUI);
        });
    }
}
