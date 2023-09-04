using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
     private bool isScrolling;
    private ScrollRect scrollRect;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isScrolling = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isScrolling = false;
    }

    private void Update()
    {
        if (isScrolling)
        {
            float scrollDirection = Input.GetAxis("Mouse Y"); // You can use Input.touches for Android touch input
            scrollRect.verticalNormalizedPosition += scrollDirection * 0.1f; // Adjust the scroll speed as needed
        }
    }
}