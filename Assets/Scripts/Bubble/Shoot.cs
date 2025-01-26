using UnityEngine;

public class Shoot : MonoBehaviour
{
    static readonly int _OFFSET = Shader.PropertyToID("_Offset");
    static readonly int _SHOOT = Animator.StringToHash("Shoot");
    [SerializeField] public GameObject bubblePrefab;
    [SerializeField] public Transform bubbleSpawnPoint;
    [SerializeField] private Movement movement;
    [SerializeField] private GameObject bubble;
    [SerializeField] private float minimumShootCooldown = 1f;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private Animator animator;
    public void ShootBubble(float hueOffset)
    {
        if (!movement.isGhostMode && bubble == null && movement.isActiveAndEnabled)
        {
            canShoot = false;
            animator.SetTrigger(_SHOOT);
            bubble = Instantiate(bubblePrefab, new Vector2(bubbleSpawnPoint.position.x, bubbleSpawnPoint.position.y), bubbleSpawnPoint.rotation);
            bubble.GetComponent<SpriteRenderer>()?.material.SetFloat(_OFFSET, hueOffset);
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
