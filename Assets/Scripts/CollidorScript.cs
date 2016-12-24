using UnityEngine;
using System.Collections;

public class CollidorScript : MonoBehaviour {

	public int side;
	public GemScript p;
	public GameObject handsup;
	// Use this for initialization
	void OnTriggerEnter (Collider o) {
//		p.addNeigbour(side,o);
		handsup = getParent(o.gameObject);
		print ("enter");
		
	}
	void OnTriggerExit (Collider o) {
		//		p.addNeigbour(side,o);
		handsup = null;
		print ("exit");
	}

	GameObject getParent(GameObject o ){
		return o.GetComponent<Transform> ().parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
