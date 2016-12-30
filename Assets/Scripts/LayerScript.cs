using UnityEngine;
using System.Collections;

public class LayerScript : MonoBehaviour {

	public int layer;
	public LayerScript inner,outer;

	public BoardScript b;
	public GameObject gem;
	public Mesh[] mesh;
	public Material[] materials;

	public float tanGemDegrees;
	public float gemDegrees = 11.25f; // 360/32
	public float gemRadians = 11.25f * 0.01746031746f;
	public float gemLength = 0.08254f;
	// Skipping first part 0.094f+ 0.08254f
	public float gemOrigin = 0.17654f;

	public GemScript AddGem(int i){
		GemScript o = ((GameObject)Instantiate(gem, new Vector3(0, 0, 0), Quaternion.identity)).GetComponent<GemScript>();
		o.i = i;
		o.gemDegrees = gemDegrees;
		o.ChangeTo (this);
		o.GetComponent<Renderer> ().material = randomPicker<Material>(materials);
		o.color = o.GetComponent<Renderer> ().material.name;
		o.AddCollidors ();
		o.transform.Rotate (0.0f, 0.0f, gemDegrees * i); 
		o.PlaceCollidors ();
		return o;
	}

	T randomPicker<T> (T[] array){
		return array [Random.Range (0, array.Length)];
	}
		
}
