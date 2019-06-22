using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class Workspace : MonoBehaviour {

	private Ingredient beeswax = new Ingredient("beeswax", "Beeswax is a natural wax produced by honey bees of the genus Apis.", new HSB(51,12,96), "https://stakich.com/collections/beeswax/products/white-beeswax-pellets", false);
//	private Ingredient sulfur = new Ingredient("sulfur", "Sulfur is a chemical element with symbol S and atomic number 16. It is abundant, multivalent, and nonmetallic.", new HSB(53,47,85), "https://upload.wikimedia.org/wikipedia/commons/4/44/Sulfur-sample.jpg");
	public HashSet<Ingredient> rawIngredients = new HashSet<Ingredient>();

	// Use this for initialization
	void Start () {
//		addIngredient (beeswax);
		updateDetectedIngredientsDisplay ();
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
