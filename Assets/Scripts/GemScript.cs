using UnityEngine;
using System.Collections;

public class GemScript : MonoBehaviour {
	public int layer;
	public Collider2D r,l,t,b;
	public string rt, lt, tt, bt;
	public GameObject chithu;
	void Start() {
//		pos = GetComponent<Transform>();
	}

	public void addNeigbour(int side, Collider2D n){
		switch (side) 
		{
		case 0:
			b = n;
			bt = n.gameObject.tag;
			chithu = n.gameObject.GetComponent<CollidorScript>().p.gameObject;
			break;
		case 1:
			r = n;
			rt = n.gameObject.tag;
			break;
		case 2:
			t = n;
			tt = n.gameObject.tag;
			break;
		case 3:
			l = n;
			lt = n.gameObject.tag;
			break;
		}
	}
	void FixedUpdate(){
	}
}
