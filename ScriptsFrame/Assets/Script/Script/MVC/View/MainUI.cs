using UnityEngine;
using System.Collections;
using UICore;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using SwanEngine.Events;

public class MainUI : BaseUI
{
    private Button gameBtn;

    //初始化界面元素
    protected override void InitUiOnAwake()
    {
        gameBtn = GameTool.GetTheChildComponent<Button>(gameObject, "gameBtn");
    }

    //初始化界面数据
    protected override void InitDataOnAwake()
    {
        
    }

    protected override void RegistBtns()
    {
        base.RegistBtns();
        gameBtn.onClick.AddListener(() => { PlayGame(); });
    }

    private void PlayGame()
    {
        //UIManager.Instance.ShowUI(EUiId.GameADialogUI,EUiId.MainUI);
    }
}
