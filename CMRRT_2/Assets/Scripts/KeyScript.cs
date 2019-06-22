using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		initKey ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void initKey() {
		int i = 1;
		foreach (KeyValuePair<string,Color> pair in GameObject.Find("Workspace").GetComponent<Data>().cleanedActivityToColor) {
			string name = "Color" + i;
			Text txt = GameObject.Find (name).GetComponent<Text> ();
			string color = GameObject.Find("Workspace").GetComponent<Data>().colorNames[pair.Value];
			txt.text = "<color=" + color + ">" + pair.Key + "</color>";
			i++;
		}
	}
}
