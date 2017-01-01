using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {


	public GameObject Layer;
	public bool Gravity;
	public int Equilibrium;
	public List<GemScript> changeList;
	public LayerScript InputLayer;
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
						if (InputLayer != null) {
							InputLayer.transform.BroadcastMessage ("ValidateRadius");

//							changeList[0].ValidateRadius();
							InputLayer = null;
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
//	void FixedUpdate(){
//		
//	}
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
