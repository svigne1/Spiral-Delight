using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemScript : MonoBehaviour {
	public int layer;

	// Taken from this answer http://answers.unity3d.com/answers/177525/view.html
	public float sensitivity;
	private Vector3 mouseReference;
	private Vector3 rotation;
	private bool isRotating;
	public Vector3 planeCenter;
	public float gemDegrees;
	public int naanthaan = 0;
	public string color;
	public BoardScript b;

	void Start ()
	{
		sensitivity = 1.0f;
		rotation = Vector3.zero;
		planeCenter = Camera.main.ViewportToWorldPoint (new Vector3(0.5f,0.5f,Camera.main.nearClipPlane));
	}

	void OnMouseDown()
	{
		naanthaan = 1;
		b.BoardLock = 1;
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
		b.BoardLock = 0;
		StartCoroutine(StartValidator());
		naanthaan = 0;

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

		Stack<GameObject> one = GemChain (1);
		one.Pop ();
		Stack<GameObject> two = GemChain (3);
		if (naanthaan == 1) {
			print (one.Count);
			print (two.Count);
		}
		if (one.Count + two.Count >= 3) {
			foreach (GameObject i in one) {
				Destroy (i);
			}
			foreach (GameObject i in two) {
				Destroy (i);
			}
		}
	}

	Stack<GameObject> GemChain(int side){
		Stack<GameObject> answer;
		foreach (Transform child in transform) {
			CollidorScript collidor = child.GetComponent<CollidorScript> ();
			if (collidor.side == side && collidor.handsup != null) {
				if (collidor.handsup.GetComponent<GemScript> ().color == color) {
					answer = collidor.handsup.GetComponent<GemScript> ().GemChain (side);
					answer.Push (gameObject);
					return answer;
				} 
			}
		}
		answer = new Stack<GameObject> ();
		answer.Push (gameObject);
		return answer;
	}
}
