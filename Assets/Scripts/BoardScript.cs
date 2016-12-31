using UnityEngine;
using System.Collections;

public class BoardScript : MonoBehaviour {


	public GameObject Layer;
	public bool FreezeGravity;
	public int Equilibrium;

	void Start () {
		LayerScript inner = null;
		for (int l = 0; l <Layer.GetComponent<LayerScript>().mesh.Length; l++) {
			LayerScript newLayer = AddLayer(l);
			if (inner != null)
				inner.outer = newLayer;
			newLayer.inner = inner;
			inner = newLayer;
		}
		FreezeGravity = false;
		Equilibrium = 0;
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
