using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

    public GameObject cat;
    public Camera cam;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var pos = cat.transform.position;
        pos.z = cam.transform.position.z;
        cam.transform.position = pos;
	}
}
