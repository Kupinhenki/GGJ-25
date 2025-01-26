using UnityEngine;

public class Shoot : MonoBehaviour
{
    static readonly int _OFFSET = Shader.PropertyToID("_Offset");
    static readonly int _SHOOT = Animator.StringToHash("Shoot");
    [SerializeField] public GameObject bubblePrefab;
    [SerializeField] public Transform bubbleSpawnPoint;
    [SerializeField] private Movement movement;
    [SerializeField] private float minimumShootCooldown = 0.5f;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem shootParticles;
    private GameObject[] bubbleArray = new GameObject[3];
    public void ShootBubble(float hueOffset)
    {
        if (!movement.isGhostMode && movement.isActiveAndEnabled && canShoot)
        {
            for (int i = 0; i < bubbleArray.Length; i++)
            {
                if (bubbleArray[i] == null)
                {
                    canShoot = false;
                    animator.SetTrigger(_SHOOT);
                    shootParticles.Play();
                    GameObject bubble = Instantiate(bubblePrefab, new Vector2(bubbleSpawnPoint.position.x, bubbleSpawnPoint.position.y), bubbleSpawnPoint.rotation);
                    bubble.GetComponent<SpriteRenderer>()?.material.SetFloat(_OFFSET, hueOffset);
                    bubbleArray[i] = bubble;
                    Vector2 direction = movement.IsFacingRight ? Vector2.right : Vector2.left;
                    Bubble bubbleScript = bubble.GetComponent<Bubble>();
                    bubbleScript.ShootBubble(direction);

                    AudioManager.Instance.PlaySoundFromAnimationEvent("Shoot");
                    Invoke(nameof(SetCanShootTrue), minimumShootCooldown);
                    return;
                }
            }
        }
    }

    private void SetCanShootTrue()
    {
        canShoot = true;
    }
}
