using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public GameObject button;

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
