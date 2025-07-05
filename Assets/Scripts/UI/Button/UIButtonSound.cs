using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    private readonly string hoverSound = "button-hover";
    private readonly string clickSound = "button-click";

    public void OnButtonHover()
    {
        AudioManager.Instance.PlaySFX(hoverSound);
    }

    public void OnButtonClick()
    {
        AudioManager.Instance.PlaySFX(clickSound);
    }
}
