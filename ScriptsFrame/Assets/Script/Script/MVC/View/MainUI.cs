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
    private Transform go;
    private RawImage[] rImgs;

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
r3rImg = GameTool.GetTheChildComponent<RawImage>(gameObject,"r3");
r3btn = GameTool.GetTheChildComponent<Button>(gameObject,"r3");

        go = GameTool.FindTheChild(gameObject, "GameObject");
        rImgs = go.GetComponentsInChildren<RawImage>();
        
    }

    //初始化界面数据
    protected override void InitDataOnAwake()
    {
        Texture[] texs = Loader.Instance.LoadObjects<Texture>("a");
        for (int i = 0; i < rImgs.Length; i++)
        {
            rImgs[i].texture = texs[i];
        }
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
