using UnityEngine;
using System.Collections;

public class PartPopulator : MonoBehaviour {

	public GameObject collidor;
	public GameObject[] objects;
	public GameObject[] parts;
	public Material[] materials;
	private string[] colors;
	public float gemDegrees = 11.25f; // 360/32
	public float gemRadians = 11.25f * 0.01746031746f;
	public float gemLength = 0.08254f;
	public float gemOrigin = 0.094f;
	void Start () {
		int ts = 32;
		objects = new GameObject[ts];
		for (int l = 0; l < parts.Length; l++) {
			for (int i = 0; i < ts; i++) {
				objects [i] = createGem (l, i);
			}
		}
	}

	GameObject createGem(int l,int i){
		GameObject o = (GameObject)Instantiate(parts[l], new Vector3(0, 0, 0), Quaternion.identity);
		o.GetComponent<Renderer> ().material = randomPicker<Material>(materials);
		o.GetComponent<GemScript> ().layer = l;
		createCollidors (o);
		o.GetComponent<Transform> ().Rotate (0.0f, 0.0f, gemDegrees * i); 
		o.GetComponent<Transform> ().parent = gameObject.GetComponent<Transform> ();
		return o;
	}

	void createCollidors(GameObject p){
		int layer = p.GetComponent<GemScript> ().layer;
		GameObject[] c = new GameObject[4];
		for (int k = 0; k < 4; k++) {
			c[k] = (GameObject)Instantiate(collidor, new Vector3(-gemOrigin - layer * gemLength, 0, 0), Quaternion.identity);
			c[k].GetComponent<CollidorScript> ().side = k;
			c[k].GetComponent<Transform> ().parent = p.GetComponent<Transform> ();
			c [k].GetComponent<CollidorScript> ().p = p.GetComponent<GemScript> ();
			placeCollidor (c [k]);
		}
	}
	void placeCollidor(GameObject c){
		int layer = c.GetComponent<CollidorScript> ().p.layer;
		switch (c.GetComponent<CollidorScript> ().side)
		{
		case 0:
			c.GetComponent<Transform> ().Translate (new Vector3(-gemLength/2,0,0));
			break;
		case 1:
			c.GetComponent<Transform> ().Translate (new Vector3(-gemLength,-0.01f,0));
			break;
		case 2:
			float x_tan = (gemOrigin + layer * gemLength + gemLength / 2);
			float tan = Mathf.Tan (gemRadians);
			float y_tan = tan * x_tan;
			c.GetComponent<Transform> ().Translate (new Vector3(-gemLength/2,-y_tan,0));
			break;
		case 3:
			c.GetComponent<Transform> ().Translate (new Vector3(0,-0.01f,0));
			break;

		}
	}
	T randomPicker<T> (T[] array){
		return array [Random.Range (0, array.Length)];
	}

	void Update () {
	
	}
}
