using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Graph : MonoBehaviour {

	private GameObject workspace; 
	private List<Ingredient> currentIngredients;
	private List<Experiment> allRecords;
	private List<Experiment> currentRecords = new List<Experiment>();

	public Transform edgePrefab;


	// Use this for initialization
	void Start () {
		workspace = GameObject.Find("Workspace");
		currentIngredients = workspace.GetComponent<Workspace> ().rawIngredients;
		allRecords = workspace.GetComponent<Data> ().records;
		setCurrentRecords ();
		createGraph ();

		print (currentRecords.Count);

//		print (currentRecords [0].color + "  " + hsb2rgb (currentRecords [0].color));

	}
	
	// Update is called once per frame
	void Update () {
		currentIngredients = workspace.GetComponent<Workspace> ().rawIngredients;
	}

	void createGraph() {

		Vector3 center = new Vector3 (0f, 1f, 0f);
		GameObject c = createSphere ("center_node", center, transform);
		c.transform.localScale *= 2f;
		foreach (Experiment e in currentRecords [0].green_edges) {
			Vector3 rand = Random.insideUnitSphere.normalized;
			Vector3 pos = rand * 0.8f + center;
			createSphere ("neighbor_" + e.experiment_id, pos, c.transform);
			createEdge ("edge_to_" + e.experiment_id, center, pos, c.transform);
		}


	}

	void createEdge(string name, Vector3 point1, Vector3 point2, Transform parent) {
		GameObject edge = Instantiate<GameObject>(edgePrefab.gameObject, Vector3.zero, Quaternion.identity);
		edge.name = name;
		edge.transform.SetParent (transform);
		edge.transform.localPosition = (point1 + point2) / 2f;
		Vector3 diff = point2 - point1;
		Vector3 scale = edge.transform.localScale;
		scale.y = diff.magnitude / 2f;
		edge.transform.localScale = scale;
		edge.transform.rotation = Quaternion.FromToRotation(Vector3.up, point2-point1);
		edge.transform.SetParent (parent);
	}

	GameObject createSphere(string name, Vector3 local_pos, Transform parent) {
		GameObject node = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		node.name = name;
		node.transform.SetParent (transform);
		node.transform.localPosition = local_pos;
		node.transform.SetParent (parent);
		return node;
	}


	Vector3 hsb2rgb(HSB hsb) {
		print (hsb.h + ", " + hsb.s + ", " + hsb.b);
		hsb.h /= 100f;
		hsb.s /= 100f;
		hsb.b /= 100f;
		float hh = hsb.h;
		if(hh >= 360.0f) hh = 0.0f;
		hh /= 60.0f;

		long i = (long)hh;
		print (i);
		print (hh);
		float ff = hh - i;

		print (hsb.b);
		float p = hsb.b * (1.0f - hsb.s);
		float q = hsb.b * (1.0f - (hsb.s * ff));
		float t = hsb.b * (1.0f - (hsb.s * (1.0f - ff)));
		print (q);

		Vector3 result = new Vector3();

		switch(i) {
		case 0:
			result.x = hsb.b;
			result.y = t;
			result.z = p;
			break;
		case 1:
			result.x = q;
			result.y = hsb.b;
			result.z = p;
			break;
		case 2:
			result.x = p;
			result.y = hsb.b;
			result.z = t;
			break;

		case 3:
			result.x = p;
			result.y = q;
			result.z = hsb.b;
			break;
		case 4:
			result.x = t;
			result.y = p;
			result.z = hsb.b;
			break;
		case 5:
		default:
			result.x = hsb.b;
			result.y = p;
			result.z = q;
			break;
		}
		return result * 255f;   
	}

	void setCurrentRecords() {
		foreach (Experiment e in allRecords) {
			
			bool eq = true;
			if (e.materials.Length != currentIngredients.Count)
				eq = false;
			else {
				foreach (Ingredient i in currentIngredients) {
					if (!e.contains (i))
						eq = false;
				}
			}
			if (eq)
				currentRecords.Add (e);
		}
	}


}
