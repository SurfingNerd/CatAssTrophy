using UnityEngine;
using System.Collections;

public class BreakVase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        

        if (coll.gameObject.tag == "potflower")
        {
            Debug.Log("Collision with flower!");
        }
    }

    // Update is called once per frame
    void Update ()
    {
        
    }
}
