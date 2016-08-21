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


    private void StartPlacementService()
    {
        AssetPlacement.PlacementService2D placementService = GameObject.FindObjectOfType<AssetPlacement.PlacementService2D>();
        if (placementService != null)
        {
            placementService.StartGame();
        }
    }

	public void startLevel(){

		if (!started) {
			// Save game data
			GameObject cat = GameObject.Find ("coolCat");
			startVector = cat.transform.position;
			cat.GetComponent<Rigidbody2D> ().isKinematic = false;
			started = true;
            StartPlacementService();
        } else {
		
			GameObject cat = GameObject.Find ("coolCat");
			cat.transform.position = startVector;
		
		}
		// Close game
	}

}