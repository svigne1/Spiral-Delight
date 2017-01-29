using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemInput : MonoBehaviour {

	public GemConfig c;

	public float sensitivity;
	private Vector3 mouseReference;
	private Vector3 rotation;
	private bool isRotating,onMouseRelease;
	public Vector3 planeCenter;

	void Start ()
	{
		sensitivity = 1.0f;
		rotation = Vector3.zero;
		c.Destroyed = false;
		planeCenter = Camera.main.ViewportToWorldPoint (new Vector3(0.5f,0.5f,Camera.main.nearClipPlane));
	}

	void OnMouseDown()
	{
		if (c.l.b.Equilibrium == 0) {
			foreach (Transform child in c.l.transform) {
				GemLogic temp = child.GetComponent<GemLogic> ();
				if (!temp.c.Destroyed)
					c.l.b.AddToChangeList (child.GetComponent<GemLogic>());
			}
			c.l.b.AddToChangeList (GetComponent<GemLogic>());
			c.l.b.Gravity = false;
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
			c.l.b.Gravity = true;
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


	Vector3 mouseInPlanePoint(){
		Vector3 mouseInworldPoint = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.nearClipPlane));
		Vector3 inPlanePoint = mouseInworldPoint - planeCenter;
		return inPlanePoint;
	}

	// Rounds off the rotation angle of the layer.
	void RoundOff(Transform l){

		float current = l.rotation.eulerAngles.z;
		float quotient = (float)Mathf.Floor(current / c.l.gemDegrees);
		float extra = current - quotient * c.l.gemDegrees;

		if (extra > c.l.gemDegrees / 2) {
			l.Rotate(new Vector3(0,0,c.l.gemDegrees-extra));
		} else {
			l.Rotate(new Vector3(0,0,-extra));
		}
	}
}
