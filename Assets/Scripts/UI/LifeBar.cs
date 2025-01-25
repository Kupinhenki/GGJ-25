using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class LifeBar : MonoBehaviour
    {
        [SerializeField] LifeIndicator _lifeIndicatorPrefab;
        
        readonly List<LifeIndicator> _lifeIndicators = new();

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
                _lifeIndicators.Add(Instantiate(_lifeIndicatorPrefab, transform));
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
    }
}