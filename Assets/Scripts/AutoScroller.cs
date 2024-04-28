using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroller : MonoBehaviour
{
    [Header("----- Settings Variable -----")]
    [SerializeField] float scrollSpeed = 10f;
    [SerializeField] float yPadding = 20f;

    
    RectTransform LayoutListGroup
    {
        get { return ScrollRect != null ? ScrollRect.content : null; }
    }
    RectTransform ScrollWindow { get; set; }
    ScrollRect ScrollRect { get; set; }
    bool isManualScrolling;

    GameObject LastGameObject { get; set; }
    GameObject CurrentGameObject
    {
        get { return EventSystem.current.currentSelectedGameObject; }
    }
    RectTransform CurrentRectTransform { get; set; }

    private void Awake()
    {
        ScrollRect = GetComponent<ScrollRect>();
        ScrollWindow = ScrollRect.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Update current and last game objects.
        UpdateGameObjects();

        // Check if manually scrolling.
        LockedScrollingEnabled();

        // Move Scroll Rect
        ScrollToSelection();
    }

    private void UpdateGameObjects()
    {
        if (CurrentGameObject != LastGameObject)
        {
            CurrentRectTransform = (CurrentGameObject != null) ? CurrentGameObject.GetComponent<RectTransform>() : null;

            // Automatically scroll if not manually scrolling.
            if (CurrentGameObject != null && CurrentGameObject.transform.parent == LayoutListGroup.transform)
            {
                isManualScrolling = false;
            }
        }

        LastGameObject = CurrentGameObject;
    }

    private void LockedScrollingEnabled()
    {
        if (isManualScrolling == true)
        {
            return;
        }
    }

    private void ScrollToSelection()
    {
        bool hasReferences = (ScrollRect == null || LayoutListGroup == null || ScrollWindow == null);

        if (hasReferences == true || isManualScrolling == true)
        {
            return;
        }

        RectTransform selection = CurrentRectTransform;

        // Check if scrolling is possible.
        if (selection == null || selection.transform.parent != LayoutListGroup.transform)
        {
            return;
        }

        UpdateScrollPosition(selection);
    }

    private void UpdateScrollPosition(RectTransform selection)
    {
        // Move to selected position.
        float selectionPos = -selection.anchoredPosition.y - (selection.rect.height * (1 - selection.pivot.y) - yPadding);

        float selectionHeight = selection.rect.height;
        float windowHeight = ScrollWindow.rect.height;
        float anchorPosition = LayoutListGroup.anchoredPosition.y;

        // Get the offset based off of mouse position.
        float offlimitsValue = ScrollOffset(selectionPos, anchorPosition, selectionHeight, windowHeight);

        // Move scroll rect.
        ScrollRect.verticalNormalizedPosition += (offlimitsValue / LayoutListGroup.rect.height) * Time.unscaledDeltaTime * scrollSpeed;
    }

    private float ScrollOffset(float position, float listPosition, float targetLength, float maskLength)
    {
        if (position < listPosition + (targetLength / 2))
        {
            return (listPosition + maskLength) - (position - targetLength);
        }
        else if (position + targetLength > listPosition + maskLength)
        {
            return (listPosition + maskLength) - (position + targetLength);
        }

        return 0;
    }
}
