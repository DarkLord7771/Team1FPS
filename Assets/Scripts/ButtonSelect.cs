using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public GameObject button;



    //public Button newButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = null;
        eventData.selectedObject = button;
        InputManager.instance.LastSelected = eventData.selectedObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        InputManager.instance.LastSelected = eventData.selectedObject;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        InputManager.instance.LastSelected = eventData.selectedObject;
    }
}
