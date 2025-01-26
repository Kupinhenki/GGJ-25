using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using static UnityEngine.Analytics.IAnalytic;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float initialForce = 30f;
    [SerializeField] private float decelerationForce = 35f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float oscillationScale = 0.001f;
    [SerializeField] private float oscillationSpeed = 10;
    [SerializeField] private float bounceForce = 40f;
    
    private float maxScale = 1f;
    private float growthDuration = 0.3f;
    [SerializeField] private GameObject trappedPlayer;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
        StartCoroutine(GrowBubble());
    }

    private void LateUpdate()
    {
        if (trappedPlayer != null)
        {
            trappedPlayer.transform.position = this.transform.position;
        }
    }
    private void FixedUpdate()
    {
        DecelerateOnXAxis();
    }
    private IEnumerator GrowBubble()
    {
        float timer = 0f;

        Vector2 initialScale = new Vector2(0.4f, 0.4f);
        Vector2 targetScale = Vector2.one * maxScale;

        while (timer < growthDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / growthDuration;
            transform.localScale = Vector2.Lerp(Vector2.zero, targetScale, progress);
            yield return null;
        }

        transform.localScale = targetScale;
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

    // Player gets hit by a bubble
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
                // TODO: Check if player is already in bubble or somehow fix with colliders
                TrapPlayer(other.gameObject);
            }
        }
    }

    // Player jump on a bubble
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(trappedPlayer != null && collision.gameObject == trappedPlayer)
            {
                return;
            }
            AudioManager.Instance.PlaySoundFromAnimationEvent("BubbleBounce");
            if (collision.gameObject.TryGetComponent<Movement>(out var movement))
            {
                float tempForce = bounceForce;
                if (movement.RB.linearVelocity.y < 0)
                    tempForce -= movement.RB.linearVelocity.y;
                movement.RB.gravityScale = 10;
                movement.RB.AddForce(Vector2.up * tempForce, ForceMode2D.Impulse);
                movement.bouncing = true;
            }
            Destroy(this.gameObject);
        }
    }

    // Disable controls for trapped player
    void TrapPlayer(GameObject player)
    {
        trappedPlayer = player;
        Vector3 originalScale = player.transform.localScale;
        player.transform.localScale = new Vector3(0.5f * Mathf.Sign(originalScale.x), 0.5f * Mathf.Sign(originalScale.y), originalScale.z);

        if (trappedPlayer.TryGetComponent<Movement>(out var movement))
        {
            movement.enabled = false;
            movement.RB.gravityScale = 0;
            movement.RB.linearVelocity = Vector2.zero;
        }
        PlayerController playerController = trappedPlayer.GetComponentInParent<PlayerController>();
        playerController.playerInBubble = true;
        trappedPlayer.transform.position = this.transform.position;
    }

    // Enable controls for trapped player
    void OnDestroy()
    {
        if (trappedPlayer != null)
        {
            Vector3 originalScale = trappedPlayer.transform.localScale;
            trappedPlayer.transform.localScale = new Vector3(1.0f * Mathf.Sign(originalScale.x), 1.0f * Mathf.Sign(originalScale.y), originalScale.z);

            if (trappedPlayer.TryGetComponent<Movement>(out var movement))
            {
                movement.enabled = true;
                movement.RB.gravityScale = 1;
                movement.RB.linearVelocity = Vector2.zero;
            }
            PlayerController playerController = trappedPlayer.GetComponentInParent<PlayerController>();
            playerController.playerInBubble = false;
            trappedPlayer = null;
        }

    }
}
