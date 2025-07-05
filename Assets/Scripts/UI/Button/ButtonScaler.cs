using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float hoverScale = 1.1f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Use SetUpdate(true) to prevent this animation from being affected by Time.timeScale
        transform.DOScale(originalScale * hoverScale, 0.2f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);  // Ignore Time.timeScale
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Use SetUpdate(true) to prevent this animation from being affected by Time.timeScale
        transform.DOScale(originalScale, 0.2f)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true);  // Ignore Time.timeScale
    }
}
