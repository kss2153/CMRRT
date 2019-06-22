using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using AssemblyCSharp;

public class RawIngredient : MonoBehaviour {
	public GameObject workspace;
	public string rawIngredientName;

	bool touching;

//	RawIngredient rawIngredient;
//	HashSet<RawIngredient> workspaceRawIngredients;

	// max ray distance used for raw ingredient detection
	static float MAX_DISTANCE = 25;

	// Use this for initialization
	void Start () {

		//// May be a bad idea because this class will now depend on two other classes (i.e. Workspace and Data need to run start before this class can)
//		rawIngredient = workspace.GetComponent<Data> ().findIngredient (rawIngredientName);
//		workspaceRawIngredients = workspace.GetComponent<Workspace> ().rawIngredients;

		transform.Find ("Text").GetComponent<TextMesh> ().text = rawIngredientName;

//		Ingredient rawIngredient = workspace.GetComponent<Data> ().findIngredient (rawIngredientName);
//		print (rawIngredient.name);
//		transform.Find ("Big Cube").GetComponent<Renderer> ().material.color = Color.HSVToRGB ((float) rawIngredient.color.h / 360f, (float) rawIngredient.color.b / 100f, (float) rawIngredient.color.b / 100f); 
	}
	
	// Update is called once per frame
	void Update () {
		// change the color of the border
		Ingredient rawIngredient = workspace.GetComponent<Data> ().findIngredient (rawIngredientName);
		transform.Find ("Big Cube").GetComponent<Renderer> ().material.color = Color.HSVToRGB ((float) rawIngredient.color.h / 360f, (float) rawIngredient.color.b / 100f, (float) rawIngredient.color.b / 100f); 

		// draw ray
		Ray ray = new Ray (transform.position, -transform.up);
		Debug.DrawLine (transform.position, transform.position + (-transform.up * MAX_DISTANCE), Color.red);

		RaycastHit[] hits = Physics.RaycastAll(ray, MAX_DISTANCE);


		bool cur_touching = false;
		foreach (RaycastHit hit in hits) {
			Debug.DrawLine (hit.point, hit.point + Vector3.up * 5, Color.green);
			if (GameObject.ReferenceEquals (hit.transform.gameObject, workspace.transform.Find ("Workspace plane").Find ("Decorative plane").gameObject)) {
//				Debug.Log ("Raw ingredient detected");
				cur_touching = true;
			}
		}
		
		if (cur_touching != touching) {
			touching = cur_touching;
			if (touching) 
				workspace.GetComponent<Workspace> ().addIngredient (workspace.GetComponent<Data> ().findIngredient (rawIngredientName));
			else
				workspace.GetComponent<Workspace> ().removeIngredient (workspace.GetComponent<Data> ().findIngredient (rawIngredientName));
		}


	}
}
