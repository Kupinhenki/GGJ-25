using System.Collections.Generic;
using UnityEngine;

public class PushingForceScript : MonoBehaviour
{
    [SerializeField] private float PushStrength;
    [SerializeField] private float SuckStrength;

    private float DistanceAlongAxis(Transform a, Transform b, Vector3 DifferenceDirection)
    {
        return Vector3.Dot(DifferenceDirection, a.position - b.position);
    }

    private void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.gameObject.CompareTag("Bubble"))
        {
            float PushStrengthMultiplier = 1 - (DistanceAlongAxis(Other.transform, transform, transform.up) / GetComponent<BoxCollider2D>().size.y);
            PushStrengthMultiplier = PushStrengthMultiplier > 0 ? PushStrengthMultiplier : 0;

            float SuckStrengthMultiplier = (DistanceAlongAxis(Other.transform, transform, transform.right) / GetComponent<BoxCollider2D>().size.x) * -1;

            Vector3 OutwardsPush = transform.up * PushStrength * PushStrengthMultiplier;
            Vector3 InwardsSuction = transform.right * SuckStrength * SuckStrengthMultiplier;
            Other.transform.GetComponent<Rigidbody2D>().AddForce(OutwardsPush + InwardsSuction);
        }
    }
}
