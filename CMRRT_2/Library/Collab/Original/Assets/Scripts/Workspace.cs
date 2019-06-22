using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class Workspace : MonoBehaviour {

	Ingredient beeswax = new Ingredient ("beeswax", "Beeswax is a natural wax produced by honey bees of the genus Apis.", new HSB (51, 12, 96), "https://stakich.com/collections/beeswax/products/white-beeswax-pellets", false);
	Ingredient sulfur = new Ingredient ("sulfur", "Sulfur is a chemical element with symbol S and atomic number 16. It is abundant, multivalent, and nonmetallic.", new HSB (53, 47, 85), "https://upload.wikimedia.org/wikipedia/commons/4/44/Sulfur-sample.jpg", false);
	Ingredient rosin = new Ingredient ("rosin", "Rosin, also called colophony or Greek pitch, is a solid form of resin obtained from pines and some other plants, mostly conifers, produced by heating fresh liquid resin to vaporize the volatile liquid terpene components.", new HSB (51, 12, 96), "https://www.leafly.com/news/cannabis-101/what-is-rosin", false);
	Ingredient vermillion = new Ingredient ("vermillion", "Vermilion is both a brilliant red or scarlet pigment originally made from the powdered mineral cinnabar and the name of the resulting color.", new HSB (1, 81, 99), "https://upload.wikimedia.org/wikipedia/commons/b/ba/Red_tikka_powder.jpg", false);
	Ingredient azurite = new Ingredient ("azurite", "Azurite is a soft, deep blue copper mineral produced by weathering of copper ore deposits. It is also known as Chessylite after the type locality at Chessy-les-Mines near Lyon, France.", new HSB (226, 98, 82), "https://www.jewelsforme.com/images/articles/azurite-pigment-history-meaning.jpg", false);
	Ingredient lead = new Ingredient ("lead", "Lead is a chemical element with symbol Pb and atomic number 82.", new HSB (210, 4, 20), "http://www.thesleuthjournal.com/wp-content/uploads/2017/09/lead.jpg", false);

//	private Ingredient sulfur = new Ingredient("sulfur", "Sulfur is a chemical element with symbol S and atomic number 16. It is abundant, multivalent, and nonmetallic.", new HSB(53,47,85), "https://upload.wikimedia.org/wikipedia/commons/4/44/Sulfur-sample.jpg");
	public HashSet<Ingredient> rawIngredients = new HashSet<Ingredient>();

	public Button vb_on;
	public bool vb = false;
	public bool buttonCurrentlyPressed = false;

	// Use this for initialization
	void Start () {
		addIngredient (sulfur);
		addIngredient (lead);
		addIngredient (vermillion);
		updateDetectedIngredientsDisplay ();
		vb_on.onClick.AddListener (vbClick);
	}

	public void vbClick() {
		if (vb) {
			vb = false;
			vb_on.gameObject.GetComponent<Image> ().color = Color.white;
		} else {
			vb = true;
			vb_on.gameObject.GetComponent<Image> ().color = Color.red;
		}
	}
	
	// Update is called once per frame
	void Update () {
		printIngredients ();
		updateDetectedIngredientsDisplay ();
//		string temp = "detected ingredients:";
//		foreach (string name in getIngredientNames ()) {
//			temp += " " + name;
//		}
//		Debug.Log (temp);
	}

	// return an array containing the names all known ingredients
	public string[] getIngredientNames() {
		List<string> output = new List<string> ();
		foreach (Ingredient i in rawIngredients) {
			output.Add (i.name);
		}
		return output.ToArray ();
	}

	public void addIngredient(Ingredient ingredient) {
		if (!rawIngredients.Contains (ingredient)) {
			rawIngredients.Add (ingredient);

			GameObject graph = GameObject.Find ("Graph");
			if (graph != null) {
				graph.GetComponent<Graph> ().updateGraph ();
			}
			transform.GetComponent<SetRecords>().getRecords();
			transform.GetComponent<SetRecords>().fillRecords(1);
		}
	}

	public void removeIngredient(Ingredient ingredient) {
		if (rawIngredients.Contains (ingredient)) {
			rawIngredients.Remove (ingredient);
			GameObject graph = GameObject.Find ("Graph");
			if (graph != null) {
				graph.GetComponent<Graph> ().updateGraph ();
			}
		}
	}

	public void updateDetectedIngredientsDisplay() {
		string[] ingredientsArray = getIngredientNames ();
		string outputText = "";

		if (ingredientsArray.Length > 0) {
			outputText = "Current mix:";

			foreach (string ingredient in ingredientsArray) {
				outputText += " " + ingredient;
			}

			printIngredients ();

		} else {
			outputText = "No ingredients detected";
		}

		transform.Find ("Workspace plane").Find("Ingredients list").Find("Canvas").Find("Text").GetComponent<Text> ().text = outputText;
	}

	public void printIngredients() {
		string temp = "ingredients:";
		foreach (string ingredient in getIngredientNames ()) {
			temp += " " + ingredient;
		}
		print (temp);
	}
}
