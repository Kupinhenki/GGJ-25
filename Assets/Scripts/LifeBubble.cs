using UnityEngine;

public class LifeBubble : MonoBehaviour
{
    public PlayerController owner;
    [SerializeField] SpriteRenderer _spriteRenderer;
    public SpriteRenderer spriteRenderer => _spriteRenderer;

    public float currentAngle;
    public BubblePointHandler pointHandler;

    /// <summary>
    /// Lose life for the owning player when this collider is hit and it wasn't the owner.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Movement refe = collision.gameObject.GetComponent<Movement>();

        if (refe != owner.movement && refe != null)
        {
            GameManager.Instance.LoseLife(owner);
            pointHandler.orbsList.Remove(gameObject);
            AudioManager.Instance.PlaySoundFromAnimationEvent("Score");
            Destroy(gameObject);
        }
    }
}
