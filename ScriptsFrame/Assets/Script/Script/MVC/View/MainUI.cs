using UnityEngine;
using System.Collections;
using UICore;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using SwanEngine.Events;

public class MainUI : BaseUI
{
    //初始化界面元素
private RawImage r3rImg;
private Button r3btn;
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
r3rImg = GameTool.GetTheChildComponent<RawImage>(gameObject,"r3");
r3btn = GameTool.GetTheChildComponent<Button>(gameObject,"r3");
    }

    //初始化界面数据
    protected override void InitDataOnAwake()
    {
      
    }

    protected override void RegistBtnsAnimation()
    {
        base.RegistBtnsAnimation();
    }


    protected override void RegistBtns()
    {
        base.RegistBtns();

        r3btn.onClick.AddListener(() => {
            UIManager.Instance.ShowUI(EUiId.GameAUI,SceneTransType.ShutterWipe);
            
        });
        BtnAniType = ButtonAniType.Scale;
        RegistBtnsAnimation();
    }

    private void PlayGame()
    {
    }

    private void AA()
    {
        
    }
}
