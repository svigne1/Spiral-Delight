using UnityEngine;
using System.Collections;

public class CollidorScript : MonoBehaviour {

	public int side;
	public GemScript p;
	public GameObject handsup;

	void OnTriggerEnter (Collider o) {
		// Ignoring the collidor on Parent and other parts.
		if(o.tag == "Collidor")
			handsup = getParent(o.gameObject);
		
	}
	void OnTriggerExit (Collider o) {
		handsup = null;
//		print ("exit");
	}

	GameObject getParent(GameObject o ){
		return o.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
