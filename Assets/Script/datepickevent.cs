using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class datepickevent : MonoBehaviour, IPointerClickHandler
{
     public UnityEvent onClickEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent.Invoke();
    }
}
