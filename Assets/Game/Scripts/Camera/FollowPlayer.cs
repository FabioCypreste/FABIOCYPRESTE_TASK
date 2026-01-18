using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    private void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
    }
    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }
        transform.position = player.transform.position + offset;
    }
}
