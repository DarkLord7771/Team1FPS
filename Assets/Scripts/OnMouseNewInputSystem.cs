using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseNewInputSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(this.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print(this.name);
    }
}
