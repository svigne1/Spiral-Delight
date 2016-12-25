using UnityEngine;
using System.Collections;

public class GemScript : MonoBehaviour {
	public int layer;

	// Taken from this answer http://answers.unity3d.com/answers/177525/view.html
	public float sensitivity;
	private Vector3 mouseReference;
	private Vector3 rotation;
	private bool isRotating;
	public Camera camera;
	public Vector3 planeCenter;
	public float gemDegrees;

	void Start ()
	{
		sensitivity = 1.0f;
		rotation = Vector3.zero;
		camera = Camera.main;
		gemDegrees = gameObject.transform.parent.parent.gameObject.GetComponent<BoardScript> ().gemDegrees;
		planeCenter = camera.ViewportToWorldPoint (new Vector3(0.5f,0.5f,camera.nearClipPlane));
		print (planeCenter);
	}

	void Update()
	{
		if(isRotating)
		{
			
			Vector3 newReference = mouseToPlanePoint (Input.mousePosition);
//			camera.transform.position;
			float finalAngle = Vector3.Angle( mouseReference, newReference);
			if (Vector3.Cross (mouseReference, newReference).z < 0)
				finalAngle *= -1;
//			finalAngle = RoundOff (finalAngle);

			rotation.z = finalAngle * sensitivity;
			gameObject.transform.parent.Rotate(rotation);
			mouseReference = newReference;
		}
	}

	void OnMouseDown()
	{
		isRotating = true;
		mouseReference = mouseToPlanePoint (Input.mousePosition);
	}

	void OnMouseUp()
	{
		isRotating = false;
		RoundOff (gameObject.transform.parent);
	}

	Vector3 mouseToPlanePoint(Vector3 mousePosition){
		Vector3 worldPoint = camera.ScreenToWorldPoint (new Vector3(mousePosition.x,mousePosition.y,camera.nearClipPlane));
		Vector3 planePoint = worldPoint - planeCenter;
		return planePoint;
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
//	float RoundOff(float current){
//
//		float quotient = (float)Mathf.Floor(current / gemDegrees);
//		float extra = current - quotient * gemDegrees;
//
//		if (extra > gemDegrees / 2) {
//			return gemDegrees*(quotient+1);
//		} else {
//			return gemDegrees*quotient;
//		}
//	}
}
