using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class LifeIndicator : MonoBehaviour
    {
        static readonly int _OFFSET = Shader.PropertyToID("_Offset");
        [SerializeField] Sprite _notSelected;
        [SerializeField] Sprite _selected;

        Image _img;

        bool _isSelected;

        void Awake()
        {
            _img = GetComponent<Image>();
            _img.material = Instantiate(_img.material);
            SetIsSelected(_isSelected);
        }

        public void SetIsSelected(bool isSelected)
        {
            _isSelected = isSelected;
            _img.sprite = _isSelected ? _selected : _notSelected;
        }

        public void UpdateHueOffset(float offset)
        {
            _img.material.SetFloat(_OFFSET, offset);
        }
    }
}