using UnityEngine;
using System.Collections;

public class CollidorScript : MonoBehaviour {

	public int side;
	public GemScript p;
	public BoardScript b;
	public GameObject handsup;


	void OnTriggerStay (Collider o) {
		// To ignore parent, layer object and other collidors.
		if (b.BoardLock == 1) {
			handsup = null;
		} else {
			if (o.tag == "Gem" && o.name != p.name) {
				handsup = o.gameObject;
			}
		}
	}
//	void OnTriggerEnter(Collider o){
//		waities = 1;
//	}
//	void OnTriggerExit(Collider o){
//		waities = 1;
//	}
}
