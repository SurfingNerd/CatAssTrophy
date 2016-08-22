using UnityEngine;
using System.Collections;

namespace Physics
{

    public class DestroyMe : MonoBehaviour
    {
        public float afterseconds = 1;
        private bool m_isDestroyed;

        // Update is called once per frame
        void Update()
        {
            if (!m_isDestroyed)
            {
                Destroy(gameObject, afterseconds);
                m_isDestroyed = true;
            }
        }
    }
}