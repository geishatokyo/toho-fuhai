using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour {

	public Song song;
	public int wavid;
	public float playtime;
	public bool testflag;

	public bool played;

	// Use this for initialization
	public virtual void Start () {
		if (testflag) {
			song.setPath ("songs/yozakura/");
			song.setWav (wavid, "bgm01");
			testflag = false;
		}
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (!played && song.time >= playtime){
			song.playWav(wavid);
			//Debug.Log("play "+wavid+"(wavid)");

			played = true;
			//Destroy(this);
		}
	}
}
