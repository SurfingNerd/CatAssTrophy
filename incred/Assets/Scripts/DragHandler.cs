using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject itemBeingDragged;
	Vector3 startPosition;
	Transform startParent;

	public Vector3          positionToReturnTo;



	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		positionToReturnTo = this.transform.position;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
		Debug.Log ("OnBeginDrag");


	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		this.transform.position = eventData.position;
		Debug.Log ("OnDrag" + eventData.position);

	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		//this.transform.position = positionToReturnTo;
		Debug.Log ("OnEndDrag");


	}

	#endregion



}
