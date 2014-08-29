using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Song : MonoBehaviour, IReset {
	public static readonly int KEYS = 4;

	Master master;

	public GameObject audioPrefab;
	AudioSource[] audioSources;

	public SortedList<float, KeyNote>[] keyLanes;
	public List<Note> bgmLane;
	public int[] nextNoteIndexs;
	public int totalNotes;

	public string path;

	public float time;
	public float startTime;
	public bool playing;

	public float noteVisibleTime;
	public static readonly float[] judgeTime
		= new float[]{0.050f, 0.100f, 0.150f, 0.200f, 0.300f};
		//= new float[]{0.010f, 0.020f, 0.035f, 0.070f, 0.100f};
	
	public int combo;
	public int maxcombo;

	public int[] judgeNum = new int[5];

	public int pastNoteNum;
	public int lastJudge;

	public float latency = 0.10f;

	//private GameObject[] touchKeyEffectPrefabs;
	private static readonly string[] keyTouchPrefabNames =
		{"Perfect","Great","Good","Bad","Miss"};
	private GameObject[] lines;


	void Awake(){
		
		audioSources = new AudioSource[36 * 36];

		keyLanes = new SortedList<float, KeyNote>[KEYS];
		nextNoteIndexs = new int[KEYS];
		for(int i = 0; i < KEYS; ++i){
			keyLanes[i] = new SortedList<float, KeyNote>();
			nextNoteIndexs[i] = 0;
		}
		bgmLane = new List<Note>();

		lines = new GameObject[KEYS];
		for (int i = 1; i <= KEYS; ++i){
			lines[i-1] = GameObject.Find("Note"+i);
		}

		GameObject.Find ("Master").GetComponent<Master>().attachIReset(this);
	}

	public void reset(){
		time = 0;
		lastJudge = -1;

		combo = 0;
		maxcombo = 0;
		pastNoteNum = 0;
		playing = false;
		for(int i = 0; i < KEYS; ++i){
			nextNoteIndexs[i] = 0;
		}
		foreach (AudioSource audio in audioSources){
			if(audio != null){
				audio.Stop();
			}
		}
		foreach (Note note in bgmLane) {
			note.played = false;
		}
		for(int i = 0; i < KEYS; ++i){
			foreach (KeyNote note in keyLanes[i].Values) {
				note.played = false;
				note.touched = false;
			}
		}

		for (int i = 0; i < judgeNum.Length; ++i) {
			judgeNum [i] = 0;
		}
	}


	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (nextNoteIndexs[0]+nextNoteIndexs[1]+nextNoteIndexs[2]+nextNoteIndexs[3] > 180){
			playing = false;
			Debug.Log("all note pasted. error");
		}
		*/
		if (nextNoteIndexs [0] >= keyLanes [0].Count &&
		    nextNoteIndexs [1] >= keyLanes [1].Count &&
			nextNoteIndexs [2] >= keyLanes [2].Count &&
			nextNoteIndexs [3] >= keyLanes [3].Count) {
			GameObject.Find("Result").GetComponent<Result>().onResult(true);
		}
		if (!playing) {
			return;
		}

		//time += Time.deltaTime;
		time = Time.time - startTime;

		for (int i = 0; i < KEYS; ++i) {
			if(keyLanes[i].Count <= 0){
				break;
			}
			IList<KeyNote> values = keyLanes[i].Values;
			if(values.Count > nextNoteIndexs[i]){
				KeyNote keynote = keyLanes[i].Values[nextNoteIndexs[i]];
				float nextTime = keynote.playtime;
				//Debug.Log("nextTime"+i+":"+nextTime);
				if(nextTime + judgeTime[3] + latency < time){
					//Debug.Log("poor");
					combo = 0;
					lastJudge = 4;
					keynote.touched = true;
					//keyLanes[i].RemoveAt (0);
					++nextNoteIndexs[i];
					++pastNoteNum;
					++judgeNum[4];
					master.addGauge(TouchCharge.energyPoints[TouchCharge.energyPoints.Length-1]);
				}
			}
		}
	}

	public void setPath(string path){
		this.path = path;
	}

	public void setWav(int wavid, string filename){
		if (audioSources [wavid] == null){
			GameObject audioSourceObj = (GameObject)Instantiate (audioPrefab);
			audioSources [wavid] = audioSourceObj.GetComponent<AudioSource>();
		}
		audioSources[wavid].clip = (AudioClip)Resources.Load (path + filename);
	}

	public void playWav(int wavid){
		if (audioSources [wavid] == null) {
			return;
		}
		audioSources[wavid].Play();
	}

	public void beginSong(){
		playing = true;
		startTime = Time.time;
	}

	public void stopSong(){
		playing = false;
	}

	public void setKeyNote(int laneID, float time, GameObject note){
		keyLanes[laneID-1].Add(time, note.GetComponent<KeyNote>());
		//Debug.Log("set key note laneID:"+laneID+" time:"+time+" keyLanes count"+keyLanes[laneID-1].Count);
		//totalNotes = nextNoteIndexs [0] + nextNoteIndexs [1] + nextNoteIndexs [2] + nextNoteIndexs [3];
		++totalNotes;
	}

	public void setNote(GameObject note){
		bgmLane.Add(note.GetComponent<Note>());
	}

	public int touch(int laneID){
		int lid = laneID - 1;
		if (keyLanes[lid].Count <= 0){
			return -1;
		}

		IList<KeyNote> values = keyLanes [lid].Values;
		if (values.Count <= nextNoteIndexs [lid]){
			return -1;
		}
		KeyNote topNote = values[nextNoteIndexs[lid]];
		float nextTime = topNote.playtime;
		float latedTime = time - latency;
		//Debug.Log ("nextTime:"+nextTime+" nowTime:"+time);
		int judge = -1;
		for (int i = 0; i < judgeTime.Length; ++i) {
			if(nextTime - judgeTime[i] < latedTime && latedTime < nextTime + judgeTime[i]){
				judge = i;
				break;
			}
		}
		//Debug.Log ("judge:"+judge);
		if (judge == -1){
			return -1;
		}

		lastJudge = judge;
		if (judge >= 2){
			combo = 0;
		} else {
			++combo;
			maxcombo = (maxcombo > combo) ? maxcombo : combo;
		}
		topNote.touched = true;

		++nextNoteIndexs[lid];
		++pastNoteNum;
		judgeNum [judge]++;
		playTouchEffect (lid, judge);
		return judge;
	}

	void playTouchEffect(int laneID, int judge){
		//key touch
		if (judge > 1) {
			return;
		}
		string effectName = keyTouchPrefabNames[judge];
		Transform effect = lines[laneID].transform.FindChild(effectName);
		/*
		if(effect == null){
			effect = ((GameObject)GameObject.Instantiate(touchKeyEffectPrefabs[judge])).transform;
			effect.parent = lines[laneID].transform;
			effect.localPosition = effect.TransformPoint(effect.parent.localPosition);
			effect.localRotation = effect.transform.rotation;
			effect.localScale = effect.parent.localScale;
			effect.name = effectName;
		}
		Debug.Log(effect.position);
		Debug.Log(effect.parent.position);
		Debug.Log("--------");
		*/
		effect.GetComponent<ParticleSystem>().Play();
	}
}
