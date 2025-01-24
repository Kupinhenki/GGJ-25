using System.Collections.Generic;
using UnityEngine;

public class PushingForceScript : MonoBehaviour
{
    private List<GameObject> AffectedObjects;
    [SerializeField] private float PushStrength;

    // Itsellä huono tunne että OnTriggerStay2D ei ole tarpeeksi hyvä, mutten pysty nyt testaamaan.
    // Niin jätän tänne väliaikaisesti koodit joilla saatan saada toimimaan myöhemmin.
    // Poistan turhat myöhemmin, ei huolta.

    /*
    void FixedUpdate()
    {
        if (AffectedObjects.Count > 0)
        {
            foreach (GameObject obj in AffectedObjects)
            {
                float PushStrengthMultiplier = 1 - (DistanceAlongAxis(obj.transform, transform) / GetComponent<BoxCollider2D>().size.y);
                // obj.PhysicsPush(transform.up * PushStrength * PushStrengthMultiplier);
            }
        }
    }
    */

    private float DistanceAlongAxis(Transform a, Transform b)
    {
        Vector3 differenceDirection = b.up;

        return Vector3.Dot(differenceDirection, a.position - b.position);
    }

    /*
    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Bubble"))
        {
            AffectedObjects.Add(Other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.CompareTag("Bubble"))
        {
            AffectedObjects.Remove(Other.gameObject);
        }
    }
    */

    private void OnTriggerStay2D(Collider2D Other)
    {
        float PushStrengthMultiplier = 1 - (DistanceAlongAxis(Other.transform, transform) / GetComponent<BoxCollider2D>().size.y);
        PushStrengthMultiplier = PushStrengthMultiplier > 0 ? PushStrengthMultiplier : 0;
        // Other.PhysicsPush(transform.up * PushStrength * PushStrengthMultiplier);
    }
}
