using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UIScroller : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{

    public ScrollRect scrollRect;
    public float scrollRectPosition = 1;
    [SerializeField] GameObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>(true);

        int itemCount = scrollRect.content.transform.childCount - 1;
        int itemIndex = transform.GetSiblingIndex();
        

        itemIndex = itemIndex < ((float)itemCount / 2) ? itemIndex - 1 : itemIndex;

        scrollRectPosition = 1 - ((float)itemIndex / itemCount);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (scrollRect)
        {
            scrollRect.verticalScrollbar.value = scrollRectPosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = parentObject;
    }
}
