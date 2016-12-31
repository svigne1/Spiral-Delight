using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollidorScript : MonoBehaviour {
	
	public GemScript g;
	public List<GemScript> handsup;

	public bool FreeFall;

	public void FreeFallController(string s){
		if (s == "Start") {
			if (!FreeFall) {
				FreeFall = true;
				g.l.b.Equilibrium++;
			}
		} 
		if(s == "Stop") {
			if (FreeFall) {
				FreeFall = false;
				g.l.b.Equilibrium--;
			}
		}
	}

	void Start(){
		if (name == "inside") {
			FreeFall = false;
			StartCoroutine (Gravity ());
		}
	}
	void OnTriggerEnter (Collider o) {
		if (o.tag == "Gem" && o.name != g.name) {
			AddToHandsUp(o.GetComponent<GemScript>());
		}
	}
	void OnTriggerExit (Collider o) {
		if (o.tag == "Gem") {
			RemoveFromHandsUp (o.GetComponent<GemScript> ());
		}
	}
	public IEnumerator Gravity(){
		while (true) {
			if (g.l.layer != 0 && !g.l.b.FreezeGravity && handsup.Count == 0) {
				FreeFallController ("Start");
				g.FallDown ();
			} else {
				FreeFallController ("Stop");
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void AddToHandsUp(GemScript g){
		handsup.Add (g);
	}
	public void RemoveFromHandsUp(GemScript g){
		handsup.Remove (g);
	}
	public void PlaceCollidor(){
		transform.position = new Vector3(0, 0, 0);
		float standard = -g.l.gemOrigin - g.l.layer * g.l.gemLength;
		float x_tan = 0f, y_tan = 0f;
		switch (name)
		{
		case "clock":
			transform.Translate (new Vector3 (standard-g.l.gemLength / 2, 0, 0));
			break;
		case "outside":
			x_tan = -standard + g.l.gemLength;
			y_tan = g.l.tanGemDegrees * x_tan;
			transform.Translate (new Vector3(standard-g.l.gemLength +0.01f,-y_tan/2,0));
			break;
		case "anti":
			x_tan = (-standard + g.l.gemLength / 2);
			y_tan = g.l.tanGemDegrees * x_tan;
			transform.Translate (new Vector3(standard-g.l.gemLength/2,-y_tan,0));
			break;
		case "inside":
			x_tan = -standard;
			y_tan = g.l.tanGemDegrees * x_tan;
			transform.Translate (new Vector3(standard +0.01f,-y_tan/2,0));
			break;

		}
	}
}
