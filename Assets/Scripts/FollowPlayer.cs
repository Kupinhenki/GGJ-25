using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        //Follow the player, not z axis
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);     
    }
}

    

