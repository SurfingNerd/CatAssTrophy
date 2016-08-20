using UnityEngine;
using System.Collections;

public class buttonHandler : MonoBehaviour {

//	GameManager 


	public void LoadLevel(int levelNumber) {

        LevelManager.LoadLevel(levelNumber);
	}

}
