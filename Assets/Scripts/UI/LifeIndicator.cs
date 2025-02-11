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
        
        float _hueOffset;
        float _notSelectedHueOffset;

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
            UpdateMaterial();
        }

        public void UpdateHueOffset(float offset, float notSelectedOffset)
        {
            _hueOffset = offset;
            _notSelectedHueOffset = notSelectedOffset;
            UpdateMaterial();
        }

        void UpdateMaterial()
        {
            _img.material.SetFloat(_OFFSET, _isSelected ? _hueOffset : _notSelectedHueOffset);
        }
    }
}