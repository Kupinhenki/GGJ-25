using UnityEngine;

public class EelPlatform : MonoBehaviour
{
    private int State = 0; // 0 = idle, 1 = extending, 2 = retracting
    private float ExtensionMultiplier = 0.0f;
    private float StartXPos = 0.0f;
    private float Length = 0.0f;
    [SerializeField] private float Timer = 0.0f;
    [SerializeField] private float TimeBetween = 0.0f;
    [SerializeField] private BoxCollider2D BoxCollider;

    private void Awake()
    {
        StartXPos = transform.localPosition.x;
        Length = transform.localScale.x;
    }


    void FixedUpdate()
    {
        if (State == 1)
        {
            if (ExtensionMultiplier >= 1.0f)
            {
                ExtensionMultiplier = 1.0f;
                State = 0;
            } else
            {
                ExtensionMultiplier += 0.1f;
            }
        } else if (State == 2)
        {
            if (ExtensionMultiplier <= 0.0f)
            {
                ExtensionMultiplier = 0.0f;
                State = 0;
            }
            else
            {
                ExtensionMultiplier -= 0.025f;
            }
        } else if (State == 0)
        {
            Timer += Time.fixedDeltaTime;
            if (Timer > TimeBetween)
            {
                Timer = 0.0f;
                State = ExtensionMultiplier > 0.5f ? 2 : 1;
            }
        }

        transform.localPosition = new Vector3(StartXPos + ((Length * 0.9f) * ExtensionMultiplier), 0, 0); // -1.2 -> 1.5

        BoxCollider.size = new Vector2(0.0f + (0.9f * ExtensionMultiplier), 1.0f);

        BoxCollider.offset = new Vector2(0.35f + (-0.40f * ExtensionMultiplier), 0.0f);
    }
}
