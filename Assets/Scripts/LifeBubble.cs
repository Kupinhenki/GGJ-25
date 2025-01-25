using UnityEngine;

public class LifeBubble : MonoBehaviour
{
    public PlayerController owner;
    private GameManager manager;

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
        if(collision.gameObject.GetComponent<PlayerController>() != owner)
        {
            manager.LoseLife(owner);
        }

        Destroy(gameObject);
    }
}
