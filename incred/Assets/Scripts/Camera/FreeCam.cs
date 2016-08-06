using UnityEngine;
using System.Collections;

public class FreeCam : MonoBehaviour {

    Camera m_camera;
    Transform m_player;
    Vector3 m_offset;

    public float CameraMovementSpeed = 1;

	// Use this for initialization
	void Start () {
        m_camera = GetComponent<Camera>();
        if (m_camera == null)
        {
            throw new System.InvalidOperationException(GetType().FullName + " needs to be attached to a camera");
        }

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_offset = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        bool focusBall = Input.GetKey(KeyCode.Space);

        if (focusBall)
        {
            transform.position = new Vector3(m_player.position.x + m_offset.x, m_player.position.y + m_offset.y, m_offset.z); 
            // Camera follows the player with specified offset position
        }
        else if (x != 0 || y != 0)
        {
            transform.position = new Vector3(transform.position.x + x * CameraMovementSpeed, transform.position.y + y * CameraMovementSpeed, transform.position.z);
        }
        //
	}
}
