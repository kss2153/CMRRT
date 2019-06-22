using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour {

	private GameObject center_node;
	private GameObject cam;
	private bool rotating = false;
	private float cur_x;
	private Vector3 current;
	private Vector3 hand;

	public GameObject w_button;
	public Transform bubblePrefab;
	public Button rotateButton;
	public Button scalingButton;


	public string selectedIngredient = "";

	public bool r = false;
	public bool scaling = false;
	public bool wayfinding = false;

	public Transform edgePrefab;


	// Use this for initialization
	void Start () {
//		w_button = GameObject.Find ("WayfindingButton");
		w_button.SetActive (false);
		current = transform.position;
		cam = GameObject.Find ("ARCamera");

		w_button.GetComponent<Button> ().onClick.AddListener (wayfind);
		rotateButton.onClick.AddListener (setHandPos);
		scalingButton.onClick.AddListener (setScalingOn);
	}

	void rotateY(float angle) {
		Vector3 cam_up = cam.transform.up;
		center_node = GameObject.Find("center_node");
		center_node.transform.RotateAround (center_node.transform.position, cam_up, angle);
	}

	void rotateX(float angle) {
		Vector3 cam_right = cam.transform.right;
		center_node = GameObject.Find("center_node");
		center_node.transform.RotateAround (center_node.transform.position, cam_right, angle);
	}

	Vector2 getMotionDirection(Vector3 cur) {

		Transform cam_transform = cam.transform;
		Vector3 rel = cam_transform.InverseTransformDirection(cur - (hand + transform.up));
		return new Vector2 (rel.x, rel.y);

	}


	// Update is called once per frame
	void Update () {
		if (!GetComponent<HandEventHandler>().tracked)
			GameObject.Find ("hand_pointer").GetComponent<Renderer> ().enabled = false;

		if (!GetComponent<HandEventHandler> ().tracked) {
			rotateButton.gameObject.SetActive (false);
			scalingButton.gameObject.SetActive (false);
		} else {
			rotateButton.gameObject.SetActive(true); 
			scalingButton.gameObject.SetActive(true);
		}

		if (r) {
			Vector2 dir = getMotionDirection (GameObject.Find ("hand2").transform.position);
			if (Mathf.Abs (dir.y) > 2f)
				rotateX (dir.y / 2.5f);
			if (Mathf.Abs (dir.x) > 2f)
				rotateY (-dir.x / 2.5f);

			if (GameObject.Find ("hand_edge") != null)
				Destroy (GameObject.Find ("hand_edge"));

			Color c = Color.red;
			if (Mathf.Abs (dir.y) > 2f || Mathf.Abs (dir.x) > 2f)
				c = Color.green;
			
			GameObject e = createEdge ("hand_edge", GameObject.Find ("hand2").transform.position, hand);
			c.a = 0.7f;
			e.GetComponent<Renderer> ().material.color = c;
		} else if (scaling) {
			Vector2 dir = getMotionDirection (GameObject.Find ("hand2").transform.position);
			Color c = Color.red;
			if (Mathf.Abs (dir.x) > 2f) {
				c = Color.green;
				if (dir.x < 0)
					GameObject.Find ("Graph").GetComponent<Graph> ().graphScale -= 0.02f;
				if (dir.x > 0)
					GameObject.Find ("Graph").GetComponent<Graph> ().graphScale += 0.02f;
			}
			if (GameObject.Find ("hand_edge") != null)
				Destroy (GameObject.Find ("hand_edge"));
			GameObject e = createEdge ("hand_edge", GameObject.Find ("hand2").transform.position, hand);
			c.a = 0.4f;
			e.GetComponent<Renderer> ().material.color = c;
		} else {
			if (GameObject.Find ("hand_edge") != null)
				Destroy (GameObject.Find ("hand_edge"));
		}
			
		current = transform.position;

		
	}

	void OnTriggerEnter(Collider other) {

		if (r || scaling)
			return;

		if (other.gameObject.tag != "Node" && other.gameObject.tag != "R_Node")
			return;

		if (GameObject.FindWithTag ("Bubble") != null)
			Destroy (GameObject.FindWithTag ("Bubble"));
		print (other.gameObject.name);
		w_button.SetActive (true);
		selectedIngredient = other.gameObject.name;

		Text txt = GameObject.Find ("GraphSelectionLabel").GetComponent<Text>();
		string msg = "See results for ";
		if (other.gameObject.tag == "Node") {
			msg += "adding " + other.gameObject.name + " to the mix?";
		} else {
			msg += "subtracting " + other.gameObject.name + " from the mix?";
		}
		txt.text = msg;

		GameObject bubble = Instantiate<GameObject>(bubblePrefab.gameObject, Vector3.zero, Quaternion.identity);
		bubble.transform.position = other.gameObject.transform.position;
		bubble.transform.SetParent (other.gameObject.transform.parent);
		bubble.transform.localScale = other.gameObject.transform.localScale * 1.5f;

//		GetComponent<ro> ().setWayfindingObject (other.gameObject);

	}

	void wayfind() {
		if (wayfinding) {
			wayfinding = false;
			stopWayfinding ();
			return;
		} else {
			wayfinding = true;
		}
		string new_name = char.ToUpper(selectedIngredient[0]) + selectedIngredient.Substring(1);
		GetComponent<ro> ().setWayfindingObject (GameObject.Find(new_name));
		GameObject.Find ("w_text").GetComponent<Text> ().text = "stop wayfinding";
	}

	void stopWayfinding() {
		GameObject.Find ("w_text").GetComponent<Text> ().text = "take me there?";
//		selectedIngredient = "";
//		Text txt = GameObject.Find ("GraphSelectionLabel").GetComponent<Text>();
//		txt.text = "";
		GetComponent<ro> ().unsetWayfindingObject();
		if (!GameObject.Find ("Graph").GetComponent<GraphEventHandler> ().tracked)
			w_button.SetActive (false);
	}

	public void setHandPos() {

		if (r) {
			setRotateOff ();
			return;
		}
		if (scaling)
			setScalingOff ();

		rotateButton.gameObject.GetComponent<Image>().color = Color.red;

		r = true;
		manipHelper ();
	}

	public void setScalingOn() {
		if (scaling) {
			setScalingOff ();
			return;
		}
		if (r)
			setRotateOff ();
		scalingButton.gameObject.GetComponent<Image>().color = Color.red;
		scaling = true;
		manipHelper ();

	}

	private void manipHelper() {
		GameObject hand_obj = transform.GetChild (0).gameObject;
		Color c = hand_obj.GetComponent<Renderer> ().material.color;
		c.a = 0.5f;
		hand_obj.GetComponent<Renderer> ().material.color = c;

		GameObject node = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		node.name = "hand2";
		node.transform.SetParent (transform);
		node.transform.localPosition = new Vector3 (0f, 0f, 0.5f);
		Shader shader2 = Shader.Find("Transparent/Diffuse");
		node.GetComponent<Renderer> ().material.shader = shader2;
		Color col = node.GetComponent<Renderer>().material.color;
		col.a = 0.7f;
		node.GetComponent<Renderer> ().material.color = col;

		hand = node.transform.position;

		GameObject.Find ("hand_pointer").GetComponent<Renderer> ().enabled = false;
	}

	public void setRotateOff() {
		rotateButton.gameObject.GetComponent<Image>().color = Color.white;
		r = false;
		GameObject hand_obj = transform.GetChild (0).gameObject;
		Color c = hand_obj.GetComponent<Renderer> ().material.color;
		c.a = 1f;
		hand_obj.GetComponent<Renderer> ().material.color = c;

		Destroy (GameObject.Find ("hand2"));
		if (GetComponent<HandEventHandler>().tracked)
			GameObject.Find ("hand_pointer").GetComponent<Renderer> ().enabled = true;
	}

	public void setScalingOff() {
		scalingButton.gameObject.GetComponent<Image>().color = Color.white;
		scaling = false;
		GameObject hand_obj = transform.GetChild (0).gameObject;
		Color c = hand_obj.GetComponent<Renderer> ().material.color;
		c.a = 1f;
		hand_obj.GetComponent<Renderer> ().material.color = c;

		Destroy (GameObject.Find ("hand2"));
		if (GetComponent<HandEventHandler>().tracked)
			GameObject.Find ("hand_pointer").GetComponent<Renderer> ().enabled = true;
	}

	GameObject createEdge(string name, Vector3 point1, Vector3 point2) {
		GameObject edge = Instantiate<GameObject>(edgePrefab.gameObject, Vector3.zero, Quaternion.identity);
		edge.name = name;
		edge.transform.position = (point1 + point2) / 2f;
		Vector3 diff = point2 - point1;
		Vector3 scale = edge.transform.localScale;
		scale.y = diff.magnitude / 2f;
		edge.transform.localScale = scale;
		edge.transform.rotation = Quaternion.FromToRotation(Vector3.up, point2-point1);
		Shader shader2 = Shader.Find("Transparent/Diffuse");
		edge.GetComponent<Renderer> ().material.shader = shader2;
		return edge;
	}

}
