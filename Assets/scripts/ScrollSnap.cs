using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ScrollSnap : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public int pageCount = 2;

    private float[] pagePositions;
    private bool isLerping = false;
    private float targetPos;

    void Start()
    {
        pagePositions = new float[pageCount];
        for (int i = 0; i < pageCount; i++)
        {
            pagePositions[i] = i / (float)(pageCount - 1); // 0.0 to 1.0
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float nearest = float.MaxValue;
        foreach (var pos in pagePositions)
        {
            if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - pos) < Mathf.Abs(scrollRect.horizontalNormalizedPosition - nearest))
            {
                nearest = pos;
            }
        }

        targetPos = nearest;
        isLerping = true;
    }

    void Update()
    {
        if (isLerping)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetPos, Time.deltaTime * 10f);
            if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetPos) < 0.001f)
            {
                scrollRect.horizontalNormalizedPosition = targetPos;
                isLerping = false;
            }
        }
    }
}
