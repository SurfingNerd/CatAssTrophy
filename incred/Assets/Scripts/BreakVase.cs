using UnityEngine;
using System.Collections;

public class BreakVase : MonoBehaviour {


    public GameObject BrokenVasePrefab;
    public GameObject BrokenVaseAlternate;

    // Use this for initialization
    void Start () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {

        Debug.Log("Collision");

        if (coll.gameObject.tag == "potflower")
        {
            Debug.Log("Gameobj length " + GameObject.FindGameObjectsWithTag("breakablePot").Length);
            Destroy(gameObject);

            //GameObject breakPot = GameObject.FindGameObjectsWithTag("breakablePot")[0];
            //Instantiate(breakPot);
            GameObject breakPot = Instantiate(BrokenVaseAlternate);
            breakPot.transform.position = transform.position;
            
        }
    }

    // Update is called once per frame
    void Update ()
    {
        
    }
}
