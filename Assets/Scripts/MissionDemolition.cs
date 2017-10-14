using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode {
	idle,
	playing,
	levelEnd
}

public class MissionDemolition : MonoBehaviour {
	static private MissionDemolition S; // a Singleton
	// fields set in the Unity Inspector pane
	[Header("Set in Inspector")]
	public GameObject[] castles; // An array of the castles
	public Text uitLevel; // The GT_Level GUIText
	public Text uitShots; // The GT_Score GUIText
	public Text uitButton;
	public Vector3 castlePos; // The place to put castles

	[Header("Set Dynamically")]
	// fields set dynamically
	public int level; // The current level
	public int levelMax; // The number of levels
	public int shotsTaken;
	public GameObject castle; // The current castle
	public GameMode mode = GameMode.idle;
	public string showing = "Show Slingshot"; // FollowCam mode

	void Start() {
		S = this; // Define the Singleton
		level = 0;
		levelMax = castles.Length;
		StartLevel();
	}

	void StartLevel() {
		// Get rid of the old castle if one exists
		if (castle != null) {
			Destroy( castle );
		}
		// Destroy old projectiles if they exist
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
		foreach (GameObject pTemp in gos) {
			Destroy( pTemp );
		}
		// Instantiate the new castle
		castle = Instantiate<GameObject>( castles[level] );
		if(castle != GameObject.FindGameObjectWithTag("Forest")){
			castle.transform.position = castlePos;
		}

		shotsTaken = 0;
		// Reset the camera
		SwitchView("wShow Both");
		ProjectileLine.S.Clear();
		// Reset the goal
		Goal.goalMet = false;
		UpdateGUI();
		mode = GameMode.playing;
	}

	void UpdateGUI(){
		uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
		uitShots.text = "Shots Taken: " + shotsTaken;
	}

	void Update() {
		// Check for level end
		UpdateGUI();
		if ((mode == GameMode.playing) && Goal.goalMet) {
			// Change mode to stop checking for level end
			mode = GameMode.levelEnd;
			// Zoom out
			SwitchView("Show Both");
			// Start the next level in 2 seconds
			Invoke("NextLevel", 2f);
		}
	}
	void NextLevel() {
		level++;
		if (level == levelMax) {
			level = 0;
		}
		StartLevel();
	}

	public void SwitchView(string eView = ""){
		if(eView ==""){
			eView = uitButton.text;
		}
		showing = eView;
		switch (showing) {
			case"Show Slingshot":
				FollowCam.POI = null;
				uitButton.text = "Show Castle";
				break;

			case"Show Castle":
				FollowCam.POI = S.castle;
				uitButton.text = "Show Both";
				break;

			case"Show Both":
				FollowCam.POI = GameObject.Find ("ViewBoth");
				uitButton.text = "Show Slingshot";
				break;

		}
	}

	public static void ShotFired(){
		S.shotsTaken++;
	}

}