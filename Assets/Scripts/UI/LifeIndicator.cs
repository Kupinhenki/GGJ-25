using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class LifeIndicator : MonoBehaviour
    {
        [SerializeField] Sprite _notSelected;
        [SerializeField] Sprite _selected;

        Image _img;

        bool _isSelected;

        void Awake()
        {
            _img = GetComponent<Image>();
            SetIsSelected(_isSelected);
        }

        public void SetIsSelected(bool isSelected)
        {
            _isSelected = isSelected;
            _img.sprite = _isSelected ? _selected : _notSelected;
        }
    }
}