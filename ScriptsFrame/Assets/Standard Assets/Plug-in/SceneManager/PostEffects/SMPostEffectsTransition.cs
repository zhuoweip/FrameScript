using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SMPostEffectsTransition : MonoBehaviour {

    private Material material;
    private RawImage rawImage;
    private float duration = 1;

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

    private string shaderName;

    public string ShaderName
    {
        set { shaderName = value; }
        get { return shaderName; }
    }

    public Texture tex;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (material == null)
        {
            Shader shader = Shader.Find("Scene Manager/" + shaderName);
            material = new Material(shader);
            if (tex && !material.GetTexture("_MaskTex"))
                material.SetTexture("_MaskTex", tex);
        }
 
        rawImage = GetComponent<RawImage>();
        rawImage.material = material;

        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(Screen.width, Screen.height);
        transform.parent = FindObjectOfType<Canvas>().transform;
        transform.localPosition = Vector2.zero;

        if (inAction != null)
            inAction();
        material.DOFloat(0, "_Value", duration).SetEase(Ease.Linear).OnComplete(() =>
          {
              if (holdAction != null)
                  holdAction();
              material.DOFloat(1, "_Value", duration).SetEase(Ease.Linear).OnComplete(()=>
              {
                  Destroy(gameObject);
              });
          });
    }
}
