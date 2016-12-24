using UnityEngine;
using System.Collections;

public class GemScript : MonoBehaviour {
	public int layer;

	// Taken from this answer http://answers.unity3d.com/answers/177525/view.html
	public float sensitivity;
	private Vector3 mouseReference;
	private Vector3 mouseOffset;
	private float degrees;
	private Vector3 rotation;
	private bool isRotating;

	void Start ()
	{
		sensitivity = 1.6f;
		rotation = Vector3.zero;
	}

	void Update()
	{
		if(isRotating)
		{
			// offset
//			mouseOffset = (Input.mousePosition - mouseReference);
			degrees = Vector3.Angle(Input.mousePosition,mouseReference);
			float sign = Mathf.Sign(Vector3.Dot(Input.mousePosition,mouseReference));
			float finalAngle = sign * degrees;

			// apply rotation
//			rotation.z = -(mouseOffset.x + mouseOffset.y) * sensitivity;
			rotation.z = finalAngle * sensitivity;

			// rotate
			gameObject.transform.parent.Rotate(rotation);

			// store mouse
			mouseReference = Input.mousePosition;
		}
	}

	void OnMouseDown()
	{
		// rotating flag
		isRotating = true;

		// store mouse
		mouseReference = Input.mousePosition;
	}

	void OnMouseUp()
	{
		// rotating flag
		isRotating = false;
		RoundOff (gameObject.transform.parent);
	}

	void RoundOff(Transform l){
		float gemDegrees = l.transform.parent.gameObject.GetComponent<PartPopulator> ().gemDegrees;

		float current = l.rotation.eulerAngles.z;
		float quotient = (float)Mathf.Floor(current / gemDegrees);
		float extra = current - quotient * gemDegrees;

		print (current);
		print (extra);
		if (extra > gemDegrees / 2) {
			l.rotation = Quaternion.Euler(0,0, gemDegrees*(quotient+1));
		} else {
			l.rotation = Quaternion.Euler(0,0, gemDegrees*quotient);
		}
	}
}
