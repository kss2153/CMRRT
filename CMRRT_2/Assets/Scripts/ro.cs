using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ro : MonoBehaviour {
	public GameObject rotater;
	public GameObject arrow;
	public GameObject circlet;

	public Camera ARcam;
	GameObject cam;

	public GameObject currentWayfinding;

	// Use this for initialization
	void Start () {
		
		rotater = GameObject.Find("rotater");
		arrow = GameObject.Find("arrow");
		circlet = GameObject.Find("circlet");
		cam = GameObject.Find ("ARCamera");

		rotater.transform.SetParent (cam.transform);
		rotater.transform.position = cam.transform.position + cam.transform.forward * 10f;


	}

	void wayfindRunner(){

		Debug.Log ("distance: " + Math.Sqrt (Math.Pow(currentWayfinding.transform.position.x,2) + Math.Pow(currentWayfinding.transform.position.y, 2) ));
		Debug.Log ("COORMS: " + currentWayfinding.transform.position.x + ", " + currentWayfinding.transform.position.y + ", " + currentWayfinding.transform.position.z);   
//		if (Math.Sqrt (Math.Pow (currentWayfinding.transform.position.x, 2) + Math.Pow (currentWayfinding.transform.position.y, 2)) > 300) {

		Vector3 diff = currentWayfinding.transform.position - cam.transform.position;
		Vector3 planeProjection = Vector3.ProjectOnPlane (diff, cam.transform.forward);

		var radians = Math.Atan2 (currentWayfinding.transform.position.y - rotater.transform.position.y, currentWayfinding.transform.position.x - rotater.transform.position.x);
			var angle = radians * (180 / Math.PI);
			Debug.Log ("triggered" + angle);
			arrow.SetActive(true);			
			circlet.SetActive(true);
			circlet.transform.position = currentWayfinding.transform.position;
			//rotater.transform.rotation = Quaternion.Euler (0, (float)angle, 0);
//			rotater.transform.rotation = Quaternion.FromToRotation(cam.transform.up, planeProjection);
//		arrow.transform.LookAt(currentWayfinding.transform);

		rotater.transform.rotation = Quaternion.LookRotation (planeProjection);
//		arrow.transform.position += new Vector3 (1.5f * (1 - Time.fixedTime % 2), 0f, 0f);
//		} else {
//			arrow.SetActive(false); 
//			circlet.SetActive(false);

//		}
	}

	// Update is called once per frame
	void Update () {
		
		
		if (currentWayfinding != null) {
//			print ("wayfinding");
			arrow.transform.GetChild(0).gameObject.GetComponent<Renderer> ().enabled = true;
//			circlet.transform.GetChild(0).gameObject.GetComponent<Renderer> ().enabled = true;
			wayfindRunner ();
		} else {
			arrow.transform.GetChild(0).gameObject.GetComponent<Renderer> ().enabled = false;
			circlet.transform.GetChild(0).gameObject.GetComponent<Renderer> ().enabled = false;
		}
	}

	public void setWayfindingObject(GameObject obj) {
		currentWayfinding = obj;
	}

	public void unsetWayfindingObject() {
		currentWayfinding = null;
	}
}
