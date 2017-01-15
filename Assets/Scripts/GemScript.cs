using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemScript : MonoBehaviour {
	public LayerScript l;
	public GameObject collidor;
	public Dictionary<string, CollidorScript> collidors;
	public bool Destroyed;
	public int i;

	// unique group formation vertical.
	public bool ugfv;
	// unique group formation horizontal.
	public bool ugfh;

	// Taken from this answer http://answers.unity3d.com/answers/177525/view.html
	public float sensitivity;
	private Vector3 mouseReference;
	private Vector3 rotation;
	public string[] collidorNames = new string[]{"outside","inside","clock","anti"};
	private bool isRotating,onMouseRelease;
	public Vector3 planeCenter;
	public float gemDegrees;
	public string color;

	void Start ()
	{
		sensitivity = 1.0f;
		rotation = Vector3.zero;
		Destroyed = false;
		planeCenter = Camera.main.ViewportToWorldPoint (new Vector3(0.5f,0.5f,Camera.main.nearClipPlane));
	}
	void OnMouseDown()
	{
		if (l.b.Equilibrium == 0) {
//			foreach (Transform child in l.transform) {
//				l.b.AddToChangeList (child.GetComponent<GemScript>());
//			}
			l.b.AddToChangeList (this);
			l.b.Gravity = false;
			isRotating = true;
			mouseReference = mouseInPlanePoint ();
		}
	}

	void FixedUpdate()
	{
		if (onMouseRelease) {
			onMouseRelease = false;
			isRotating = false;
			RoundOff (transform.parent);
			l.b.Gravity = true;
		}
		if(isRotating)
		{
			Vector3 newReference = mouseInPlanePoint ();
			float finalAngle = Vector3.Angle( mouseReference, newReference);
			if (Vector3.Cross (mouseReference, newReference).z < 0)
				finalAngle *= -1;

			rotation.z = finalAngle * sensitivity;
			transform.parent.Rotate(rotation);
			mouseReference = newReference;
		}

	}

	void OnMouseUp()
	{
		onMouseRelease = true;
	}
	public void StartValidator(){
		l.transform.BroadcastMessage ("Validate");
	}
	public void Validate(string direction){
		if (Destroyed == false) {
			List<GemScript> chain = new List<GemScript> ();
			GemChain (direction,chain);

			if (chain!=null) {

				foreach (GemScript i in chain) {
					i.Destroyed = true;
					i.transform.Translate (new Vector3(0,0,-20));
					//Destroy (i.gameObject);
				}
			}
		}
	}
	public List<GemScript> GemChain(string direction,List<GemScript> answer){

		if(direction == "radius"){
			Chain ("inside",answer);
			answer.RemoveAt (0);
			Chain ("outside", answer);
		}else {
			Chain ("clock", answer);
			answer.RemoveAt (0);
			Chain ("anti", answer);
		}
		if (answer.Count < 3)
			answer = null;
		
		return answer;
	}
	public List<GemScript> Chain(string collidorName,List<GemScript> answer){
		answer.Add (this);
		CollidorScript c = collidors[collidorName];
		if(c.handsup.Count != 0 && c.handsup[0].color == color){
			return c.handsup[0].Chain (collidorName,answer);
		}
		return answer;
	}

	public void FallDown()
	{
		ChangeTo (l.inner);
		PlaceCollidors ();
	}
	public void ChangeTo(LayerScript tolayer)
	{
		if (tolayer != null) {
			l = tolayer;
			// For Debugging .. i'm not changing name as it changes layers.
			if(name=="Gem(Clone)")name = "L" + l.layer + "N" + l.transform.childCount;
			GetComponent<MeshFilter>().mesh = tolayer.mesh[tolayer.layer];
			GetComponent<MeshCollider>().sharedMesh = tolayer.mesh[tolayer.layer];
			transform.parent = tolayer.transform;
		}
	}
	Vector3 mouseInPlanePoint(){
		Vector3 mouseInworldPoint = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.nearClipPlane));
		Vector3 inPlanePoint = mouseInworldPoint - planeCenter;
		return inPlanePoint;
	}

	// Rounds off the rotation angle of the layer.
	void RoundOff(Transform l){

		float current = l.rotation.eulerAngles.z;
		float quotient = (float)Mathf.Floor(current / gemDegrees);
		float extra = current - quotient * gemDegrees;

		if (extra > gemDegrees / 2) {
			l.Rotate(new Vector3(0,0,gemDegrees-extra));
		} else {
			l.Rotate(new Vector3(0,0,-extra));
		}
	}

	public GemScript FindNeigbhour(string side){
		return collidors[side].handsup[0];
	}

	public void PlaceCollidors(){
		foreach (Transform child in transform) {
			child.GetComponent<CollidorScript> ().PlaceCollidor ();
		}
	}
	public void AddCollidors(){

		GameObject[] c = new GameObject[4];
		for (int k = 0; k < 4; k++) {
			c[k] = (GameObject)Instantiate(collidor, new Vector3(0, 0, 0), Quaternion.identity);
			c [k].name = collidorNames[k];
			c [k].transform.parent = transform;
			c [k].GetComponent<CollidorScript> ().g = this;
			collidors.Add (collidorNames[k],c[k].GetComponent<CollidorScript>());
		}
	}

}
