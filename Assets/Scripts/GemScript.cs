using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemScript : MonoBehaviour {
	public LayerScript l;
	public GameObject collidor;
	public int i;

	// Taken from this answer http://answers.unity3d.com/answers/177525/view.html
	public float sensitivity;
	private Vector3 mouseReference;
	private Vector3 rotation;
	public string[] collidorNames = new string[]{"outside","inside","clock","anti"};
	private bool isRotating,onMouseRelease;
	public Vector3 planeCenter;
	public float gemDegrees;
	public int naanthaan = 0;
	public string color;

	void Start ()
	{
		sensitivity = 1.0f;
		rotation = Vector3.zero;
		planeCenter = Camera.main.ViewportToWorldPoint (new Vector3(0.5f,0.5f,Camera.main.nearClipPlane));
	}

	void OnMouseDown()
	{
		if (l.b.BoardLock == 0) {
			l.b.BoardLock++;
			naanthaan = 1;
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
			StartValidator ();
			l.b.BoardLock--;
			naanthaan = 0;
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

	public void StartValidator() {
		transform.parent.BroadcastMessage ("ValidateRadius");
	}
	public void ValidateRadius(){
		Queue<GemScript> inside = GemChain ("inside");
		Queue<GemScript> outside = GemChain ("outside");

		// As same object is counted twice
		if (outside.Count + inside.Count >= 4) {

			foreach (GemScript i in outside) {
				i.transform.Translate (new Vector3(0,0,-20));
				Destroy (i.gameObject);
			}
			foreach (GemScript i in inside) {
				i.transform.Translate (new Vector3(0,0,-20));
				Destroy (i.gameObject);
			}
		}
	}
	public void FallDown()
	{
		l.b.BoardLock++;
		ChangeTo (l.inner);
		PlaceCollidors ();
		l.b.BoardLock--;
	}
	public void ChangeTo(LayerScript tolayer)
	{
		if (tolayer != null) {
			l = tolayer;
			name = "L" + l.layer + "N" + l.transform.childCount;
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
		return transform.Find (side).GetComponent<CollidorScript> ().handsup[0];
	}

	public void PlaceCollidors(){
		foreach (Transform child in transform) {
			child.GetComponent<CollidorScript> ().PlaceCollidor ();
		}
	}
	Queue<GemScript> GemChain(string collidorName){
		Queue<GemScript> answer;
		foreach (Transform child in transform) {
			CollidorScript collidor = child.GetComponent<CollidorScript> ();
			if (collidor.name == collidorName && collidor.handsup.Count != 0) {
				if (collidor.handsup[0].color == color) {
					answer = collidor.handsup[0].GemChain (collidorName);
					answer.Enqueue (this);
					return answer;
				} 
			}
		}
		answer = new Queue<GemScript> ();
		answer.Enqueue (this);
		return answer;
	}

	public void AddCollidors(){

		GameObject[] c = new GameObject[4];
		for (int k = 0; k < 4; k++) {
			c[k] = (GameObject)Instantiate(collidor, new Vector3(0, 0, 0), Quaternion.identity);
			c [k].name = collidorNames[k];
			c [k].transform.parent = transform;
			c [k].GetComponent<CollidorScript> ().g = this;
		}
	}

}
