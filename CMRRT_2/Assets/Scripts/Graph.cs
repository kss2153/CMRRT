using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class Graph : MonoBehaviour {

	private GameObject workspace; 
	private HashSet<Ingredient> currentIngredients;
	private List<Experiment> allRecords;
	private List<Experiment> currentRecords = new List<Experiment>();
	public Dictionary<HashSet<Ingredient>, List<Experiment>> dict;

	public Transform edgePrefab;
	public Transform labelPrefab;

	public float graphScale = 1f;
	private float gFactorMin = 1f;
	private float gFactorMax = 2.27f;

	public float radius = 0.6f;
	// Use this for initialization
	void Start () {
		workspace = GameObject.Find("Workspace");
		currentIngredients = new HashSet<Ingredient>();
		allRecords = workspace.GetComponent<Data> ().records;
		dict = workspace.GetComponent<Data> ().dict;
		updateGraph ();
	}
	
	// Update is called once per frame
	void Update () {
//		updateGraph ();
		GameObject center = GameObject.Find("center_node");
		if (center != null) {
			Vector3 original = new Vector3 (0.22f, 0.22f, 0.22f);
			if (graphScale > gFactorMax)
				graphScale = gFactorMax;
			if (graphScale < gFactorMin)
				graphScale = gFactorMin;
			center.transform.localScale = original * graphScale;
		}
	}

	public Vector3[] pointsOnSphere(int n)
	{
		List<Vector3> upts = new List<Vector3>();
		float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
		float off = 2.0f / n;
		float x = 0;
		float y = 0;
		float z = 0;
		float r = 0;
		float phi = 0;

		for (var k = 0; k < n; k++){
			y = k * off - 1 + (off /2);
			r = Mathf.Sqrt(1 - y * y);
			phi = k * inc;
			x = Mathf.Cos(phi) * r;
			z = Mathf.Sin(phi) * r;

			upts.Add(new Vector3(x, y, z));
		}
		Vector3[] pts = upts.ToArray();
		return pts;
	}

	public void updateGraph() {
//		print ("updating");
		allRecords = workspace.GetComponent<Data> ().records;
		currentIngredients = workspace.GetComponent<Workspace> ().rawIngredients;
		print (currentIngredients.Count);
		clearGraph ();
		setCurrentRecords ();
		createGraph ();
	}

	void clearGraph() {
		
		foreach (Transform child in transform) {
			if (child.gameObject.name != "StatusParent") {
				Destroy (child.gameObject);
			}
		}
	}

	void createGraph() {

		Vector3 rgb;
		if (currentRecords.Count > 0)
			rgb = hsb2rgb (currentRecords [0].color);
		else
			rgb = new Vector3(1f, 1f, 1f);

		Vector3 center = new Vector3 (0f, 1f, 0f);
		GameObject c = createSphere ("center_node", center, transform);

		c.GetComponent<Renderer> ().material.color = new Color (rgb.x, rgb.y, rgb.z, 1f);
		c.transform.localScale *= 2f;


		HashSet<string> names = new HashSet<string> ();
		foreach (Ingredient i in currentIngredients) {
			names.Add (i.name);
		}
		Vector3[] points = pointsOnSphere (workspace.GetComponent<Data> ().ingredients.Count);
		int counter = 0;
		foreach (Ingredient i in workspace.GetComponent<Data> ().ingredients) {
			if (names.Contains(i.name))
				continue;

			HashSet<Ingredient> cur_copy = new HashSet<Ingredient> (currentIngredients);
			cur_copy.Add (i);
			if (dict [cur_copy].Count > 0) {
				Experiment e = dict [cur_copy] [0];
				Vector3 rand = Random.insideUnitSphere.normalized;
				rand = points [counter] * radius;
				counter++;
				Vector3 pos = rand * 0.8f + center;
				GameObject node;
				if (i.tool)
					node = createCube (i.name, pos, c.transform);
				else
					node = createSphere (i.name, pos, c.transform);
				GameObject label = createLabel (i.name, pos, c.transform, dict [cur_copy].Count);
				node.tag = "Node";


				GameObject edge = createEdge ("edge_to_" + e.experiment_id, center, pos, c.transform);
				edge.GetComponent<Renderer> ().material.color = Color.green;



				Texture2D texture = new Texture2D(dict [cur_copy].Count, 1, TextureFormat.ARGB32, false);
				for (int j = 0; j < dict [cur_copy].Count; j++) {
					Experiment ex = dict [cur_copy] [j];
					rgb = hsb2rgb (ex.color);
					Data d = GameObject.Find ("Workspace").GetComponent<Data> ();
					Color eColor = Color.white;
					if (d.cleanedActivityToColor.ContainsKey(d.cleanActivity(ex.activity)))
						eColor =d.cleanedActivityToColor [d.cleanActivity(ex.activity)];
					print (eColor.r + ", " + eColor.g + ", " + eColor.b);
					texture.SetPixel(j, 0, eColor);
				}
				texture.Apply();
				node.GetComponent<Renderer> ().material.mainTexture = texture;
			}

		
		}

		foreach (Ingredient i in currentIngredients) {

			HashSet<Ingredient> cur_copy = new HashSet<Ingredient> (currentIngredients);
			cur_copy.Remove (i);

			if (dict [cur_copy].Count > 0) {
				Experiment e = dict [cur_copy] [0];
				Vector3 rand = Random.insideUnitSphere.normalized;
				rand = points [counter] * radius;
				counter++;
				Vector3 pos = rand * 0.8f + center;
				GameObject node;
				if (i.tool)
					node = createCube (i.name, pos, c.transform);
				else
					node = createSphere (i.name, pos, c.transform);
				node.tag = "R_Node";
				GameObject label = createLabel (i.name, pos, c.transform, dict [cur_copy].Count);

				GameObject edge = createEdge ("edge_to_" + e.experiment_id, center, pos, c.transform);
				edge.GetComponent<Renderer> ().material.color = Color.red;

				Texture2D texture = new Texture2D(dict [cur_copy].Count, 1, TextureFormat.ARGB32, false);
				for (int j = 0; j < dict [cur_copy].Count; j++) {
					Experiment ex = dict [cur_copy] [j];
					rgb = hsb2rgb (ex.color);
					Data d = GameObject.Find ("Workspace").GetComponent<Data> ();
					Color eColor = Color.white;
					if (d.cleanedActivityToColor.ContainsKey(d.cleanActivity(ex.activity)))
						eColor =d.cleanedActivityToColor [d.cleanActivity(ex.activity)];
					print (eColor.r + ", " + eColor.g + ", " + eColor.b);
					texture.SetPixel(j, 0, eColor);
				}
				texture.Apply();
				node.GetComponent<Renderer> ().material.mainTexture = texture;
			}
		}
			
		GetComponent<GraphEventHandler> ().newGraph ();

	}

	GameObject createEdge(string name, Vector3 point1, Vector3 point2, Transform parent) {
		GameObject edge = Instantiate<GameObject>(edgePrefab.gameObject, Vector3.zero, Quaternion.identity);
		edge.name = name;
		edge.transform.SetParent (transform);
		edge.transform.localPosition = (point1 + point2) / 2f;
		Vector3 diff = point2 - point1;
		Vector3 scale = edge.transform.localScale;
		scale.y = diff.magnitude / 2f;
		edge.transform.localScale = scale;
		edge.transform.rotation = Quaternion.FromToRotation(transform.up, point2-point1);
		edge.transform.SetParent (parent);
		return edge;
	}

	GameObject createSphere(string name, Vector3 local_pos, Transform parent) {
		GameObject node = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		node.name = name;
		node.transform.SetParent (transform);
		node.transform.localPosition = local_pos;
		node.transform.SetParent (parent);
		return node;
	}

	GameObject createCube(string name, Vector3 local_pos, Transform parent) {
		GameObject node = GameObject.CreatePrimitive (PrimitiveType.Cube);
		node.name = name;
		node.transform.SetParent (transform);
		node.transform.localPosition = local_pos;
		node.transform.SetParent (parent);
		return node;
	}

	GameObject createLabel(string name, Vector3 pos, Transform parent, int num) {
		GameObject label = Instantiate<GameObject>(labelPrefab.gameObject, Vector3.zero, Quaternion.identity);
		label.name = name + "_label";
		print (label.name);
		label.transform.SetParent (transform);
		label.transform.localPosition = pos + transform.up*0.15f;
		label.transform.SetParent (parent);
		TextMesh txt = label.transform.GetChild (0).gameObject.GetComponent<TextMesh>();
		txt.text = name + ": " + num;
		return label;
	}

	// adapted from stackexchange
	Vector3 hsb2rgb(HSB hsb_in) {
		HSB hsb = new HSB (hsb_in.h, hsb_in.s, hsb_in.b);
		print (hsb.h + ", " + hsb.s + ", " + hsb.b);

		hsb.s /= 100f;
		hsb.b /= 100f;
		float hh = hsb.h;
		if (hh >= 360.0f) hh = 0.0f;
		hh /= 60.0f;

		long i = (long)hh;
		float ff = hh - i;

		float p = hsb.b * (1.0f - hsb.s);
		float q = hsb.b * (1.0f - (hsb.s * ff));
		float t = hsb.b * (1.0f - (hsb.s * (1.0f - ff)));

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
		return result;   
	}

	void setCurrentRecords() {
		currentRecords = dict [new HashSet<Ingredient> (currentIngredients)];
		print (currentIngredients.Count);
	}

	


}
