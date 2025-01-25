using UnityEngine;

public class ContactDamageScript : MonoBehaviour
{
    [SerializeField] private int DamageAmount = 1;

    private void OnCollisionEnter2D(Collision2D Other)
    {
        if (Other.gameObject.CompareTag("Player"))
        {
            // Other.TakeDamage(DamageAmount);
        }
    }

    private void OnCollisionStay2D(Collision2D Other)
    {
        if (Other.gameObject.CompareTag("Player"))
        {
            // Other.TakeDamage(DamageAmount);
        }
    }
}
