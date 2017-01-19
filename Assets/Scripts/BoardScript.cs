using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BoardScript : MonoBehaviour {


	public GameObject Layer;
	public bool Gravity;
	public int Equilibrium;
	public static List<GemLogic> changeList;
	public bool FirstRunSinceGravity;

	public void AddToChangeList(GemLogic a){
		if (!changeList.Contains (a))
			changeList.Add (a);
	}

	void Start () {
		
		LayerScript inner = null;
		for (int l = 0; l <Layer.GetComponent<LayerScript>().mesh.Length; l++) {
			LayerScript newLayer = AddLayer(l);
			if (inner != null)
				inner.outer = newLayer;
			newLayer.inner = inner;
			inner = newLayer;
		}
		Gravity = true;
		changeList = new List<GemLogic> ();
		FirstRunSinceGravity = false;
		Equilibrium = 0;
		StartCoroutine (BoardValidator ());
	}

	public IEnumerator BoardValidator(){
		while (true) {
			if (Gravity == true) {
				if(FirstRunSinceGravity == true){
					FirstRunSinceGravity = false;
					yield return new WaitForFixedUpdate();	
					yield return new WaitForFixedUpdate();	
				} else {
					if (changeList.Count!=0 && Equilibrium == 0) {
						while (changeList.Count != 0) {
							Collection s = new Collection ();
							s.FormCollection (changeList [0]);
							print ("----------------------From Here");
							s.Print ();
//							changeList = new List<GemLogic> ();
						}
						BroadcastMessage ("ResetProcess");
						yield return new WaitForFixedUpdate();	
						yield return new WaitForFixedUpdate();	
					}
				}
			} else {
				FirstRunSinceGravity = true;
			}
			yield return new WaitForFixedUpdate();	
		}
	}

	public class Collection{

		public Dictionary<string, List<GemGroup>> answer;

		public void Print(){
			print ("radius = "+answer["radius"].Count);
			print ("circumference = "+answer["circumference"].Count);
//			print ("---------radius--------");
//			foreach (GemGroup s in answer["radius"]) {
//				print ("------");
//				s.Print ();
//			}
//			print ("---------circumference----------");
//			foreach (GemGroup s in answer["circumference"]) {
//				print ("------");
//				s.Print ();
//			}
		}

		public Collection(){
			answer = new Dictionary<string, List<GemGroup>>();
			answer["radius"] = new List<GemGroup>();
			answer["circumference"] = new List<GemGroup>();
		}
		public void AddToGroup(GemGroup s) {
			answer[s.direction].Add(s);
		}
		public void FormCollection(GemLogic inputGem) {
			if (changeList.Contains (inputGem)) 
				changeList.Remove (inputGem);

			List<string> processList = new List<string> ();
			if (!inputGem.process ["radius"])
				processList.Add ("radius");
			if (!inputGem.process ["circumference"])
				processList.Add ("circumference");

			foreach (string i in processList) {
				GemGroup r = inputGem.GemGroup (i);
				if (r != null) {
					AddToGroup (r);
					foreach (GemLogic s in r.negative) {
						r.SetReferenceFor (s);
					}
					foreach (GemLogic s in r.positive) {
						r.SetReferenceFor (s);
					}
					foreach (GemLogic s in r.negative) {
						FormCollection (s);
					}
					foreach (GemLogic s in r.positive) {
						FormCollection (s);
					}
				}
			}
		}

	}

	LayerScript AddLayer(int l){
		LayerScript newLayer = ((GameObject)Instantiate(Layer, new Vector3(0, 0, 0), Quaternion.identity)).GetComponent<LayerScript>();
		newLayer.name = "Layer"+l;
		newLayer.layer = l;
		newLayer.b = this;
		newLayer.transform.parent = transform;
		newLayer.tanGemDegrees = Mathf.Tan (newLayer.gemRadians);

		// Add Gems to the Layer.
		int ts;
		if (l == 0 )
			ts = 32;
		else if (l == 1)
			ts = 32;
		else
			ts = 32;
		for (int i = 0; i < ts; i++) {
			newLayer.AddGem (i);
		}
		return newLayer;
	}

}
