using UnityEngine;
using System.Collections;


public class WakeUp : MonoBehaviour {

    public GameObject wokenOwner;

    // Use this for initialization
    void Start () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {

        //Debug.Log("Collision");

        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "wakeobject")
        {
            GameObject waki = Instantiate(wokenOwner);
            //Vector3 sub = new Vector3(0, -2, 0);
            //Vector3 newPos = transform.position + sub;
            waki.transform.position = transform.position;
            Destroy(gameObject);
            //breakPot.transform.position = startposition;

            //Debug.Log("OldPOS: " + transform.position + " NewPOS" + breakPot.transform.position);

        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
