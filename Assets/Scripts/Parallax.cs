using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float startPos;
    public GameObject player;
    public float parallaxAmount;

    void Start()
    {
        startPos = transform.position.x;
        
    }
    void FixedUpdate()
    {
        float distance = (player.transform.position.x * parallaxAmount);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

    }
}
