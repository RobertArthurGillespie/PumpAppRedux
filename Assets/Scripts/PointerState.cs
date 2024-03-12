using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerState : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{
    public bool pointerIsUp = false;
    public float pointerDownValveValue = 0f;
    public float pointerUpValveValue = 0f;
    public Slider ValveSlider;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerIsUp = false;
        pointerDownValveValue = ValveSlider.value;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerIsUp = true;
        pointerUpValveValue= ValveSlider.value;
    }

 
}
