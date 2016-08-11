using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void LoadLevel(int levelNumber) {

		if(levelNumber==0) {
			Debug.Log ("hihi 2" );
			Application.LoadLevel("0-startScreen");

		}

		else if(levelNumber==1) {
			Application.LoadLevel("1-levelselector");

		}


		else if(levelNumber==2) {
			Application.LoadLevel("2-level");

		}
	}


}
