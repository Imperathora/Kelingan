using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event System.Action OnSelected;
    private bool m_selected;

    public void OnSelect(BaseEventData eventData)
    {
        if (!m_selected && !Cursor.visible)
            OnSelected.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_selected = true;
        OnSelected.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_selected = false;
    }
}
