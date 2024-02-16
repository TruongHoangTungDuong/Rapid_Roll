using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;

    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);

    private Vector3 originalScale;

    void Start()
    {
        originalScale = button.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.transform.localScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.transform.localScale = originalScale;
    }
}
