using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	//static field accessible by code anywhere
	static public bool goalMet = false;


	void OnTriggerEnter (Collider other) {
		//when the trigger is hit by something
		//check to see if it's a projectile
		if(other.gameObject.tag == "Projectile"){
			//if so, set goalMet to true
			Goal.goalMet = true;
			//Also set the alpha of the color to higher opacity
			Color c = GetComponent<Renderer>().material.color;
			c.a = 0.9f;
			GetComponent<Renderer>().material.color = c;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
