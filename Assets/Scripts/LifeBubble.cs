using UnityEngine;

public class LifeBubble : MonoBehaviour
{
    public PlayerController owner;
    private GameManager manager;

    public float currentAngle;
    public BubblePointHandler pointHandler;

    private void Start()
    {
        manager = GameManager.Instance;
    }

    /// <summary>
    /// Lose life for the owning player when this collider is hit and it wasn't the owner.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Movement refe = collision.gameObject.GetComponent<Movement>();

        if (refe != owner.movement && refe != null)
        {
            manager.LoseLife(owner);
            pointHandler.orbsList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
