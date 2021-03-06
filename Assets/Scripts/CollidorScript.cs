﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollidorScript : MonoBehaviour {

	public GemLogic g;
	public List<GemLogic> handsup;

	public bool FreeFall;

	public void FreeFallController(string s){
		if (s == "Start") {
			if (!FreeFall) {
				g.c.l.b.AddToChangeList (g);
				FreeFall = true;
				g.c.l.b.Equilibrium++;
			}
		}
		if(s == "Stop") {
			if (FreeFall) {
				FreeFall = false;
				g.c.l.b.Equilibrium--;
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
			AddToHandsUp(o.GetComponent<GemLogic>());
		}
	}
	void OnTriggerExit (Collider o) {
		if (o.tag == "Gem") {
			RemoveFromHandsUp (o.GetComponent<GemLogic> ());
		}
	}
	public IEnumerator Gravity(){
		while (g.c.l.layer != 0 && g.c.Destroyed == false) {
			if (g.c.l.b.Gravity == true) {
				if (handsup.Count == 0) {
					FreeFallController ("Start");
					g.gy.FallDown ();
				} else {
					FreeFallController ("Stop");
				}
			}
			yield return new WaitForFixedUpdate();
		}
		// when it falls to layer 0, equilibrium value needs to be reduced as it quits out of the loop.
		FreeFallController ("Stop");
	}

	public void AddToHandsUp(GemLogic g){
		handsup.Add (g);
	}
	public void RemoveFromHandsUp(GemLogic g){
		handsup.Remove (g);
	}
	public void PlaceCollidor(){
		transform.position = new Vector3(0, 0, 0);
		float standard = -g.c.l.gemOrigin - g.c.l.layer * g.c.l.gemLength;
		float x_tan = 0f, y_tan = 0f;
		switch (name)
		{
		case "clock":
			transform.Translate (new Vector3 (standard-g.c.l.gemLength / 2, 0, 0));
			break;
		case "outside":
			x_tan = -standard + g.c.l.gemLength;
			y_tan = g.c.l.tanGemDegrees * x_tan;
			transform.Translate (new Vector3(standard-g.c.l.gemLength +0.01f,-y_tan/2,0));
			break;
		case "anti":
			x_tan = (-standard + g.c.l.gemLength / 2);
			y_tan = g.c.l.tanGemDegrees * x_tan;
			transform.Translate (new Vector3(standard-g.c.l.gemLength/2,-y_tan,0));
			break;
		case "inside":
			x_tan = -standard;
			y_tan = g.c.l.tanGemDegrees * x_tan;
			transform.Translate (new Vector3(standard +0.01f,-y_tan/2,0));
			break;

		}
	}
}
