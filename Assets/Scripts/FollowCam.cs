using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
	static public FollowCam S; //a followCam Singleton

	//fields set in the unity inspector pane
	public float easing = 0.05f;
	public Vector2 minXY;
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
	void FixedUpdate () {
		Vector3 destination;
		//if there's no poi return to P:[0,0,0]
		if (poi == null) { //if there's no poi
			destination = Vector3.zero;
		} else {
			//get the position of the poi
			destination = poi.transform.position;
			//if poit is a projectile, check to see if it's at rest
			if (poi.tag == "Projectile") {
				//if it is sleeping (that is, not moving)
				if(poi.GetComponent<Rigidbody>().IsSleeping()){
					//return to default view
					poi = null;
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
		this.GetComponent<Camera>().orthographicSize = destination.y + 10;
	}
}
