using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
	static public FollowCam S; //a followCam Singleton

	//fields set in the unity inspector pane
	public bool ___________________________________________________________________;
	//fields set dynamically
	public GameObject poi; // point of interest
	public float camZ; //desired Z position of the camera

	void Awake(){
		S = this;
		camZ = this.transform.position.z;
	}
		
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//there's only one line following an if, if doesn't need braces
		if (poi == null) { //if there's no poi
			return;
		}

		//get the position of poi
		Vector3 destination = poi.transform.position;
		//retain a destination.z of camZ
		destination.z = camZ;
		//set the camera to the destination
		transform.position = destination;
	}
}
