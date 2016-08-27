using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PreviewCollisionMemory : MonoBehaviour {

    HashSet<Collider2D> m_currentColliders
        = new HashSet<Collider2D>();

    private PreviewCollisionMemory[] ChildCollisionDetectors;

    private bool m_isOriginalColorTracked;
    private Color m_originalColor;

    public HashSet<GameObject> ObjectsToIgnore;

    void Start()
    {
        ChildCollisionDetectors = gameObject.GetComponentsInChildren<PreviewCollisionMemory>();
    }

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
        if (!ObjectsToIgnore.Contains(otherCollider.gameObject))
        {
            //Debug.Log(otherCollider);
            m_currentColliders.Add(otherCollider);
            Debug.Log(gameObject.name + " - " + otherCollider.gameObject.name + " Ignored: " + ObjectsToIgnore.Count);
            MarkAsUnplaceable();
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        if ( m_currentColliders.Contains(otherCollider))
        {
            m_currentColliders.Remove(otherCollider);
        }
        
        if (!IsColliding)
        {
            MarkAsNormal();
        }
    }

    public bool IsColliding
    {
        get
        {
            return m_currentColliders.Count > 0 || (ChildCollisionDetectors != null && ChildCollisionDetectors.Any(x=>x.IsColliding));
        }
    }

}
