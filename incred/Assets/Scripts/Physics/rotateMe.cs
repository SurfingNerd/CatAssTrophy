using UnityEngine;
using System.Collections;

public class rotateMe : MonoBehaviour {

	public float rotationSpeed = 0.1f; 
	public float moveSpeed = 0.1f; 
	public int direction = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (direction == 0){
		transform.position += Vector3.left * moveSpeed;
		}
		else if (direction == 1) {
		transform.position += Vector3.right * moveSpeed;
		}
		transform.Rotate (0,0,rotationSpeed);

		//transform.Rotate(Vector3.back * Time.deltaTime * rotationSpeed, Space.Self  );

		//transform.Translate (Vector3.right * Time.deltaTime * moveSpeed );

	

//		Vector3 PlayerRot PlayerRot = Player.eulerAngles; Arrow.transform.rotation = Quaternion.Euler (0, 0, PlayerRot.y);


	}
}
