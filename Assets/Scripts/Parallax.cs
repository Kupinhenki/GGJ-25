using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float startPosX, startPosY;
    public GameObject player;
    public float parallaxAmountX, parallaxAmountY;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        
    }
    void FixedUpdate()
    {
        float distanceX = (player.transform.position.x * parallaxAmountX);
        float distanceY = (player.transform.position.y * parallaxAmountY);

        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

    }
}
