using UnityEngine;
using System.Collections;

public class GemScript : MonoBehaviour {
	public int layer;
	public Collider2D r,l,t,b;
	void Start() {
//		pos = GetComponent<Transform>();
	}

	public void addNeigbour(int side, Collider2D n){
		switch (side) 
		{
		case 0:
			b = n;
			break;
		case 1:
			r = n;
			break;
		case 2:
			t = n;
			break;
		case 3:
			l = n;
			break;
		}
	}
	void FixedUpdate(){
	}
}
