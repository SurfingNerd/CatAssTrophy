using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Levels
{
	using System.Collections.Generic;      

	public class GameManager : MonoBehaviour
	{

		[SerializeField]
		private GameConfig gameConfig = new GameConfig();
		



		private String state = "start";

		public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
		private int level = 0;                                  //Current level number, expressed in game as "Day 1".

		//Awake is always called before any Start functions
		void Awake()
		{
			//Check if instance already exists
			if (instance == null)

				//if not, set instance to this
				instance = this;

			//If instance already exists and it's not this:
			else if (instance != this)

				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);    

			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);

			//Get a component reference to the attached BoardManager script
			//boardScript = GetComponent<BoardManager>();

			//levelManager = GetComponent<LevelManager> ();


			//Call the InitGame function to initialize the first level 
			InitGame();
		}

		//Initializes the game for each level.
		void InitGame()
		{
            LevelManager.LoadLevel(level);
		}



		//Update is called every frame.
		void Update()
		{

		}


		public void OnGUI() {
			#if UNITY_EDITOR
			GUILayout.Label("Current state: " + state);
			GUILayout.Label("Current level: " + level);
			#endif
		}


		public GameConfig GameConfig {
			get {
				return gameConfig;
			}
		}
	}
}