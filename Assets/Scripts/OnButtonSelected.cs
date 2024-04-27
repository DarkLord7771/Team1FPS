using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OnButtonSelected : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{


    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject.GetComponentInParent<MenuNavigation>().firstSelectedObject);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShopManager.instance.LastSelectedButton = gameObject;

        for (int i = 0; i < ShopManager.instance.buttons.Count; i++)
        {
            if (ShopManager.instance.buttons[i] == gameObject)
            {
                ShopManager.instance.LastSelectedIndex = i;
                return;
            }
        }
    }
}
