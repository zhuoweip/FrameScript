using JacobGames.SuperInvoke;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    public delegate void EventDataDelegate(GameObject go, PointerEventData eventData);
    public EventDataDelegate onDragEventData;
    public EventDataDelegate onBeginDragEventData;
    public EventDataDelegate onEndDragEventData;

    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDoubleClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onDownEffect;
    public VoidDelegate onUpEffect;
    public VoidDelegate onBeginDrag;
    public VoidDelegate onDrag;
    public VoidDelegate onEndDrag;


    public VoidDelegate onBeforeClick;
    public VoidDelegate onAfterClick;


    // 延迟时间，连续按的间隔时间 
    protected float delayTime = 0.2f;

    // 按钮最后一次是被按住状态时候的时间  
    private float startIsDownTime;

    private bool isDelayTime = true;

    static public EventTriggerListener Get(Component go, bool isDelayTime = true, float delayTime = 0.2f)
    {
        return Get(go.gameObject, isDelayTime, delayTime);
    }

    static public EventTriggerListener Get(GameObject go, bool isDelayTime = true, float delayTime = 0.2f)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
        {
            listener = go.AddComponent<EventTriggerListener>();
        }
        listener.startIsDownTime = 0;
        listener.delayTime = delayTime;
        listener.isDelayTime = isDelayTime;
        return listener;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(gameObject);
        if (onBeginDragEventData != null) { onBeginDragEventData(gameObject, eventData); }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject);
        if (onDragEventData != null) { onDragEventData(gameObject, eventData); }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(gameObject);
        if (onEndDragEventData != null) { onEndDragEventData(gameObject, eventData); }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        //if (isDelayTime)
        //{
            string tag = SuperInvoke.CreateTag();
            SuperInvoke.Run(Click, delayTime, tag);
            if (Time.time - startIsDownTime < delayTime && startIsDownTime!= 0)
            {
                if (onDoubleClick != null) onDoubleClick(gameObject);
                SuperInvoke.KillAll();
            }
            startIsDownTime = Time.time;
        //}
        //else
        //{
        //    if (onClick != null)
        //    {
        //        if (onBeforeClick != null) onBeforeClick(gameObject);
        //        onClick(gameObject);
        //        if (onAfterClick != null) onAfterClick(gameObject);
        //    }
        //}
    }

    private void Click()
    {
        if (onClick != null)
        {
            if (onBeforeClick != null) onBeforeClick(gameObject);
            onClick(gameObject);
            if (onAfterClick != null) onAfterClick(gameObject);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null)
        {
            if (onBeforeClick != null) onBeforeClick(gameObject);
            if (onDownEffect != null) onDownEffect(gameObject);
            onDown(gameObject);
            if (onAfterClick != null) onAfterClick(gameObject);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUpEffect != null) onUpEffect(gameObject);
        if (onUp != null) onUp(gameObject);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
    }


}

