using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GameManager;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        None,
        // Might not be needed for anything
        ChoosingBubbleSpawns,
        Playing,
        Ghost
    }

    public GameObject playerVisualization;
    public GameObject ghostVisualization;

    public PlayerState currentPlayerState = PlayerState.None;
    public int numOfLives = 3;

    public UnityEvent OnPlayerDeath;
    public UnityEvent<PlayerState> OnPlayerStateChanged;

    private GameManager manager;


    private void Start()
    {
        OnPlayerDeath.AddListener(HandleDeathEvent);
        OnPlayerStateChanged.AddListener(HandlePlayerStateChange);

        manager = GameManager.Instance;
        manager.OnGameStateChanged.AddListener(HandleGameStateChange);
    }

    void Update()
    {
        if (numOfLives <= 0 && currentPlayerState != PlayerState.Ghost)
        {
            currentPlayerState = PlayerState.Ghost;
            HandlePlayerStateChange(currentPlayerState);
        }

        switch (currentPlayerState)
        {
            case PlayerState.None: 
                break;
            case PlayerState.ChoosingBubbleSpawns:
                break;
            case PlayerState.Playing:
                break;
            case PlayerState.Ghost:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Called when the game state changes.
    /// 
    /// TODO: Needs a bit of thinking what 
    /// </summary>
    /// <param name="gameState"></param>
    private void HandleGameStateChange(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.None:
                break;
            case GameState.LifeBubbleSpawn:
                playerVisualization.SetActive(false);
                ghostVisualization.SetActive(false);
                break;
            case GameState.OnGoing:
                currentPlayerState = PlayerState.Playing;
                break;
            case GameState.Paused:
                break;
            case GameState.Ended:
                currentPlayerState = PlayerState.None;
                break;
            default:
                break;
        }
    }

    private void HandlePlayerStateChange(PlayerState playerState)
    {
        switch(playerState)
        {
            case PlayerState.None:
                break;
            case PlayerState.ChoosingBubbleSpawns:
                // T‰h‰n valinta controlleri p‰‰lle
                break;
            case PlayerState.Playing:
                // T‰h‰n perus movement controlleri p‰‰lle
                playerVisualization.SetActive(true);
                break;
            case PlayerState.Ghost:
                // T‰h‰n ghost controlleri p‰‰lle
                playerVisualization.SetActive(false);
                ghostVisualization.SetActive(true);
                OnPlayerDeath.Invoke();
                break;
            default:
                break;
        }
    }

    private void HandleDeathEvent()
    {

    }
}
