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
				} else {
					if (changeList.Count!=0 && Equilibrium == 0) {
						List<Collection> all = new List<Collection> ();
						while (changeList.Count != 0) {
							Collection s = new Collection ();
							s.FormCollection (changeList [0]); 
							all.Add (s);
						}
						foreach (Collection item in all){
							item.DestroyLogicOne ();
						}
						BroadcastMessage ("ResetProcess");
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
			print ("---------radius--------");
			foreach (GemGroup s in answer["radius"]) {
				print ("------");
				s.Print ();
			}
			print ("---------circumference----------");
			foreach (GemGroup s in answer["circumference"]) {
				print ("------");
				s.Print ();
			}
		}

		public void DestroyLogicOne(){
			Dictionary<string, int> count = new Dictionary<string,int>();
			count ["circumference"] = 0;
			count ["radius"] = 0;
			
			foreach (GemGroup s in answer["circumference"]) {
				count["circumference"]+=s.count;
			}
			foreach (GemGroup s in answer["radius"]) {
				count["radius"]+=s.count;
			}
			if (count ["radius"] >= count ["circumference"]) {
				HelpDestroyGroup (answer["radius"]);
			} else {
				HelpDestroyGroup (answer["circumference"]);
			}
				
		}
		public void HelpDestroyGroup(List<GemGroup> a){
			foreach (GemGroup s in a) {
				s.selfDestruct ();
			}
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

			List<GemGroup> temp = new List<GemGroup> ();
			foreach (string i in processList) {
				GemGroup r = inputGem.getGemGroup (i);
				if (r != null) {
					temp.Add (r);
					AddToGroup (r);
					foreach (GemLogic s in r.left) {
						r.SetReferenceFor (s);
					}
					foreach (GemLogic s in r.right) {
						r.SetReferenceFor (s);
					}
				}
			}
			foreach (GemGroup j in temp) {
				foreach (GemLogic s in j.left) {
					FormCollection (s);
				}
				foreach (GemLogic s in j.right) {
					FormCollection (s);
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
