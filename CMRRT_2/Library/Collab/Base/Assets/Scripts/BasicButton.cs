using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class BasicButton : MonoBehaviour, IVirtualButtonEventHandler {

	public GameObject cube;

	// Use this for initialization
	void Start () {
		cube.gameObject.GetComponent<Renderer> ().material.color = Color.white;
		gameObject.GetComponent<VirtualButtonBehaviour> ().RegisterEventHandler (this);
		cube.gameObject.transform.localScale = new Vector3 (1, 0.05f, 1);
	}

	// Update is called once per frame
	void Update () {

	}

	public void OnButtonPressed (VirtualButtonBehaviour vb) {
		Debug.Log (this.name + "Button pressed");
		cube.gameObject.GetComponent<Renderer> ().material.color = Color.green;
		cube.gameObject.transform.localScale = new Vector3 (1, 0.025f, 1);

		int prevPage = GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number;
		int maxPageNumber = GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number;
		Debug.Log ("previous page is " + prevPage);
		if (this.name.Equals ("Previous")) {
			// go to previous page if not already on first page
			if (prevPage > 1) {
				GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number = prevPage + 1;
				GameObject.Find ("Workspace").GetComponent<SetRecords> ().fillRecords(prevPage + 1);
			}
		}

		if (this.name.Equals ("First")) {
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number = 1;
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().fillRecords(1);
		}

		if (this.name.Equals ("Previous")) {
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number = prevPage - 1;
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().fillRecords(prevPage - 1);
		}

		if (this.name.Equals ("Next")) {
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number = prevPage + 1;
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().fillRecords(prevPage + 1);
		}

		if (this.name.Equals ("Last")) {
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().page_number = maxPageNumber;
			GameObject.Find ("Workspace").GetComponent<SetRecords> ().fillRecords(maxPageNumber);
		}

		//		GameObject newObject = Instantiate (preFab, cursor.transform.position, cursor.transform.rotation);
		//
		//		newObject.transform.SetParent (WorldTarget.transform);
	}

	public void OnButtonReleased (VirtualButtonBehaviour vb) {
		Debug.Log ("Button released");
		cube.gameObject.GetComponent<Renderer> ().material.color = Color.white;
		cube.gameObject.transform.localScale = new Vector3 (1, 0.05f, 1);
	}
}
