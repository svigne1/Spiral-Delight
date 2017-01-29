using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GemGroup{
	public GemLogic center;
	public List<GemLogic> right;
	public List<GemLogic> left;
	public string direction;
	public int count;

	public void RemoveCenter(){
		right.Remove(center);
		left.Remove(center);
	}
	public void Print(){
		Debug.Log ("center "+center.name);
		foreach (GemLogic s in right)
			Debug.Log (s.name);
		foreach (GemLogic s in left)
			Debug.Log (s.name);
	}
	public void SetReferenceFor(GemLogic a){
			a.groups [direction] = this;
			a.process [direction] = true;
	}
	public void doCount(){
		count = 1+ right.Count+ left.Count;
	}
	public void selfDestruct(){
		center.selfDestruct ();

		foreach (GemLogic i in left) {
			i.selfDestruct ();
		}
		foreach (GemLogic i in right) {
			i.selfDestruct ();
		}
	}
}

public class GemLogic : MonoBehaviour {
	public GemConfig c;
	public GemGravity gy;

	public Dictionary<string, GemGroup> groups;
	public Dictionary<string, bool> process;
	void Start ()
	{
		groups = new Dictionary<string, GemGroup>();
		process = new Dictionary<string, bool>();
		process["radius"] = false;
		process["circumference"] = false;
	}

	public void ResetProcess(){
		process["radius"] = false;
		process["circumference"] = false;
		groups = new Dictionary<string, GemGroup>();
	}
	public void selfDestruct(){
		c.Destroyed = true;
		transform.Translate (new Vector3(0,0,-20));
//		Destroy (gameObject);
	}
	public GemGroup getGemGroup(string direction){
		GemGroup answer = new GemGroup();
		process [direction] = true;

		if (groups.TryGetValue (direction, out answer)) {
			return answer;
		} else{

			answer = new GemGroup();
			answer.direction = direction;

			if(direction == "radius"){
				answer.left = Chain ("inside",new List<GemLogic>());
				answer.right = Chain ("outside", new List<GemLogic>());
			} else if(direction == "circumference"){
				answer.right = Chain ("clock", new List<GemLogic>());
				answer.left = Chain ("anti", new List<GemLogic>());
			}

			answer.center = this;
			answer.RemoveCenter ();

			if (answer.left.Count + answer.right.Count + 1 < 3)
				answer = null;
			else
				answer.doCount ();
			
			groups [direction] = answer;
		}

		return answer;
	}
	public List<GemLogic> Chain(string collidorName,List<GemLogic> answer){
		if (!answer.Contains (this))
			answer.Add (this);
		CollidorScript t = c.collidors[collidorName];
		if(t.handsup.Count != 0 && t.handsup[0].c.color == c.color){
			return t.handsup[0].Chain (collidorName,answer);
		}
		return answer;
	}
	

}
