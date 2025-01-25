using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] LifeSpawnSelector _lifeSpawnSelector;
    [SerializeField] Camera _camera;
    public LifeSpawnSelector lifeSpawnSelector => _lifeSpawnSelector;
    
    public Movement movement;
    private Shoot shoot;
    public int playerId;
    
    int _numOfLives = LifeSpawnSelector.MAX_LIVES;
    public int numOfLives
    {
        get => _numOfLives;
        set
        {
            int oldNumOfLives = numOfLives;
            _numOfLives = Mathf.Max(value, 0);

            if (oldNumOfLives == _numOfLives)
            {
                return;
            }
            
            _lifeSpawnSelector.lifeBar.SetLives(numOfLives);

            if (_numOfLives != 0)
            {
                return;
            }
            
            movement.isGhostMode = true;
            onPlayerDeath.Invoke();
        }
    }

    public UnityEvent onPlayerDeath;
    
    PlayerInput _playerInput;
    public PlayerInput playerInput => _playerInput;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        shoot = movement.GetComponent<Shoot>();
    }
    
    void Start()
    {
        onPlayerDeath.AddListener(HandleDeathEvent);
        GameManager.Instance?.OnGameStateChanged.AddListener(HandleGameStateChange);
    }

    /// <summary>
    /// Called when the game state changes.
    /// 
    /// TODO: Needs a bit of thinking what 
    /// </summary>
    /// <param name="gameState"></param>
    public void HandleGameStateChange(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.LifeBubbleSpawn:
                _camera.gameObject.SetActive(false);
                movement.gameObject.SetActive(false);
                break;
            case GameState.OnGoing:
                _camera.GetComponent<UniversalAdditionalCameraData>().volumeLayerMask |= (1 << LayerMask.NameToLayer("P" + (playerId + 1)));              
                movement.gameObject.SetActive(true);
                _camera.gameObject.SetActive(true);
                break;
            case GameState.Ended:
                movement.gameObject.SetActive(false);
                break;
        }
    }

    private void HandleDeathEvent()
    {
        FindFirstObjectByType<VisualEffectController>().ActivateDeadState(playerId);
        //Change sprite to dead sprite
        Debug.Log("Player " + playerId + " is dead");
    }
    
    // Life Spawn
    
    void OnSelect1() => SelectSpawn(0);
    void OnSelect2() => SelectSpawn(1);
    void OnSelect3() => SelectSpawn(2);
    void OnSelect4() => SelectSpawn(3);
    void OnSelect5() => SelectSpawn(4);
    void OnSelect6() => SelectSpawn(5);
    void OnSelect7() => SelectSpawn(6);
    void OnSelect8() => SelectSpawn(7);

    void SelectSpawn(int index)
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        
        if (GameManager.Instance.currentGameState != GameState.LifeBubbleSpawn)
        {
            return;
        }
        
        _lifeSpawnSelector.SelectSpawn(index);
    }
    
    void OnTryStartGame()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        
        if (GameManager.Instance.currentGameState != GameState.LifeBubbleSpawn)
        {
            return;
        }
        
        _lifeSpawnSelector.TryStartGame();
    }

    void OnUndo()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        
        if (GameManager.Instance.currentGameState != GameState.LifeBubbleSpawn)
        {
            return;
        }
        
        if (!_lifeSpawnSelector.TryUndo())
        {
            // PlayerInputManager.OnPlayerLeft gets triggered
            // when GameObject with PlayerInput is destroyed or disabled
            Destroy(gameObject);
        }
    }
    
    // Movement
    
    void OnMove(InputValue value)
    {
        movement.SetMove(value);
    }

    void OnJump(InputValue value)
    {
        movement.SetJump(value);
    }

    void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            shoot.ShootBubble();
        }
    }
}
