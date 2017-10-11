using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
	static public FollowCam S; //a followCam Singleton
	static public GameObject POI; // the satic point of interest
	//fields set in the unity inspector pane
	[Header("Set in Inspector")]
	public float easing = 0.05f;
	public Vector2 minXY = Vector2.zero;

	[Header("Set Dynamically")]
	//fields set dynamically
	public float camZ; //desired Z position of the camera

	void Awake(){
		camZ = this.transform.position.z;
	}


	void FixedUpdate () {
		Vector3 destination;
		//if there's no poi return to P:[0,0,0]
		if (POI == null) { //if there's no poi
			destination = Vector3.zero;
		} else {
			//get the position of the poi
			destination = POI.transform.position;
			destination.x = Mathf.Max (minXY.x, destination.x);
			destination.y = Mathf.Max (minXY.y, destination.y);
			//interpolate from the current camera postion toward destination
			destination = Vector3.Lerp(transform.position, destination, easing);
			if (POI.tag == "Projectile") {
				//if it is sleeping (that is, not moving)
				if(POI.GetComponent<Rigidbody>().IsSleeping()){
					//return to default view
					POI = null;
					//in next update
					return;
				}
			}
		}
			
		//limit the x and y to minimum values
		destination.x = Mathf.Max(minXY.x, destination.x);
		destination.y = Mathf.Max(minXY.y, destination.y);
		//interpolate from the current camera position toward destination
		destination = Vector3.Lerp(transform.position, destination, easing);
		//retain a destination.z of camZ
		destination.z = camZ;
		//set the camera to the destination
		transform.position = destination;
		//set the orthrographic size of the camera to keep ground in view
		Camera.main.orthographicSize = destination.y + 10;
	}
}
