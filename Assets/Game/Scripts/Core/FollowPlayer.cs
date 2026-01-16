using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    void LateUpdate()
    {
        transform.position = player.transform.position + new Vector3(5, 7, -2.5f);
        //transform.rotation = player.transform.rotation;
    }
}
