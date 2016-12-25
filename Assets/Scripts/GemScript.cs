using UnityEngine;
using System.Collections;

public class GemScript : MonoBehaviour {
	public int layer;

	// Taken from this answer http://answers.unity3d.com/answers/177525/view.html
	public float sensitivity;
	private Vector3 mouseReference;
	private Vector3 rotation;
	private bool isRotating;
	public Vector3 planeCenter;
	public float gemDegrees;

	void Start ()
	{
		sensitivity = 1.0f;
		rotation = Vector3.zero;
		planeCenter = Camera.main.ViewportToWorldPoint (new Vector3(0.5f,0.5f,Camera.main.nearClipPlane));
	}

	void OnMouseDown()
	{
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
			gameObject.transform.parent.Rotate(rotation);
			mouseReference = newReference;
		}
	}

	void OnMouseUp()
	{
		isRotating = false;
		RoundOff (gameObject.transform.parent);
		gameObject.transform.parent.gameObject.BroadcastMessage ("ValidateAngle");
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
	public void ValidateAngle(){
		
	}
}
