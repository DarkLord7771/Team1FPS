using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuNavigation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{

    public GameObject firstSelectedObject;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(eventData.selectedObject == null)
        {
            eventData.selectedObject = firstSelectedObject;
        }
    }

}
