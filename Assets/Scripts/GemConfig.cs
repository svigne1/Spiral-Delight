using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemConfig : MonoBehaviour {
	public LayerScript l;
	public GameObject collidor;
	public Dictionary<string, CollidorScript> collidors;
	public int i;
	public bool Destroyed;

	// Taken from this answer http://answers.unity3d.com/answers/177525/view.html

	public string[] collidorNames = new string[]{"outside","inside","clock","anti"};
	public string color;

	public void PlaceCollidors(){
		foreach (Transform child in transform) {
			child.GetComponent<CollidorScript> ().PlaceCollidor ();
		}
	}
	public void AddCollidors(){

		GameObject[] c = new GameObject[4];
		for (int k = 0; k < 4; k++) {
			c[k] = (GameObject)Instantiate(collidor, new Vector3(0, 0, 0), Quaternion.identity);
			c [k].name = collidorNames[k];
			c [k].transform.parent = transform;
			c [k].GetComponent<CollidorScript> ().g = GetComponent<GemLogic>();
			collidors.Add (collidorNames[k],c[k].GetComponent<CollidorScript>());
		}
	}

}
