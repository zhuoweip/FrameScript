using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICore;
using UnityEngine.UI;

public class GameAUI : BaseUI {

private RawImage r1rImg;
private Button r1btn;
private RawImage r2rImg;
private Button r2btn;
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
r2rImg = GameTool.GetTheChildComponent<RawImage>(gameObject,"r2");
r2btn = GameTool.GetTheChildComponent<Button>(gameObject,"r2");
        r1rImg = GameTool.GetTheChildComponent<RawImage>(gameObject,"r1");
        r1btn = GameTool.GetTheChildComponent<Button>(gameObject,"r1");
    }

    // Use this for initialization
    protected override void RegistBtns()
    {
        base.RegistBtns();
r2btn.onClick.AddListener(() => { UIManager.Instance.ShowUI(EUiId.MainUI); });
        r1btn.onClick.AddListener(() => {
            UIManager.Instance.ShowUI(EUiId.GameBUI,this.transform);
        });
    }

    public void AAA()
    {
        Debug.Log(123);
    }
}
