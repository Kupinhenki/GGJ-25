using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform _camera;
    public float parallaxAmountX, parallaxAmountY;
    private float startPosX, startPosY;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
    }
    void LateUpdate()
    {
        float distanceX = (_camera.transform.position.x * parallaxAmountX);
        float distanceY = (_camera.transform.position.y * parallaxAmountY);

        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);
    }
}
