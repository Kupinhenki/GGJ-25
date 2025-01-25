using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Transform bubbleSpawnPoint;
    public float shootCooldown = 1f;
    private bool right = false;
    private bool canShoot = true;

    void Update()
    {
        if (canShoot)
        {
            ShootBubble();
        }
    }

    void ShootBubble()
    {
        canShoot = false;
        GameObject bubble = Instantiate(bubblePrefab, bubbleSpawnPoint.position, bubbleSpawnPoint.rotation);

        Vector2 direction;
        if (right)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }
        right = !right;

        Bubble bubbleScript = bubble.GetComponent<Bubble>();
        bubbleScript.ShootBubble(direction);

        Invoke(nameof(ResetShoot), shootCooldown);
    }

    void ResetShoot()
    {
        canShoot = true;
    }
}
