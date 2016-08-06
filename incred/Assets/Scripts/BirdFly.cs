using UnityEngine;
using System.Collections;

public class BirdFly : MonoBehaviour {

    public int speed = 1;
    private bool fly = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (fly)
        {
            GetComponent<Rigidbody2D>().AddForce(-transform.right * 2, ForceMode2D.Force);
        }
        if (fly && Input.GetKeyDown("h"))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            fly = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }
}
