using UnityEngine;
using System.Collections;

public class CollidorScript : MonoBehaviour {
	
	public GemScript p;
	public BoardScript b;
	public GemScript handsup;


	void OnTriggerStay (Collider o) {
		// To ignore parent, layer object and other collidors.
		if (b.BoardLock == 1) {
			handsup = null;
		} else {
			if (o.tag == "Gem" && o.name != p.name) {
				handsup = o.GetComponent<GemScript>();
			}
		}
	}
	public void PlaceCollidor(){
		transform.position = new Vector3(0, 0, 0);
		float standard = -p.l.gemOrigin - p.l.layer * p.l.gemLength;
		switch (name)
		{
		case "clock":
			transform.Translate (new Vector3 (standard-p.l.gemLength / 2, 0, 0));
			break;
		case "outside":
			transform.Translate (new Vector3(standard-p.l.gemLength,-0.01f,0));
			break;
		case "anti":
			float x_tan = (p.l.gemOrigin + p.l.layer * p.l.gemLength + p.l.gemLength / 2);
			float tan = Mathf.Tan (p.l.gemRadians);
			float y_tan = tan * x_tan;
			transform.Translate (new Vector3(standard-p.l.gemLength/2,-y_tan,0));
			break;
		case "inside":
			transform.Translate (new Vector3(standard,-0.01f,0));
			break;

		}
	}
}
