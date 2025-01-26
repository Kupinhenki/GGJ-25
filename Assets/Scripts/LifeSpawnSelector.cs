using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class LifeSpawnSelector : MonoBehaviour
{
    [SerializeField] LifeBar _lifeBar;
    public LifeBar lifeBar => _lifeBar;
    
    public readonly List<int> selection = new();
    public Action<LifeSpawnSelector> onSelectionChanged;
    public Action onStartGamePressed;

    public const int MAX_LIVES = 3;
    public const int SPAWN_POSITIONS = 8;

    void Awake()
    {
        _lifeBar.SetLives(MAX_LIVES);
    }

    public bool TryUndo()
    {
        if (selection.Count == 0)
        {
            return false;
        }
        
        AudioManager.Instance.PlaySoundFromAnimationEvent("Select");
        
        selection.RemoveAt(selection.Count - 1);
        SelectionChanged();

        return true;
    }

    public void TryStartGame()
    {
        onStartGamePressed?.Invoke();
    }
    
    public void UpdateLifeBar(int index, float hueOffset)
    { 
        _lifeBar.UpdateHueOffset(hueOffset);
        
        var rectTransform = _lifeBar.transform as RectTransform;
        if (rectTransform == null)
        {
            return;
        }
        
        switch (index)
        {
            case 0:
                rectTransform.pivot = new Vector2(0, 1);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
            case 1:
                rectTransform.pivot = new Vector2(1, 1);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
            case 2:
                rectTransform.pivot = new Vector2(0, 0);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
            case 3:
                rectTransform.pivot = new Vector2(1, 0);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
        }
    }
    
    public void SelectSpawn(int index)
    {
        if (selection.Count >= MAX_LIVES || index >= SPAWN_POSITIONS)
        {
            return;
        }
        
        AudioManager.Instance.PlaySoundFromAnimationEvent("Plop");
        selection.Add(index);
        SelectionChanged();
    }

    void SelectionChanged()
    {
        onSelectionChanged?.Invoke(this);
        _lifeBar.SetSelectedLives(selection.Count);
    }
}
