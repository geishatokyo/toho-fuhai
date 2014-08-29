using UnityEngine;
using System.Collections;

public class Judge : MonoBehaviour {
	public Song song;
	public float visibleTime = 0.4f;
	private float lastChangedTime;
	private int lastNoteNum;

	public static readonly string[] judgeTexts =
		new string[]{"PERFECT", "GREAT", "GOOD", "BAD", "MISS"};


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (lastNoteNum != song.pastNoteNum){
			TextMesh tm = GetComponent<TextMesh> ();
			lastChangedTime = song.time;
			lastNoteNum = song.pastNoteNum;
			int judge = song.lastJudge;
			judge = (judge == -1) ? 4 : judge;
			if(song.combo != 0){
				tm.text = judgeTexts[song.lastJudge] + " " + song.combo;
			}else{
				tm.text = judgeTexts[song.lastJudge];
			}
			renderer.enabled = true;
		}
		if (lastChangedTime + visibleTime < song.time){
			renderer.enabled = false;
		}
	}
}
