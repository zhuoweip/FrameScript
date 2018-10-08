using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SMPostEffectsTransition : MonoBehaviour {

    private Material material;
    private RawImage rawImage;
    private float duration;

    public Action HoldAction
    {
        set { holdAction = value; }
        get { return holdAction; }
    }

    public Action InAction
    {
        set { inAction = value; }
        get { return inAction; }
    }

    private Action holdAction;
    private Action inAction;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (material ==null)
            material = new Material(Shader.Find("Scene Manager/transition/CircleWipe"));
        rawImage = GetComponent<RawImage>();
        rawImage.SetParent(FindObjectOfType<Canvas>().transform);
        rawImage.material = material;
        if (inAction != null)
            inAction();
        material.DOFloat(0, "_Value", duration).OnComplete(() =>
          {
              if (holdAction != null)
                  holdAction();
              material.DOFloat(1, "_Value", duration).OnComplete(()=>
              {
                  Destroy(gameObject);
              });
          });
    }
}
