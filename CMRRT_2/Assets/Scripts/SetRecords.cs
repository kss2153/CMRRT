using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Linq;

public class SetRecords : MonoBehaviour {

	// get the records from workspace data script
	// private List <Experiment> records = GameObject.Find("Workspace").GetComponent<Data>().records;
	public List <Experiment> matchedRecords = new List<Experiment>();
	public int page_number;
	public int max_page_number;
	public GameObject record1;
	public GameObject record2;
	public GameObject record3;
	public GameObject record4;


	// Use this for initialization
	void Start () {
		page_number = 1;
		record1.SetActive(false);
		record2.SetActive(false);
		record3.SetActive(false);
		record4.SetActive(false);
		//fillRecords (1);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void getRecords() {
		//update the list of materials
		matchedRecords.Clear();
		foreach (Ingredient material in this.GetComponent<Workspace> ().rawIngredients){
			// if all materials on workspace is a subset of materials in a given experiment
			Debug.Log ("~~~~" + material.name + "is on the workspace");
		}

		foreach (Experiment record in this.GetComponent<Data> ().dict[this.GetComponent<Workspace> ().rawIngredients]){
			// if all materials on workspace is a subset of materials in a given experiment
			matchedRecords.Add (record);
			Debug.Log ("~~~~added " + record.description + " to matchedRecords");
		}
		max_page_number = getMaxPageNumber ();
	}

	public void fillRecords(int pageNumber){
		record1.SetActive(false);
		record2.SetActive(false);
		record3.SetActive(false);
		record4.SetActive(false);
		if (pageNumber >= 0 && pageNumber <= getMaxPageNumber ()) {
			// if the first record in the page exists
			if (matchedRecords.Count > (pageNumber * 4) - 4) {
				//Debug.Log ("~~~~found " + matchedRecords [(pageNumber * 4) - 4].activity);
				record1.SetActive(true);
				record1.transform.parent.gameObject.GetComponent<BasicButton> ().exp = matchedRecords [(pageNumber * 4) - 4];
				record1.GetComponent<MeshRenderer> ().enabled = true;
				record1.transform.Find ("Canvas").Find ("Record_title").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 4].title;
				record1.transform.Find ("Canvas").Find ("Record_date").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 4].semester;
				record1.transform.Find ("Canvas").Find ("Record_description").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 4].activity;
				record1.transform.Find ("Canvas").Find ("Image").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>(matchedRecords [(pageNumber * 4) - 4].experiment_id);
			}

			// if the second record in the page exists
			if (matchedRecords.Count > (pageNumber * 4) - 3) {
				record2.SetActive(true);
				record2.transform.parent.gameObject.GetComponent<BasicButton> ().exp = matchedRecords [(pageNumber * 4) - 3];
				record2.GetComponent<MeshRenderer> ().enabled = true;
				record2.transform.Find ("Canvas").Find ("Record_title").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 3].title;
				record2.transform.Find ("Canvas").Find ("Record_date").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 3].semester;
				record2.transform.Find ("Canvas").Find ("Record_description").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 3].activity;
				record2.transform.Find ("Canvas").Find ("Image").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>(matchedRecords [(pageNumber * 4) - 3].experiment_id);
			}

			// if the third record in the page exists
			if (matchedRecords.Count > (pageNumber * 4) - 2) {
				record3.SetActive(true);
				record3.transform.parent.gameObject.GetComponent<BasicButton> ().exp = matchedRecords [(pageNumber * 4) - 2];
				record3.GetComponent<MeshRenderer> ().enabled = true;
				record3.transform.Find ("Canvas").Find ("Record_title").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 2].title;
				record3.transform.Find ("Canvas").Find ("Record_date").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 2].semester;
				record3.transform.Find ("Canvas").Find ("Record_description").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 2].activity;
				record3.transform.Find ("Canvas").Find ("Image").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>(matchedRecords [(pageNumber * 4) - 2].experiment_id);
			}

			// if the fourth record in the page exists
			if (matchedRecords.Count > (pageNumber * 4) - 1) {
				record4.SetActive(true);
				record4.transform.parent.gameObject.GetComponent<BasicButton> ().exp = matchedRecords [(pageNumber * 4) - 1];
				record4.GetComponent<MeshRenderer> ().enabled = true;
				record4.transform.Find ("Canvas").Find ("Record_title").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 1].title;
				record4.transform.Find ("Canvas").Find ("Record_date").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 1].semester;
				record4.transform.Find ("Canvas").Find ("Record_description").GetComponent<Text> ().text = matchedRecords [(pageNumber * 4) - 1].activity;
				record4.transform.Find ("Canvas").Find ("Image").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>(matchedRecords [(pageNumber * 4) - 1].experiment_id);
			}
		} else {
			throw new System.ArgumentException("Invalid page number, i.e. page number is not between 0 to max page number");
		}
	}

	public int getMaxPageNumber() {
		if (matchedRecords.Count % 4 == 0) {
			return (matchedRecords.Count / 4);
		} else {
			return (int) Mathf.Floor (matchedRecords.Count / 4) + 1;
		}
	}
}
