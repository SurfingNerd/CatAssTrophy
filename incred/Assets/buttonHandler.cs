using UnityEngine;
using System.Collections;

public class buttonHandler : MonoBehaviour {

//	GameManager 


	public void LoadLevel(int levelNumber) {


		if(levelNumber==0) {
			Application.LoadLevel("0-startScreen");

		}

		if(levelNumber==1) {
			Application.LoadLevel("1-levelPicker");

		}

		if(levelNumber==2) {
			Application.LoadLevel("2-level");

		}

		else if(levelNumber==3) {
			Application.LoadLevel("3-level");

		}


		else if(levelNumber==4) {
			Application.LoadLevel("4-level");

		}

		else if(levelNumber==4) {
			Application.LoadLevel("4-level");

		}
		else if(levelNumber==5) {
			Application.LoadLevel("5-level");

		}
		else if(levelNumber==6) {
			Application.LoadLevel("6-level");

		}
		else if(levelNumber==7) {
			Application.LoadLevel("7-level");

		}
		else if(levelNumber==8) {
			Application.LoadLevel("8-level");

		}
		else if(levelNumber==9) {
			Application.LoadLevel("9-level");

		}


	}

}
