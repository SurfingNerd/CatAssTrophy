using UnityEngine;
using System.Collections;

public class pillow_squach : MonoBehaviour {

    public GameObject top;
    public GameObject bottom;

    private float initDistanceInverse;
    private float currDistance;
    private float k;
    private Vector3 initScale;
    private Vector3 currScale;

    // Use this for initialization
    void Start () {
        initDistanceInverse = 1.0f / (top.transform.position - bottom.transform.position).magnitude;
        initScale = this.transform.localScale;
        currScale = initScale;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = 0.5f * (top.transform.position + bottom.transform.position);
        currDistance = (top.transform.position - bottom.transform.position).magnitude;
        k = currDistance * initDistanceInverse;
        currScale.y = initScale.y * k; 
        this.transform.localScale = currScale;
    }
}
