using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GemGroup{
	public GemLogic center;
	public List<GemLogic> positive;
	public List<GemLogic> negative;
	public string direction;

	public void RemoveCenter(){
		positive.Remove(center);
		negative.Remove(center);
	}
	public void Print(){
		Debug.Log ("center "+center.name);
		foreach (GemLogic s in positive)
			Debug.Log (s.name);
		foreach (GemLogic s in negative)
			Debug.Log (s.name);
	}
	public void SetReferenceFor(GemLogic a){
			a.groups [direction] = this;
			a.process [direction] = true;
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
	}

	public GemGroup GemGroup(string direction){
		GemGroup answer = new GemGroup();
		process [direction] = true;

		if (groups.TryGetValue (direction, out answer)) {
			return answer;
		} else{

			answer = new GemGroup();
			answer.direction = direction;

			if(direction == "radius"){
				answer.negative = Chain ("inside",new List<GemLogic>());
				answer.positive = Chain ("outside", new List<GemLogic>());
			} else if(direction == "circumference"){
				answer.positive = Chain ("clock", new List<GemLogic>());
				answer.negative = Chain ("anti", new List<GemLogic>());
			}

			answer.center = this;
			answer.RemoveCenter ();

			if (answer.negative.Count + answer.positive.Count + 1 < 3)
				answer = null;
			
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
