using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemGravity : MonoBehaviour {

	public GemConfig c;

	public void FallDown()
	{
		ChangeTo (c.l.inner);
		c.PlaceCollidors ();
	}
	public void ChangeTo(LayerScript tolayer)
	{
		if (tolayer != null) {
			c.l = tolayer;
			// For Debugging .. i'm not changing name as it changes layers.
			if(name=="Gem(Clone)")name = "L" + c.l.layer + "N" + c.l.transform.childCount;
			GetComponent<MeshFilter>().mesh = tolayer.mesh[tolayer.layer];
			GetComponent<MeshCollider>().sharedMesh = tolayer.mesh[tolayer.layer];
			transform.parent = tolayer.transform;
		}
	}
}
