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

        if (coll.gameObject.tag == "potflower" || coll.gameObject.tag == "Player" || coll.gameObject.tag == "wakeobject")
        {
            //Debug.Log("Gameobj length " + GameObject.FindGameObjectsWithTag("breakablePot").Length);


            //GameObject breakPot = GameObject.FindGameObjectsWithTag("breakablePot")[0];
            //Instantiate(breakPot);
            //Vector3 oldPosition = transform.position;
            //Quaternion oldRotation = transform.rotation;
            //Object breakPot = Instantiate(BrokenVaseAlternate, oldPosition, oldRotation);
            GameObject breakPot = Instantiate(BrokenVaseAlternate);
            //breakPot.transform.position = transform.position;

            Vector3 sub = new Vector3(0, -2, 0);
            Vector3 newPos = transform.position + sub;
            breakPot.transform.position = newPos;

            //breakPot.transform.position = startposition;

            Debug.Log("OldPOS: "+ transform.position + " NewPOS"+ breakPot.transform.position);
            

            //breakPot.transform.position = new Vector3((float)30.02227, (float) -20.9, 0);

            Destroy(gameObject);


        }
    }

    // Update is called once per frame
    void Update ()
    {
        
    }
}
