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

    public PlayerState currentPlayerState = PlayerState.None;
    public int numOfLives = 3;

    public UnityEvent OnPlayerDeath;

    private GameManager manager;


    private void Start()
    {
        OnPlayerDeath.AddListener(HandleDeathEvent);

        manager = GameManager.Instance;
        manager.OnGameStateChanged.AddListener(HandleGameStateChange);
    }

    void Update()
    {
        if (numOfLives <= 0 && currentPlayerState != PlayerState.Ghost)
        {
            currentPlayerState = PlayerState.Ghost;
            OnPlayerDeath.Invoke();
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
            case GameState.InMenu:
                break;
            case GameState.LifeBubbleSpawn:
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

    private void HandleDeathEvent()
    {

    }
}
