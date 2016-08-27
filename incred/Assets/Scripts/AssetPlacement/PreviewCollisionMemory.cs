using UnityEngine;
using System.Collections.Generic;

public class PreviewCollisionMemory : MonoBehaviour {

    HashSet<Collider2D> m_currentColliders
        = new HashSet<Collider2D>();

    private bool m_isOriginalColorTracked;
    private Color m_originalColor;

    void MarkAsUnplaceable()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (!m_isOriginalColorTracked)
        {
            m_originalColor = renderer.material.color;
            m_isOriginalColorTracked = true;
        }

        renderer.material.color = new Color(0.75f, 0.15f, 0.15f, 0.8f);
    }

    void MarkAsNormal()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = m_originalColor;
    }


    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //Debug.Log(otherCollider);
        m_currentColliders.Add(otherCollider);
        MarkAsUnplaceable();
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        m_currentColliders.Remove(otherCollider);
        if (m_currentColliders.Count == 0)
        {
            MarkAsNormal();
        }
    }

    public bool IsColliding
    {
        get
        {
            return m_currentColliders.Count > 0;
        }

    }

}
