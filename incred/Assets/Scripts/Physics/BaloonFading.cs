using UnityEngine;
using System.Collections;

namespace Physics
{
    public class BaloonFading : MonoBehaviour
    {
        private Renderer m_renderer;

        public float FadeTime;

        private float m_fadeTimeLeft;
        private Vector3 m_initialScale;
        void Start()
        {
            m_renderer = gameObject.GetComponent<Renderer>();
            m_fadeTimeLeft = FadeTime;
            m_initialScale = gameObject.transform.localScale;
        }

        void Update()
        {
            m_fadeTimeLeft = m_fadeTimeLeft - Time.deltaTime;
            if (m_fadeTimeLeft < 0)
            {
                m_fadeTimeLeft = 0;
            }

            float alpha = m_fadeTimeLeft / FadeTime;
            m_renderer.material.color = new Color(m_renderer.material.color.r, m_renderer.material.color.g, m_renderer.material.color.b, alpha);

            float scaleMultiplier = 2 - alpha;
            gameObject.transform.localScale = m_initialScale * scaleMultiplier;

            if (m_fadeTimeLeft == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
