using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    public GameObject button;
    //public Button newButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = button;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputManager.instance.LastSelected = eventData.selectedObject;
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        InputManager.instance.LastSelected = eventData.selectedObject;
    }
}
