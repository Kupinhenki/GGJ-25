using UnityEngine;

public class LavaDamageScript : MonoBehaviour
{
    [SerializeField] private int DamageAmount = 1;

    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Player"))
        {
            // Other.TakeDamage(DamageAmount);
        }
    }

    void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.CompareTag("Player"))
        {
            // Other.TakeDamage(DamageAmount);
        }
    }
}
