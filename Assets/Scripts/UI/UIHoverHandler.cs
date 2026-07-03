using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private UIEffect uiEffect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiEffect != null)
        {
            uiEffect.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiEffect != null)
        {
            uiEffect.enabled = false;
        }
    }

    private void OnDisable()
    {
        OnPointerExit(null);
    }
}
