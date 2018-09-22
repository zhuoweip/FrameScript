using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using DevelopEngine;
using System;


public class DoTweenManager:MonoSingleton<DoTweenManager>{

    Graphic graphic;
    public void DoFadeTween(Graphic _graphic, float _endValue, float _tweenTime, Action _action1 = null, bool isRewind = false, float _waitTime = 0, Action _action2 = null)
    {
        graphic.DOKill();
        graphic = _graphic;
        Tween tween = graphic.DOFade(_endValue, _tweenTime);
        tween.OnComplete(() =>
        {
            if (_action1 !=null)
                _action1();
            if (!isRewind)
                return;
            StartCoroutine(DoFadeRewind(_endValue, _tweenTime, _waitTime, _action2));
        });
    }

    IEnumerator DoFadeRewind(float _endValue,float _tweenTime, float _waitTime, Action _action)
    {
        yield return new WaitForSeconds(_waitTime);
        graphic.DOFade(1 - _endValue, _tweenTime).OnComplete(()=>
        {
            if (_action != null)
                _action();
        });
    }

    Transform trans;
    public void DoScaleTween(Transform _trans, float _endValue, float _tweenTime, Action _action1 = null, bool _isRewind = false, float _waitTime = 0, Action _action2 = null)
    {
        trans.DOKill();
        trans = _trans;
        Tween tween = trans.DOScale(_endValue, _tweenTime);
        tween.OnComplete(() =>
        {
            if (_action1 != null)
                _action1();
            if (!_isRewind)
                return;
            StartCoroutine(DoScaleRewind(_endValue, _tweenTime, _isRewind, _waitTime,_action2));
        });
    }

    IEnumerator DoScaleRewind(float _endValue, float _tweenTime, bool isRewind, float _waitTime, Action _action)
    {
        yield return new WaitForSeconds(_waitTime);
        if (isRewind)
        {
            trans.DOScale(1 - _endValue, _tweenTime).OnComplete(() =>
            {
                if (_action != null)
                    _action();
            });
        }
        else
        {
            if (_action != null)
                _action();
        }
    }

    public void DoScaleTween(Transform _trans, float _endValue, float _tweenTime)
    {
        _trans.DOScale(_endValue, _tweenTime).SetAutoKill(true);
    }
}
