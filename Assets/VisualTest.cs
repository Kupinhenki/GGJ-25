using UnityEngine;

public class VisualTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindFirstObjectByType<VisualEffectController>().ActivateDeadState(0);
    }

}
