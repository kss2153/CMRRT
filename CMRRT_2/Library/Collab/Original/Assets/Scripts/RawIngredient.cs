using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class RawIngredient : MonoBehaviour {
	public GameObject workspace;
	public string rawIngredientName;

	bool touching;

//	RawIngredient rawIngredient;
//	HashSet<RawIngredient> workspaceRawIngredients;

	static float MAX_DISTANCE = 25;

	// Use this for initialization
	void Start () {

		//// May be a bad idea because this class will now depend on two other classes (i.e. Workspace and Data need to run start before this class can)
//		rawIngredient = workspace.GetComponent<Data> ().findIngredient (rawIngredientName);
//		workspaceRawIngredients = workspace.GetComponent<Workspace> ().rawIngredients;
	}
	
	// Update is called once per frame
	void Update () {
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
