using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour /*IPointerEnterHandler, IPointerExitHandler*/
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
        EventSystem.current.SetSelectedGameObject(InputManager.instance.LastSelected);
    }
}
