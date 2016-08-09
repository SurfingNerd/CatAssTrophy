using UnityEngine;
using System.Collections;

public class BaloonPop : MonoBehaviour {

    public GameObject baloonExplosionPrefab;
    public float force = 1;
    public float radius = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "baloonPopping")
        {
            if (baloonExplosionPrefab != null)
            {
                
                GameObject newExplosion = Instantiate(baloonExplosionPrefab);
                newExplosion.transform.position = transform.position;
                //ExplodingBaloon explosionScript = newExplosion.GetComponent<ExplodingBaloon>();
                Destroy(gameObject);

                //if (explosionScript != null)
                //{
                //    explosionScript.force = force;
                //    explosionScript.radius = radius;
                    
                //}
                //else
                //{
                //    Debug.LogWarning(typeof(ExplodingBaloon).FullName + " not found on BaloonExplosionPrefab");
                //}
            }
            else
            {
                Debug.LogWarning("BaloonExplosionPrefab needs to be set for " + GetType().FullName + " Script.");
            }
        }
    }
}
