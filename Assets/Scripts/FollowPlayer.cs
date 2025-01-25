using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Movement player;
    private Vector3 targetPoint = Vector3.zero;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float lookAheadDist = 8f;
    [SerializeField] private float lookAheadSpeed = 3f;
    [SerializeField] private float lookOffset;
    private bool isFalling;
    [SerializeField] private float maxVerticalOffset = 5f;

    private float stationaryTime = 0f; // Tracks how long the player has been stationary
    [SerializeField] private float stationaryDelay = 1f; // Time in seconds before resetting the camera

    void Start()
    {
        //Follow the player, not z axis
        targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);     
    }

    private void LateUpdate()
    {
         //If player is on the ground, follow them on the y axis
        if (player.LastOnGroundTime > 0.1f)
        {
            targetPoint.y = player.transform.position.y;
        }
        
        //targetPoint.y = player.transform.position.y;
        if (transform.position.y - player.transform.position.y > maxVerticalOffset)
        {
            isFalling = true;
        }

        if (isFalling)
        {
            targetPoint.y = player.transform.position.y;

            if (player.LastOnGroundTime > 0.1f)
            {
                isFalling = false;
            }
        }

        // Adjust lookOffset based on player movement
        if (player.RB.linearVelocity.x > 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, lookAheadDist, lookAheadSpeed * Time.deltaTime);
            stationaryTime = 0f; // Reset stationary timer
        }
        else if (player.RB.linearVelocity.x < 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, -lookAheadDist, lookAheadSpeed * Time.deltaTime);
            stationaryTime = 0f; // Reset stationary timer
        }
        else
        {
            stationaryTime += Time.deltaTime; // Increment stationary time

            // If stationary for longer than the delay, reset the lookOffset
            if (stationaryTime >= stationaryDelay)
            {
                lookOffset = Mathf.Lerp(lookOffset, 0f, lookAheadSpeed * Time.deltaTime);
            }
        }


        targetPoint.x = player.transform.position.x + lookOffset;
       
        transform.position = Vector3.Lerp(transform.position, targetPoint, followSpeed * Time.deltaTime);
    }
}

    

