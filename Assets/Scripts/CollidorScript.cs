using UnityEngine;
using System.Collections;

public class CollidorScript : MonoBehaviour {

	public int side;
	public GemScript p;
	public GameObject handsup;

//	void OnTriggerEnter (Collider o) {
//		// Ignoring parent,layer and other collidors.
//		if (o.tag == "Gem" && o.name != transform.parent.name) {
//			if(side==1)print (name+"enter");
//			handsup = o.gameObject;
//		}
//		
//	}
//	void OnTriggerExit (Collider o) {
//		handsup = null;
//		print (name+"exit");
//	}
	void OnTriggerStay (Collider o) {
		if (o.tag == "Gem" && o.name != transform.parent.name) {
//			if(side==1)print (name+"enter");
			handsup = o.gameObject;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
