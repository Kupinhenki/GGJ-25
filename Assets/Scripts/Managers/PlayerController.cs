using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    static readonly int _OFFSET = Shader.PropertyToID("_Offset");
    [SerializeField] LifeSpawnSelector _lifeSpawnSelector;
    [SerializeField] Camera _camera;
    [SerializeField] Transform _background;
    [SerializeField] SpriteRenderer _playerSprite;
    [SerializeField] float[] _playerSpriteHueOffsets = new float[4] {
        0,
        0.5f,
        0.86f,
        0.59f
    };
    public LifeSpawnSelector lifeSpawnSelector => _lifeSpawnSelector;
    
    public Movement movement;
    private Shoot shoot;
    public int playerId;
    public bool playerInBubble;
    public Animator animator;

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
    
    GameManager _gameManager = null;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        shoot = movement.GetComponent<Shoot>();
        playerInBubble = false;

        _gameManager = GameManager.Instance;

        if (_gameManager != null)
        {
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChange);
        }
    }
    
    void Start()
    {
        onPlayerDeath.AddListener(HandleDeathEvent);
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChange);
        }
    }

    private void Update()
    {
        if (playerInBubble)
        {
            animator.SetBool("InBubble", true);
        }
        else
        {
            animator.SetBool("InBubble", false);
        }
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
                _background.gameObject.SetActive(false);
                break;
            case GameState.OnGoing:
                int playerLayer = LayerMask.NameToLayer("P" + (playerId + 1));
                _camera.GetComponent<UniversalAdditionalCameraData>().volumeLayerMask |= 1 << playerLayer;
                ChangeLayerRecursive(_background, playerLayer);
                movement.gameObject.SetActive(true);
                _camera.gameObject.SetActive(true);
                _background.gameObject.SetActive(true);
                _camera.cullingMask |= 1 << playerLayer;
                
                float playerSpriteHueOffset = playerId < _playerSpriteHueOffsets.Length ? _playerSpriteHueOffsets[playerId] : 0;
                _playerSprite.material.SetFloat(_OFFSET, playerSpriteHueOffset);
                break;
            case GameState.Ended:
                movement.gameObject.SetActive(false);
                break;
        }
    }

    void ChangeLayerRecursive(Transform parent, int layer)
    {
        parent.gameObject.layer = layer;
        foreach (Transform t in parent)
        {
            ChangeLayerRecursive(t, layer);
        }
    }

    private void HandleDeathEvent()
    {
        FindFirstObjectByType<VisualEffectController>().ActivateDeadState(playerId);
        //Change sprite to dead sprite
        animator.SetTrigger("Dead");
        Debug.Log("Player " + playerId + " is dead");
    }

    private void ChangeToGhost()
    {

    }

    public IEnumerator BubblePopped(int playerId)
    {
        _camera.GetComponent<UniversalAdditionalCameraData>().SetRenderer(2);
        yield return new WaitForSeconds(2f);
        _camera.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
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
