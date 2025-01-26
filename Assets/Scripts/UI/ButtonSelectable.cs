using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectable : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Sprite normalSprite;
    public Sprite selectSprite;
    private Image img;

    private void Start()
    {
        img = gameObject.GetComponent<Image>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(img == null)
        {
            img = gameObject.GetComponent<Image>();
        }
        img.sprite = selectSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (img == null)
        {
            img = gameObject.GetComponent<Image>();
        }
        img.sprite = normalSprite;
    }
}
