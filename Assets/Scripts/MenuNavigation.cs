using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{
    public GameObject firstSelectedObject;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedObject);
        InputManager.instance.LastSelected = firstSelectedObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame && EventSystem.current.currentSelectedGameObject == null)
        {
            if (InputManager.instance.LastSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(InputManager.instance.LastSelected);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(firstSelectedObject);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputManager.instance.LastSelected = eventData.selectedObject;
        eventData.selectedObject = null;
    }


    public void OnDeselect(BaseEventData eventData)
    {
        if (Gamepad.current != null && !Gamepad.current.wasUpdatedThisFrame && EventSystem.current.currentSelectedGameObject == null)
        {
            eventData.selectedObject = InputManager.instance.LastSelected;
        }
    }
}
