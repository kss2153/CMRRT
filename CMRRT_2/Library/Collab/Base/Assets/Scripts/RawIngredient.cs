using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using AssemblyCSharp;

public class RawIngredient : MonoBehaviour {
	public GameObject workspace;
	public string rawIngredientName;

//	RawIngredient rawIngredient;
//	HashSet<RawIngredient> workspaceRawIngredients;

	static float MAX_DISTANCE = 25;

	// Use this for initialization
	void Start () {
		//// May be a bad idea because this class will now depend on two other classes (i.e. Workspace and Data need to run start before this class can)
//		rawIngredient = workspace.GetComponent<Data> ().findIngredient (rawIngredientName);
//		workspaceRawIngredients = workspace.GetComponent<Workspace> ().rawIngredients;

		transform.Find ("Text").GetComponent<TextMesh> ().text = rawIngredientName;

		Ingredient rawIngredient = workspace.GetComponent<Data> ().findIngredient (rawIngredientName);
		print (rawIngredient.name);
		transform.Find ("Big Cube").GetComponent<Renderer> ().material.color = Color.HSVToRGB ((float) rawIngredient.color.h / 360f, (float) rawIngredient.color.b / 100f, (float) rawIngredient.color.b / 100f); 
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray (transform.position, -transform.up);
		Debug.DrawLine (transform.position, transform.position + (-transform.up * MAX_DISTANCE), Color.red);

		RaycastHit[] hits = Physics.RaycastAll(ray, MAX_DISTANCE);

		workspace.GetComponent<Workspace> ().rawIngredients.Remove (workspace.GetComponent<Data> ().findIngredient (rawIngredientName));

		foreach (RaycastHit hit in hits) {
			Debug.DrawLine (hit.point, hit.point + Vector3.up * 5, Color.green);

			if (GameObject.ReferenceEquals(hit.transform.gameObject, workspace.transform.Find ("Workspace plane").Find ("Decorative plane").gameObject)) {
//				Debug.Log ("Raw ingredient detected");
				workspace.GetComponent<Workspace> ().rawIngredients.Add (workspace.GetComponent<Data> ().findIngredient (rawIngredientName));
			}
		}
	}
}
