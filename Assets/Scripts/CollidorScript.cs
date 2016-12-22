using UnityEngine;
using System.Collections;

public class CollidorScript : MonoBehaviour {

	public int side;
	public GemScript p;
	// Use this for initialization
	void OnTriggerEnter2D (Collider2D o) {
		p.addNeigbour(side,o);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
