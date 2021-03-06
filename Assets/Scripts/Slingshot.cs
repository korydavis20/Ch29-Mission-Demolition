﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {
	static private Slingshot S;
	[Header("Set in Inspector")]
	public GameObject prefabProjectile;
	public float velocityMult = 8f;
	[Header("Set Dynamically")]
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;
	private Rigidbody projectileRigidbody;

	static public Vector3 LAUNCH_POS{
		get{
			if(S == null)	return Vector3.zero;
				return S.launchPos;
		}
	}
	void Awake(){
		//set the slingshot singleton
		S = this;
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
		//projectile.GetComponent<Rigidbody>().isKinematic = true;
		projectileRigidbody = projectile.GetComponent<Rigidbody> ();
		projectileRigidbody.isKinematic = true;
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
			projectileRigidbody.isKinematic = false;
			projectileRigidbody.velocity = -mouseDelta * velocityMult;
			FollowCam.POI = projectile;
			projectile = null;
			MissionDemolition.ShotFired();
			ProjectileLine.S.poi = projectile;
		}
	}
}
