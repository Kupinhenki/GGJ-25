using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class LifeBar : MonoBehaviour
    {
        [SerializeField] LifeIndicator _lifeIndicatorPrefab;
        
        readonly List<LifeIndicator> _lifeIndicators = new();
        
        float _hueOffset;
        float _notSelectedHueOffset;

        public void SetLives(int maxLives)
        {
            while (_lifeIndicators.Count > maxLives)
            {
                int lastIndex = _lifeIndicators.Count - 1;
                Destroy(_lifeIndicators[lastIndex].gameObject);
                _lifeIndicators.RemoveAt(lastIndex);
            }
            
            while (transform.childCount < maxLives)
            {
                LifeIndicator lifeIndicator = Instantiate(_lifeIndicatorPrefab, transform);
                _lifeIndicators.Add(lifeIndicator);
                lifeIndicator.UpdateHueOffset(_hueOffset, _notSelectedHueOffset);
            }
        }

        public void SetSelectedLives(int lives)
        {
            if (lives > _lifeIndicators.Count)
            {
                SetLives(lives);
            }
            
            for (int i = 0; i < _lifeIndicators.Count; i++)
            {
                _lifeIndicators[i].SetIsSelected(i < lives);
            }
        }
        
        public void UpdateHueOffset(float offset, float notSelectedOffset)
        {
            _hueOffset = offset;
            _notSelectedHueOffset = notSelectedOffset;
            foreach (LifeIndicator lifeIndicator in _lifeIndicators)
            {
                lifeIndicator.UpdateHueOffset(_hueOffset, _notSelectedHueOffset);
            }
        }
    }
}