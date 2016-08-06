using UnityEngine;
using System.Collections;

public class nextLevel : MonoBehaviour {
	

	public int waitSeconds = 5;

	private bool rotating = false; 
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		if (rotating) {
			GameObject.Find ("hiding-background").transform.Rotate(1,0,0);

		}

		if (Input.anyKey) {


			rotating = true; 

			//	transform.Rotate (0,0,rotationSpeed);



			StartCoroutine(LoadAfterTime(waitSeconds));


		}
	}

	public IEnumerator LoadAfterTime(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		Application.LoadLevel("1-levelPicker");
	}

}
