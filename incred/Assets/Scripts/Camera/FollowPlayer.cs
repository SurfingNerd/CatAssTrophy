using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public Vector3 offset;

    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); // Camera follows the player with specified offset position
    }

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        offset = transform.position;

	}
	
	
}
