using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {

    AudioClip sound;

    private AudioSource audio;
    // Use this for initialization
    void Start () {
        
        audio = GetComponent<AudioSource>();
        audio.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audio.PlayOneShot(sound);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            audio.PlayOneShot(sound);
        }
    }
}
