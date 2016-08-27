using UnityEngine;
using System.Collections;

namespace Physics
{
    public class BaloonPop : MonoBehaviour
    {
        public BaloonPop()
        {
            ExplodingBaloonFadeTime = 0.25f;
        }

        public GameObject baloonExplosionPrefab;
        public float force = 1;
        public float radius = 1;

        public float ExplodingBaloonFadeTime;
        //private bool m_baloonIsPopped = false;

        void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.tag == "baloonPopping")
            {
                //if (baloonExplosionPrefab != null)
                //{

                GameObject newExplosion = Instantiate(baloonExplosionPrefab);
                newExplosion.transform.position = transform.position;
                //ExplodingBaloon explosionScript = newExplosion.GetComponent<ExplodingBaloon>();
                //explosionScript.force = force;
                //explosionScript.radius = radius;

                GameObject fadingBaloon = Instantiate<GameObject>(gameObject);
                fadingBaloon.transform.SetParent(gameObject.transform.parent);
                fadingBaloon.transform.position = gameObject.transform.position;

                Collider2D collider = fadingBaloon.GetComponent<Collider2D>();
                collider.enabled = false;

                Rigidbody2D rigidBody = fadingBaloon.GetComponent<Rigidbody2D>();
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

                BaloonFading fading = fadingBaloon.AddComponent<BaloonFading>();
                fading.FadeTime = ExplodingBaloonFadeTime;

                Destroy(gameObject);
                //disable this behavior.
                enabled = false;
                    //if (explosionScript != null)
                    //{
                    //    explosionScript.force = force;
                    //    explosionScript.radius = radius;

                    //}
                    //else
                    //{
                    //    Debug.LogWarning(typeof(ExplodingBaloon).FullName + " not found on BaloonExplosionPrefab");
                    //}
                //}
                //else
                //{
                //    Debug.LogWarning("BaloonExplosionPrefab needs to be set for " + GetType().FullName + " Script.");
                //}
            }
            
        }
    }
}