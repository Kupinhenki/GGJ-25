using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] public GameObject bubblePrefab;
    [SerializeField] public Transform bubbleSpawnPoint;
    [SerializeField] private Movement movement;
    [SerializeField] private GameObject bubble;
    [SerializeField] private float minimumShootCooldown = 1f;
    [SerializeField] private bool canShoot = true;
    public void ShootBubble()
    {
        if (!movement.isGhostMode && bubble == null && movement.isActiveAndEnabled)
        {
            canShoot = false;
            bubble = Instantiate(bubblePrefab, new Vector2(bubbleSpawnPoint.position.x, bubbleSpawnPoint.position.y), bubbleSpawnPoint.rotation);
            Vector2 direction = movement.IsFacingRight ? Vector2.right : Vector2.left;
            Bubble bubbleScript = bubble.GetComponent<Bubble>();
            bubbleScript.ShootBubble(direction);
            AudioManager.Instance.PlaySoundFromAnimationEvent("Shoot");
            Invoke("SetCanShootTrue", minimumShootCooldown);
        }
    }

    private void SetCanShootTrue()
    {
        canShoot = true;
    }
}
