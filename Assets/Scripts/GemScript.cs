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
	private bool isRotating;
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
		naanthaan = 1;
		l.b.BoardLock = 1;
		isRotating = true;
		mouseReference = mouseInPlanePoint ();
	}

	void Update()
	{
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
		isRotating = false;
		RoundOff (transform.parent);
		l.b.BoardLock = 0;
		StartCoroutine(StartValidator());
	}

	public IEnumerator StartValidator() {
		yield return new WaitForSeconds(0.05f); // waits 0.6 seconds
		transform.parent.BroadcastMessage ("ValidateRadius");
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
			l.rotation = Quaternion.Euler(0,0, gemDegrees*(quotient+1));
		} else {
			l.rotation = Quaternion.Euler(0,0, gemDegrees*quotient);
		}
	}
	public void ValidateRadius(){
		Queue<GemScript> inside = GemChain ("inside");
		Queue<GemScript> outside = GemChain ("outside");

		// As same object is counted twice
		if (outside.Count + inside.Count >= 4) {
			GemScript xOut =  outside.Peek ();
			GemScript xIn =  inside.Peek ();
			GemScript xOutPlusOne = xOut.FindOuterFriend();

			foreach (GemScript i in outside) {
				Destroy (i.gameObject);
			}
			foreach (GemScript i in inside) {
				Destroy (i.gameObject);
			}
			if(xOutPlusOne != null){
				xOutPlusOne.FallDown (xIn.l);
			}
		}
		naanthaan = 0;
	}
	public GemScript FindOuterFriend(){
		return transform.Find ("outside").GetComponent<CollidorScript> ().handsup;
	}
	void FallDown(LayerScript tolayer)
	{
		while (l.layer != tolayer.layer) {
			ChangeTo (l.inner);
		}
		PlaceCollidors ();
		GemScript outerFriend = FindOuterFriend();
		if(outerFriend != null){
			outerFriend.FallDown(tolayer.outer);
		}
	}
	public void ChangeTo(LayerScript tolayer)
	{
		l = tolayer;
		name = "L" + l.layer + "N" + i;
		GetComponent<MeshFilter>().mesh = tolayer.mesh[tolayer.layer];
		GetComponent<MeshCollider>().sharedMesh = tolayer.mesh[tolayer.layer];
		transform.parent = tolayer.transform;

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
			if (collidor.name == collidorName && collidor.handsup != null) {
				if (collidor.handsup.GetComponent<GemScript> ().color == color) {
					answer = collidor.handsup.GetComponent<GemScript> ().GemChain (collidorName);
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
			c [k].GetComponent<CollidorScript> ().p = this;
			c [k].GetComponent<CollidorScript> ().b = l.b;
//			c [k].GetComponent<CollidorScript> ().PlaceCollidor ();
		}
	}

}
