using UnityEngine;
using System.Collections;

namespace Physics
{

    public class ExplodingBaloon : MonoBehaviour
    {

        public float force;
        public float radius;


        bool forceIsApplied = false;

        System.Collections.Generic.HashSet<Rigidbody2D> m_bodies = new System.Collections.Generic.HashSet<Rigidbody2D>();


        void Awake()
        {
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            collider.radius = radius;

        }

        // Update is called once per frame
        void Update()
        {

            if (!forceIsApplied)
            {
                Debug.Log("Applying forces.");
                foreach (var item in m_bodies)
                {
                    Debug.Log("BOOM " + item);
                    //vector from the center of this explosion to center of body
                    //Vector2 start = new Vector2(this.transform.position.x, transform.position.y);
                    Vector2 dir = new Vector2(item.centerOfMass.x - this.transform.position.x, item.centerOfMass.y - this.transform.position.y);
                    //item.centerOfMass
                    AddExplosionForce(item, 2, dir, radius);
                }
            }

            //Destroy(this);
        }

        public void AddExplosionForce(Rigidbody2D body, float expForce, Vector2 expPosition, float expRadius)
        {
            var dir = (body.centerOfMass - expPosition);
            float calc = 1 - (dir.magnitude / expRadius);
            if (calc <= 0)
            {
                calc = 0;
            }

            body.AddForce(dir.normalized * expForce * calc);
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            Rigidbody2D body = other.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                if (!m_bodies.Contains(body))
                {
                    m_bodies.Add(body);
                }
            }
        }
    }
}