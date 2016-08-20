using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject itemBeingDragged;

	public GameObject itemToDrag;
	//Vector3 startPosition;
	//Transform startParent;

	public Vector3 positionToReturnTo;



	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		positionToReturnTo = this.transform.position;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
		Debug.Log ("OnBeginDrag");



		Debug.Log ("START" + itemToDrag.transform.position);

	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		this.transform.position = eventData.position;
		Debug.Log ("OnDrag" + eventData.position);
		Debug.Log ("OnDrag" + Camera.main.ScreenToWorldPoint(Input.mousePosition));
		//itemToDrag.transform.position = eventData.position;
		itemToDrag.transform.localPosition = eventData.position;
		//itemToDrag.transform.position = eventData.pointerCurrentRaycast.worldPosition;


	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		//this.transform.position = positionToReturnTo;
		Debug.Log ("OnEndDrag" + itemToDrag.transform.position);
		//itemToDrag.transform.position = eventData.position;

		//960 , 520
		itemToDrag.transform.localPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, itemToDrag.transform.localPosition);

	}

	#endregion



}
