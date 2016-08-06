using UnityEngine;
using System.Collections;

public class FreeCam : MonoBehaviour {

    Camera m_camera;
    Transform m_cameraTransform;

    public float CameraMovementSpeed = 1;

	// Use this for initialization
	void Start () {
        m_camera = GetComponent<Camera>();
        if (m_camera == null)
        {
            throw new System.InvalidOperationException(GetType().FullName + " needs to be attached to a camera");
        }

        m_cameraTransform = GetComponent<Transform>();

    }
	
	// Update is called once per frame
	void Update () {

        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        if (x != 0 || y != 0)
        {
            m_cameraTransform.position = new Vector3(m_cameraTransform.position.x + x * CameraMovementSpeed, m_cameraTransform.position.y + y * CameraMovementSpeed, m_cameraTransform.position.z);
        }
        //
	}
}
