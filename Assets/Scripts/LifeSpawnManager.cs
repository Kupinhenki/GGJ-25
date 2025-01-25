using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LifeSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _joinText;
    [SerializeField] GameObject _startText;
    [SerializeField] Camera _mainCamera;
    [SerializeField] SpriteRenderer _mapBounds;
    [SerializeField] GameObject[] _locationIconPrefabs = new GameObject[8];
    
    readonly List<PlayerController> _players = new();
    readonly HashSet<LifeSpawnSelector> _readySelectors = new();
    
    bool isReadyToStart => _readySelectors.Count == _players.Count && _players.Count > 1;

    void Awake()
    {
        Vector3 pos = _mapBounds.transform.position;
        pos.z = _mainCamera.transform.position.z;
        _mainCamera.transform.position = pos;
    }

    void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += PlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += PlayerLeft;
        _startText.gameObject.SetActive(isReadyToStart);
        _joinText.gameObject.SetActive(_players.Count == 0);
        
        for (int i = 0; i < GameManager.Instance.bubbleLocations.Length; i++)
        {
            Transform location = GameManager.Instance.bubbleLocations[i];
            GameObject locationIcon = Instantiate(_locationIconPrefabs[i], transform);
            locationIcon.transform.position = location.position;
        }
    }

    void LateUpdate()
    {
        float screenRatio = (float)Screen.width / Screen.height;
            
        Bounds bounds = _mapBounds.bounds;
        float targetRatio = bounds.size.x / bounds.size.y;
        if (screenRatio >= targetRatio)
        {
            _mainCamera.orthographicSize = bounds.size.y / 2;
        }
        else
        {
            float scale = targetRatio / screenRatio;
            _mainCamera.orthographicSize = bounds.size.y / 2 * scale;
        }
    }

    void PlayerJoined(PlayerInput playerInput)
    {
        var player = playerInput.GetComponent<PlayerController>();
        player.HandleGameStateChange(GameManager.Instance.currentGameState);
        _players.Add(player);
        player.lifeSpawnSelector.onStartGamePressed += TryStartGame;
        player.lifeSpawnSelector.onSelectionChanged += SelectionChanged;
        SelectionChanged(player.lifeSpawnSelector);
        
        PlayerCountChanged();
        
        _joinText.gameObject.SetActive(false);
    }

    void PlayerLeft(PlayerInput playerInput)
    {
        var player = playerInput.GetComponent<PlayerController>();
        player.lifeSpawnSelector.onSelectionChanged -= SelectionChanged;
        player.lifeSpawnSelector.onStartGamePressed -= TryStartGame;
        _readySelectors.Remove(player.lifeSpawnSelector);
        _players.Remove(player);
        
        PlayerCountChanged();

        if (_players.Count == 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void SelectionChanged(LifeSpawnSelector selector)
    {
        if (selector.selection.Count == LifeSpawnSelector.MAX_LIVES)
        {
            _readySelectors.Add(selector);
        }
        else
        {
            _readySelectors.Remove(selector);
        }

        _startText.gameObject.SetActive(isReadyToStart);
    }

    void TryStartGame()
    {
        if (!isReadyToStart)
        {
            return;
        }

        _startText.gameObject.SetActive(false);
        _mainCamera.cullingMask = 0;
        PlayerInputManager.instance.splitScreen = true;
        
        var data = new GameManager.PlayerBubbleData[_players.Count];
        for (int i = 0; i < _players.Count; i++)
        {
            PlayerController player = _players[i];
            player.playerInput.SwitchCurrentActionMap("Player");
            data[i] = new GameManager.PlayerBubbleData(player, player.lifeSpawnSelector.selection.ToArray());
        }
        
        PlayerInputManager.instance.onPlayerJoined -= PlayerJoined;
        PlayerInputManager.instance.onPlayerLeft -= PlayerLeft;
        
        GameManager.Instance.AddPlayers(data);
        gameObject.SetActive(false);
        Destroy(this);
    }

    void PlayerCountChanged()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].lifeSpawnSelector.UpdateLifeBarPosition(i);
        }
    }
}
