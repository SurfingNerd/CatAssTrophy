using UnityEngine;
using System.Collections;

public class DragHandler3D : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 scanPos;


		
	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(scanPos);


		//offset = scanPos - Camera.main.ScreenToWorldPoint(
		//	new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

	}


	void OnMouseDrag()
	{

		GameObject cat = GameObject.Find ("coolCat");

		if ((cat.GetComponent<Rigidbody2D>().velocity.magnitude < 0.5) ) {
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

//		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		transform.position = curPosition;
		this.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	void OnMouseUp()
	{
		
		this.GetComponent<BoxCollider2D> ().enabled = true;
	}


	public void addThisObject()
	{

		Debug.Log ("moving..");
		transform.position = new Vector3 (5f, 3f, -2.0f);

	}



}



