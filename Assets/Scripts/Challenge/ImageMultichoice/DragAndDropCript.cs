using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropCript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Button btn;
    public RectTransform rect;
    public CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    public Vector2 position;
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        originalPosition = rect.anchoredPosition;
        Debug.Log("OnBeginDrag");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag.transform.parent.childCount == 4)
        {
            Vector2 p = new Vector2(145f, -360f);
            SetPosition(p);
        }
        else
        {
            SetPosition(originalPosition);
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta;
    }
   
   
    public void SetPosition(Vector2 position)
    {
        rect.anchoredPosition = position;
    }
   
}
