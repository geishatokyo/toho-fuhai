using UnityEngine;
using System.Collections;

public class KeyNote : Note {
	
	public float lineLength;
	public Vector3 linePosition;
	public float latency;

	public bool touched = false;


	// Use this for initialization
	public override void Start () {
	}
	
	// Update is called once per frame
	public override void Update () {
		if (!touched && song.time + song.noteVisibleTime >= playtime){
			renderer.enabled = true;
			float t = 1 - ((playtime - song.time + latency) / song.noteVisibleTime);
			t = (t < 0) ? 0
			  : (t > 1) ? 1
			  :           t;
			//Debug.Log("note enable t:"+t+" wavid:"+wavid);
			Vector3 scale = transform.localScale;
			scale.y = lineLength * t;
			transform.localScale = scale;
			transform.localPosition = linePosition * t;
		} else {
			renderer.enabled = false;
		}

		if (!played && song.time >= playtime){
			play();
		}
	}

	public void play(){
		//Debug.Log("play "+wavid+"(wavid) KeyNote");
		song.playWav(wavid);
		
		played = true;
	}
}
