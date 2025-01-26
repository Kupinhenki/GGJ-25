using UnityEngine;

public class ContactDamageScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.CompareTag("Player"))
        {
            // Pelaaja menee takaisin tason alkuun?
        }
    }

    private void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.gameObject.CompareTag("Player"))
        {
            // Jotain tämmöstä
        }
    }
}
