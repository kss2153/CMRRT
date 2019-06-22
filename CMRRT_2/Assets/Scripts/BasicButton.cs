using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using AssemblyCSharp;

public class BasicButton : MonoBehaviour, IVirtualButtonEventHandler {

	public GameObject cube;
	public GameObject overlay;
	public Button exit_overlay;

	private float timer = 3f;
	private bool pressingReport = false;

	public Experiment exp = null;
	public string desc = "";

	// Use this for initialization
	void Start () {
		cube.gameObject.GetComponent<Renderer> ().material.color = Color.white;
		gameObject.GetComponent<VirtualButtonBehaviour> ().RegisterEventHandler (this);
		cube.gameObject.transform.localScale = new Vector3 (1, 0.05f, 1);

		exit_overlay.onClick.AddListener (exitClick);
	}

	// Update is called once per frame
	void Update () {
		if (pressingReport) {
			timer -= Time.deltaTime;
		}
		if (timer < 0) {
			GameObject.Find ("Workspace").GetComponent<Workspace> ().vbClick ();
			overlay.SetActive (true);
			Text title = GameObject.Find ("Overlay_title").GetComponent<Text> ();
			title.text = this.gameObject.transform.GetChild (0).Find ("Canvas").Find ("Record_title").GetComponent<Text> ().text;
			Text date = GameObject.Find ("Overlay_date").GetComponent<Text> ();
			date.text = this.gameObject.transform.GetChild (0).Find ("Canvas").Find ("Record_date").GetComponent<Text> ().text;
			Text description = GameObject.Find ("Overlay_description").GetComponent<Text> ();
			if (exp == null)
				description.text = this.gameObject.transform.GetChild (0).Find ("Canvas").Find ("Record_description").GetComponent<Text> ().text;
			else
				description.text = exp.description;
			UnityEngine.UI.Image image = GameObject.Find ("Overlay_image").GetComponent<UnityEngine.UI.Image> ();   
			image.sprite = this.gameObject.transform.GetChild (0).Find ("Canvas").Find ("Image").GetComponent<SpriteRenderer> ().sprite;

			pressingReport = false;
			timer = 3f;
		}


	}

	public void OnButtonPressed (VirtualButtonBehaviour vb) {
		if (!GameObject.Find ("Workspace").GetComponent<Workspace> ().vb)
			return;

		if (GameObject.Find ("Workspace").GetComponent<Workspace> ().buttonCurrentlyPressed)
			return;

		GameObject.Find ("Workspace").GetComponent<Workspace> ().buttonCurrentlyPressed = true;

		cube.gameObject.GetComponent<Renderer> ().material.color = Color.green;
		cube.gameObject.transform.localScale = new Vector3 (1, 0.025f, 1);

		int prevPage = GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number;
		int maxPageNumber = GameObject.Find ("Workspace").GetComponent<SetRecords> ().max_page_number;
		Debug.Log (this.name + "Button pressed; previous page is " + prevPage + " and maxPageNumber = " + maxPageNumber);
		if (this.name.Equals ("Previous")) {
			// go to previous page if not already on first page
			if (prevPage > 1) {
				GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number = prevPage - 1;
				GameObject.Find ("Workspace").GetComponent<SetRecords> ().fillRecords(prevPage - 1);
			}
		}

		if (this.name.Equals ("First")) {
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number = 1;
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().fillRecords(1);
		}
			
		if (this.name.Equals ("Next")) {
			if (prevPage < maxPageNumber) {
				GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number = prevPage + 1;
				GameObject.Find ("Workspace").GetComponent<SetRecords> ().fillRecords(prevPage + 1);
			}
		}

		if (this.name.Equals ("Last")) {
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number = maxPageNumber;
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().fillRecords(maxPageNumber);
		}

		if (this.name.Contains("Report")) {
			pressingReport = true;
		}

		//		GameObject newObject = Instantiate (preFab, cursor.transform.position, cursor.transform.rotation);
		//
		//		newObject.transform.SetParent (WorldTarget.transform);
	}

	public void OnButtonReleased (VirtualButtonBehaviour vb) {
		GameObject.Find ("Workspace").GetComponent<Workspace> ().buttonCurrentlyPressed = false;
		pressingReport = false;
		timer = 3f;
		Debug.Log ("Button released");
		cube.gameObject.GetComponent<Renderer> ().material.color = Color.white;
		cube.gameObject.transform.localScale = new Vector3 (1, 0.05f, 1);
	}

	void exitClick() {
		overlay.SetActive (false);
	}
}
