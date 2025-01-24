using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        None,
        InMenu,
        LifeBubbleSpawn,
        OnGoing,
        Paused,
        Ended
    }

    private GameState currentGameState = GameState.None;

    private int maxNumOfPlayers = 4;
    private int minNumOfPlayers = 2;

    private int maxNumOfBubbles = 3;

    private int currentNumOfPlayers;
    private int numOfPlayersAlive;

    private List<PlayerController> players = new List<PlayerController>();

    /// <summary>
    /// Add positions for the life bubble spawns.
    /// </summary>
    public Transform[] bubbleLocations;

    public GameObject lifeBubblePrefab;
    public GameObject playerPrefab;

    public UnityEvent<GameState> OnGameStateChanged;

    /// <summary>
    /// Singleton initialization
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(currentGameState == GameState.OnGoing && numOfPlayersAlive <= 1)
        {
            // Pitää ehkä tehä jotain muutakin kun peli loppuu
            SwitchGameState(GameState.Ended);
        }

        switch(currentGameState)
        {
            case GameState.None:
                break;
            case GameState.InMenu:
                break;
            case GameState.LifeBubbleSpawn:
                break;
            case GameState.OnGoing:
                break;
            case GameState.Paused:
                break;
            case GameState.Ended:
                break;
            default:
                break;
        }
    }

    public void SwitchGameState(GameState state)
    {
        currentGameState = state;
        OnGameStateChanged.Invoke(currentGameState);
    }

    /// <summary>
    /// Spawns a life bubble at the specified spawn location.
    /// </summary>
    /// <param name="index"></param>
    private GameObject SpawnLifeBubble(int index, int creatorIndex)
    {
        GameObject instantiatedPrefab = Instantiate(lifeBubblePrefab, bubbleLocations[index].position, Quaternion.identity);
        instantiatedPrefab.GetComponent<LifeBubble>().owner = players[creatorIndex];
        return instantiatedPrefab;
    }

    /// <summary>
    /// Spawns all the life bubbles based on the cached player choices.
    /// </summary>
    /// <param name="playerStruct"></param>
    public void SpawnLifeBubbles(int[][] playerStruct)
    {
        for(int i = 0; i < currentNumOfPlayers; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                SpawnLifeBubble(playerStruct[i][j], i);
            }
        }
    }

    // Tätä pitää vähän pohtia, pitää mm kattoa miten me lisätään ne pelaajat johonkin listaan ennen ku ne spawnataan kentälle
    public void SpawnPlayers()
    {
        while(players.Count < currentNumOfPlayers)
        {
            PlayerController instantiatedPlayer = Instantiate(playerPrefab).GetComponent<PlayerController>();
            instantiatedPlayer.OnPlayerDeath.AddListener(DoSomethingWhenPlayerDies);
            players.Add(instantiatedPlayer);
        }

        numOfPlayersAlive = currentNumOfPlayers;
    }

    public void LoseLife(PlayerController player)
    {
        player.numOfLives--;
    }

    private void DoSomethingWhenPlayerDies()
    {
        // I'm doing something
    }
}