using UnityEngine;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlacementService : MonoBehaviour
{


    public GameObject Prefab;

	// Use this for initialization
	void Start () {
	    
        
	}

    GameObject currentPreviewObject;
	
	// Update is called once per frame
	void Update () {

        if (Prefab != null)
        {
            
            Vector3? vector = GeometryHelper.GetPositionFromMouse();
            if ( vector.HasValue)
            {
                //create new if not exist
                if (currentPreviewObject == null)
                {
                    currentPreviewObject = Instantiate(Prefab) as GameObject;
                    MakePreviewObject(currentPreviewObject);
                }


                currentPreviewObject.transform.position = vector.Value;
                
                //DeactivateAllColliders(currentPreviewObject);
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject obj = Instantiate(Prefab) as GameObject;
                obj.transform.position = vector.Value;
            }
        }
	}

    private void MakePreviewObject(GameObject currentPreviewObject)
    {
        DeactivateAllColliders(currentPreviewObject);
        DeactivateAllRigidBodies(currentPreviewObject);
        DeactivateAllJoints(currentPreviewObject);
    }

    private void DeactivateAllRigidBodies(GameObject currentPreviewObject)
    {
        foreach(var rig in GetAll<Rigidbody2D>(currentPreviewObject))
        {
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void DeactivateAllJoints(GameObject currentPreviewObject)
    {
        foreach (var item in GetAll<Joint2D>(currentPreviewObject))
        {
            item.enabled = false;
        }
        
        
        //Rigidbody2D rigBody = currentPreviewObject.GetComponent<Rigidbody2D>();
        
        //rigBody.enabled = false;
        

    }

    private void DeactivateAllColliders(GameObject obj)
    {
        
        foreach (var collider in GetAll<Collider2D>(obj))
        {
            collider.enabled = false;
        }
    }

    //private System.Collections.Generic.IEnumerable<T> GetAll<T>(GameObject obj)
    //{
    //    T parentObject = obj.GetComponent<T>();
    //    if (parentObject != null)
    //        yield return parentObject;

    //    foreach (T child in obj.GetComponentsInChildren<T>())
    //    {
    //        yield return child;
    //    }
    //}

    private IEnumerable<T> GetAll<T>(GameObject obj)
    {
        List<Component> components = new List<Component>();
        obj.GetComponents(typeof(T), components);
        components.AddRange(obj.GetComponentsInChildren(typeof(T)));

        return components.Cast<T>();
    }
}


class GeometryHelper
{
    public static Vector3? GetPositionFromMouse()
    {
        //Transform transform = GetComponent<Transform>();
        Plane plane = new UnityEngine.Plane(Vector3.back, Vector3.zero);

        //Plane plane = Plane.GetComponent<Plane>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hit;

        //Vector3 newPosition = new Vector3((float)System.Math.Round(ray.origin.x), 1, (float)System.Math.Round(ray.origin.z));
        float center;

        if (plane.Raycast(ray, out center))
        {
            Vector3 point = ray.GetPoint(center);
            return point;
        }

        return null;
    }
}