using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {


	public GameObject Layer;
	public bool Gravity;
	public int Equilibrium;
	public List<GemScript> changeList;
	public bool FirstRunSinceGravity;

	public void AddToChangeList(GemScript a){
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
		changeList = new List<GemScript> ();
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
							List<GemScript> temp = new List<GemScript> ();
							changeList [0].ugfv = false;
							changeList [0].ugfh = false;
							temp.Add (changeList [0]);
							FormCollection (changeList [0], temp);
							if (temp.Count != 1) {
								foreach (GemScript i in temp) {
									print (i.name);
								}
								print ("-----Group OVER----");
								FormGroup (temp);
							}
						}
						changeList =  new List<GemScript> ();
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

	List<GemScript> FormCollection(GemScript inputGem,List<GemScript> result){

		if (changeList.Contains (inputGem)) {
			changeList.Remove (inputGem);
		}

		int lastIndex = result.Count;
		List<GemScript> radius = inputGem.GemChain ("radius",new List<GemScript>());
		List<GemScript> circumference = inputGem.GemChain ("circumference",new List<GemScript>());
		if(radius != null){
			radius.Remove(inputGem);
			foreach (GemScript i in radius) {
				if (!result.Contains (i)) {
					i.ugfv = false;
					i.ugfh = false;
					result.Add (i);
				}
			}
		}
		if(circumference != null){
			circumference.Remove(inputGem);
			foreach (GemScript i in circumference) {
				if (!result.Contains (i)) {
					i.ugfv = false;
					i.ugfh = false;
					result.Add (i);
				}
			}
		}
		if (lastIndex != result.Count) {
			for(int j=lastIndex; j<result.Count;j++){
				FormCollection (result [j], result);
			}
		}
		
		return result;
	}
	void FormGroup(List<GemScript> group){
		List<List<GemScript>> uniqueGroups = new List<List<GemScript>>();

		foreach (GemScript j in group) {
			if (!j.ugfv) {
				List<GemScript> radius = j.GemChain ("radius",new List<GemScript>());
				if (radius != null) {
					uniqueGroups.Add (radius);
					foreach (GemScript i in radius) {
						i.ugfv = true;	
					}
				}

			}
			if (!j.ugfh) {
				List<GemScript> circumference = j.GemChain ("circumference",new List<GemScript>());
				if (circumference != null) {
					uniqueGroups.Add (circumference);
					foreach (GemScript i in circumference) {
						i.ugfh = true;	
					}
				}
			}
		}
		foreach (List<GemScript> k in uniqueGroups) {
			foreach (GemScript l in k) {
				print (l.name);
			}
			print ("new group");
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
