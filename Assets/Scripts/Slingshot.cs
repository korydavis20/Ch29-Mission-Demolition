using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {
	public GameObject prefabProjectile;
	public float velocityMult = 4f;
	public bool ____________________________________________________;
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;

	void Awake(){
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive (false);
		launchPos = launchPointTrans.position;
	}
		
	void OnMouseEnter(){
		//print ("Slingshot:OnMouseEnter ()");
		launchPoint.SetActive (true);
	}

	void OnMouseExit(){
		//print("Slingshot:OnMouseExit");
		launchPoint.SetActive (false);
	}

	void OnMouseDown(){
		//the player has pressed the mouse button while over Slingshot
		aimingMode = true;
		//Instantiage a projectile
		projectile = Instantiate(prefabProjectile) as GameObject;
		//start it at launch point
		projectile.transform.position = launchPos;
		//set it to isKinematic for now
		projectile.GetComponent<Rigidbody>().isKinematic = true;
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if slingshot is not in aimingmode, don't run this code
		if(!aimingMode){ return; }
		//get current mouse position in 2D screen coordinates
		Vector3 mousePos2D = Input.mousePosition;
		//convert mouse position to 3D world coordinates
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);
		//find delta from the launch position to the mousepos 3D
		Vector3 mouseDelta = mousePos3D - launchPos;
		//Limit mousedelta to the radius of the slingshot sphere collider
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if (mouseDelta.magnitude > maxMagnitude) {
			mouseDelta.Normalize();
			mouseDelta *= maxMagnitude;
		}
		//move the projectile to this new position
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;

		if(Input.GetMouseButtonUp(0)){
			//the mouse has been released
			aimingMode = false;
			projectile.GetComponent<Rigidbody> ().isKinematic = false;
			projectile.GetComponent<Rigidbody> ().velocity = -mouseDelta * velocityMult;
			FollowCam.S.poi = projectile;
			projectile = null;
		}
	}
}
