using UnityEngine;
using System.Collections;

public class BoardScript : MonoBehaviour {


	public GameObject Layer;
	public int BoardLock = 1;

	void Start () {
		LayerScript inner = null;
		for (int l = 0; l < Layer.GetComponent<LayerScript>().mesh.Length; l++) {
			LayerScript newLayer = AddLayer(l);
			if (inner != null)
				inner.outer = newLayer;
			newLayer.inner = inner;
			inner = newLayer;
		}
		BoardLock = 0;
	}

	LayerScript AddLayer(int l){
		LayerScript newLayer = ((GameObject)Instantiate(Layer, new Vector3(0, 0, 0), Quaternion.identity)).GetComponent<LayerScript>();
		newLayer.name = "Layer"+l;
		newLayer.layer = l;
		newLayer.b = this;
		newLayer.transform.parent = transform;

		// Add Gems to the Layer.
		int ts = 32;
		for (int i = 0; i < ts; i++) {
			newLayer.AddGem (i);
		}
		return newLayer;
	}

}
