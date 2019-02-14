using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 注意，这个代码可能会造成编辑时卡顿,崩溃，但是比较实用
/// </summary>
[CustomEditor(typeof(RectTransform))]
public class AutoAddScript : DecoratorEditor
{
    public AutoAddScript() : base("RectTransformEditor") { }
    public void OnEnable()
    {
        #region Text 自动添加ContentSizeFitter组件 
        //这里的target 是在Editor中的变量 也就是当前对象 要里式转换一下
        RectTransform rectTransform = target as RectTransform;
        Text text = rectTransform.GetComponent<Text>();
        ContentSizeFitter contentSizeFitter = rectTransform.GetComponent<ContentSizeFitter>();
        if (text != null && contentSizeFitter == null)
        {
            ContentSizeFitter contentSizeFitter_new = rectTransform.gameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter_new.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter_new.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        //else if (text == null && contentSizeFitter != null)
        //{
        //    Object.DestroyImmediate(contentSizeFitter);
        //}
        #endregion

        #region Raycast 自动关闭不需要的raycast
        RawImage rawImage = rectTransform.GetComponent<RawImage>();
        Image image = rectTransform.GetComponent<Image>();
        Button btn = rectTransform.GetComponent<Button>();
        if (btn == null)
        {
            if (rawImage != null && rawImage.raycastTarget)
                rawImage.raycastTarget = false;
            if (image != null && image.raycastTarget)
                image.raycastTarget = false;
        }
        else
        {
            if (rawImage != null && !rawImage.raycastTarget)
                rawImage.raycastTarget = true;
            if (image != null && !image.raycastTarget)
                image.raycastTarget = true;
        }
        #endregion

        #region Scroll View 自动关闭scrollbar，设置mask锚点，添加content组件
        ScrollRect scrollRect = rectTransform.GetComponent<ScrollRect>();
        if (scrollRect!=null)
        {
            scrollRect.horizontalScrollbar = null;
            scrollRect.verticalScrollbar = null;
            Scrollbar[] scrollBars = scrollRect.GetComponentsInChildren<Scrollbar>();
            for (int i = 0; i < scrollBars.Length; i++)
                scrollBars[i].gameObject.SetActive(false);

            Mask mask = scrollRect.GetComponentInChildren<Mask>();
            mask.rectTransform.anchorMin = Vector2.one / 2;
            mask.rectTransform.anchorMax = Vector2.one / 2;
            mask.rectTransform.pivot = Vector2.one / 2;
            if (scrollRect.horizontalScrollbar == null)
                mask.rectTransform.SetSizeDelta(scrollRect.GetComponent<RectTransform>().sizeDelta);

            Transform content = mask.transform.Find("Content");
            ContentSizeFitter contentSizeFitter_new = content.GetComponent<ContentSizeFitter>();
            if (contentSizeFitter_new == null)
            {
                ContentSizeFitter contentSizeFitter_new_add = content.gameObject.AddComponent<ContentSizeFitter>();
                contentSizeFitter_new_add.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                contentSizeFitter_new_add.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }
        #endregion
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RectTransform rectTransform = target as RectTransform;
        ScrollRect scrollRect = rectTransform.GetComponent<ScrollRect>();
        if (scrollRect!= null)
        {
            Mask mask = scrollRect.GetComponentInChildren<Mask>();
            Transform content = mask.transform.Find("Content");
            HorizontalLayoutGroup horizontalLayoutGroup = content.GetComponent<HorizontalLayoutGroup>();
            VerticalLayoutGroup verticalLayoutGroup = content.GetComponent<VerticalLayoutGroup>();

            if (scrollRect.horizontal && !scrollRect.vertical)
            {
                if (verticalLayoutGroup != null)
                    Object.DestroyImmediate(verticalLayoutGroup);
                if (horizontalLayoutGroup == null)
                {
                    HorizontalLayoutGroup horizontalLayoutGroup_new = content.gameObject.AddComponent<HorizontalLayoutGroup>();
                    horizontalLayoutGroup_new.spacing = 10;
                }
            }
            else if (scrollRect.vertical && !scrollRect.horizontal)
            {
                if (horizontalLayoutGroup != null)
                    Object.DestroyImmediate(horizontalLayoutGroup);
                if (verticalLayoutGroup == null)
                {
                    VerticalLayoutGroup verticalLayoutGroup_new = content.gameObject.AddComponent<VerticalLayoutGroup>();
                    verticalLayoutGroup_new.spacing = 10;
                }
            }
        }
    }
}