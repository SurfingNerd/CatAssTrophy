using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttonClick : MonoBehaviour {
	bool started = false;
	Vector3 startVector;

	public void RestartLevel(){
		// Save game data

		// Close game
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void startLevel(){

		if (!started) {
			// Save game data
			GameObject cat = GameObject.Find ("coolCat");
			startVector = cat.transform.position;
			cat.GetComponent<Rigidbody2D> ().isKinematic = false;
			started = true;
		} else {
		
			GameObject cat = GameObject.Find ("coolCat");
			cat.transform.position = startVector;
		
		}
		// Close game
	}

}