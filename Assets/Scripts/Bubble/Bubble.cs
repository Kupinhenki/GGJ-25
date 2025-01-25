using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float initialForce = 25f;
    [SerializeField] private float decelerationForce = 12f;
    [SerializeField] private float lifetime = 5f;

    [SerializeField] private float timer;
    [SerializeField] private GameObject trappedPlayer;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        timer = 0;
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        rb.transform.Translate(new Vector2(0, 1f * Oscillate(timer, 10, 0.01f)));
        DecelerateOnXAxis();
        if (trappedPlayer != null)
        {
            trappedPlayer.transform.position = this.transform.position;
        }
    }

    float Oscillate(float time, float speed, float scale)
    {
        return Mathf.Cos(time * speed / Mathf.PI) * scale;
    }

    public void DecelerateOnXAxis()
    {
        Vector2 decelerationDirection = new(-rb.linearVelocity.normalized.x, 0);
        rb.AddForce(decelerationDirection * decelerationForce, ForceMode2D.Force);
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
    }
    
    // Call from playerController?
    public void ShootBubble(Vector2 direction) 
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (rb != null)
        {
            rb.AddForce(direction * initialForce, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (trappedPlayer != null && other.gameObject != trappedPlayer)
            {
                Destroy(gameObject);
            }
            else
            {
                TrapPlayer(other.gameObject);
            }
        }
        else if (other.CompareTag("Obstacle") || other.CompareTag("Bubble"))
        {
            Destroy(gameObject);
        }
    }

    // Disable controls for trapped player
    void TrapPlayer(GameObject player)
    {
        trappedPlayer = player;
        player.transform.localScale = new Vector2(0.5f, 0.5f);
        if (trappedPlayer.TryGetComponent<Movement>(out var movement))
        {
            movement.enabled = false;
            movement.RB.gravityScale = 0;
            movement.RB.linearVelocity = Vector2.zero;
        }
        trappedPlayer.transform.position = this.transform.position;
    }

    // Enable controls for trapped player
    void OnDestroy()
    {
        if (trappedPlayer != null)
        {
            trappedPlayer.transform.localScale = new Vector2(1.0f, 1.0f);
            if (trappedPlayer.TryGetComponent<Movement>(out var movement))
            {
                movement.enabled = true;
                movement.RB.gravityScale = 1;
                movement.RB.linearVelocity = Vector2.zero;
            }
            trappedPlayer = null;
        }
    }
}
