using UnityEngine;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour /*IPointerEnterHandler, IPointerExitHandler*/
{
    public GameObject firstSelectedObject;

    // Start is called before the first frame update
    void Start()
    {
        SetFirstSelected();
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.current.SetSelectedGameObject(InputManager.instance.LastSelected);
    }

    public void SetFirstSelected()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedObject);
        InputManager.instance.LastSelected = firstSelectedObject;
    }
}
