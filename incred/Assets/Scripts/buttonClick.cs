using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttonClick : MonoBehaviour {

	public void RestartLevel(){
		// Save game data

		// Close game
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void startLevel(){
		// Save game data
		GameObject cat = GameObject.Find("coolCat");
		cat.GetComponent<Rigidbody2D> ().isKinematic = false;

		// Close game
	}

}