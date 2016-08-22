using UnityEngine;
using System.Collections;

namespace Gui
{

    public class DropableObject : MonoBehaviour
    {

        private Collider2D m_collider;
        private Rigidbody2D m_rigidBody;

        // Use this for initialization
        void Start()
        {

            m_collider.isTrigger = true;

        }

        void OnTriggerEnter(Collider other)
        {
            m_collider.isTrigger = false;
            m_rigidBody.isKinematic = true;
        }

        void Awake()
        {
            m_collider = GetComponent<Collider2D>();
            m_rigidBody = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}