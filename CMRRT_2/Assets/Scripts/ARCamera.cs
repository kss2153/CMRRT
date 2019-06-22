using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCamera : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnPostRender() {
//		if (GameObject.Find("Graph").GetComponent<GraphEventHandler>().tracked)
//			DrawConnectingLines ();

	}

	void DrawConnectingLines() {

		GameObject[] g_nodes = GameObject.FindGameObjectsWithTag ("Node");
		GameObject[] r_nodes = GameObject.FindGameObjectsWithTag ("R_Node");
		GameObject center = GameObject.Find ("center_node");
		if(center) {
			// Loop through each point to connect to the mainPoint
			foreach(GameObject node in g_nodes) {
				print ("line");
				Vector3 mainPointPos = center.transform.position;
				Vector3 pointPos = node.transform.position;

				GL.Begin(GL.LINES);
//				lineMat.SetPass(0);
				GL.Color(Color.green);
				GL.Vertex3(mainPointPos.x, mainPointPos.y, mainPointPos.z);
				GL.Vertex3(pointPos.x, pointPos.y, pointPos.z);
				GL.End();
			}
		}
	}
}
