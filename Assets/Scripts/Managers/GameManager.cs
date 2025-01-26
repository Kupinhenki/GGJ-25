using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    LifeBubbleSpawn,
    OnGoing,
    Paused,
    Ended
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        Instance = null;
    }
#endif

    public struct PlayerBubbleData
    {
        public PlayerController controller;
        public int[] bubbleSpawnIndexes;

        public PlayerBubbleData(PlayerController _controller, int[] _array)
        {
            controller = _controller;
            bubbleSpawnIndexes = _array;
        }
    }

    public GameState currentGameState { get; private set; } = GameState.LifeBubbleSpawn;

    private int maxNumOfPlayers = 4;
    private int minNumOfPlayers = 2;
    private int maxNumOfBubbles = 3;

    private int numOfPlayersAlive = 0;

    private List<PlayerBubbleData> players = new List<PlayerBubbleData>();

    /// <summary>
    /// Add positions for the life bubble spawns.
    /// </summary>
    public Transform[] bubbleLocations;

    public GameObject lifeBubblePrefab;

    public UnityEvent<GameState> OnGameStateChanged;

    public GameObject endScreenCanvas;

    public Transform[] playerSpawnLocations;

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
        OnGameStateChanged.AddListener(HandleGameStateChanged);
        SwitchGameState(currentGameState);
    }

    /// <summary>
    /// Switches the current game state.
    /// </summary>
    /// <param name="newState"></param>
    public void SwitchGameState(GameState newState)
    {
        currentGameState = newState;
        OnGameStateChanged.Invoke(currentGameState);
    }


    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.LifeBubbleSpawn:
                break;
            case GameState.OnGoing:
                numOfPlayersAlive = players.Count;
                SpawnLifeBubbles();
                break;
            case GameState.Paused:
                // Show pause screen
                break;
            case GameState.Ended:
                // Show end screen
                if(endScreenCanvas != null)
                {
                    endScreenCanvas.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Spawns all the life bubbles based on the cached player choices.
    /// </summary>
    /// <param name="playerStruct"></param>
    public void SpawnLifeBubbles()
    {
        for(int i = 0; i < players.Count; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                SpawnLifeBubble(players[i].bubbleSpawnIndexes[j], players[i].controller);
            }
        }
    }
    
    /// <summary>
    /// Spawns a life bubble at the specified spawn location.
    /// </summary>
    /// <param name="index"></param>
    private GameObject SpawnLifeBubble(int index, PlayerController creator)
    {
        GameObject instantiatedPrefab = Instantiate(lifeBubblePrefab, bubbleLocations[index].position, Quaternion.identity);
        bubbleLocations[index].GetComponent<BubblePointHandler>().orbsList.Add(instantiatedPrefab);
        var lifeBubble = instantiatedPrefab.GetComponent<LifeBubble>();
        lifeBubble.pointHandler = bubbleLocations[index].GetComponent<BubblePointHandler>();
        lifeBubble.owner = creator;
        lifeBubble.spriteRenderer.material.SetFloat("_Offset", creator.GetLifeBubbleHueOffset(creator.playerId));
        return instantiatedPrefab;
    }
    
    /*
    // T�t� pit�� v�h�n pohtia, pit�� mm kattoa miten me lis�t��n ne pelaajat johonkin listaan ennen ku ne spawnataan kent�lle
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
    */

    public void LoseLife(PlayerController player)
    {
        player.numOfLives--;
        StartCoroutine(player.BubblePopped());   
    }

    public void AddPlayers(PlayerBubbleData[] bubbleDatas)
    {
        for (int i = 0; i < bubbleDatas.Length; i++)
        {
            PlayerBubbleData data = bubbleDatas[i];
            players.Add(data);
            data.controller.playerId = i;
            data.controller.onPlayerDeath.AddListener(DoSomethingWhenPlayerDies);
            data.controller.movement.transform.position = playerSpawnLocations[i].position;
            data.controller.movement.GetComponent<Rigidbody2D>().position = playerSpawnLocations[i].position;
            data.controller.movement.spawnLocation = playerSpawnLocations[i];
        }

        SwitchGameState(GameState.OnGoing);
    }

    private void DoSomethingWhenPlayerDies()
    {
        // I'm doing something
        numOfPlayersAlive--;
        AudioManager.Instance.PlaySoundFromAnimationEvent("Death");

        if(currentGameState == GameState.OnGoing && numOfPlayersAlive <= 1)
        {
            // Pit�� ehk� teh� jotain muutakin kun peli loppuu
            SwitchGameState(GameState.Ended);
        }
    }
}