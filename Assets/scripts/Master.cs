using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Master : MonoBehaviour ,IReset {

	//public GameObject songObj;
	public Song song;
	public Result result;

	private bool started = false;
	float energyGauge;
	public static readonly float MIN_GAUGE = 0.001f;
	public static readonly float MAX_GAUGE = 1.0f;
	int life = 800;
	public static readonly int MIN_LIFE = 0;
	public static readonly int MAX_LIFE = 1000;
	
	public static readonly float ATTACK_COST = 0.2f;
	public static readonly float HEAL_COST = 0.4f;
	public static readonly int HEAL_POINT = 300;
	public static readonly float HEALMAX_COST = 0.99f;

	private IList<IReset> resetItems;

	public int[] record;
	public static readonly int RECORD_ATTACK = 0;
	public static readonly int RECORD_HIT = 1;
	public static readonly int RECORD_DAMAGE = 2;
	public static readonly int RECORD_HEAL = 3;
	public static readonly int RECORD_MAXHEAL = 4;

	GameObject startbutton;
	GameObject areyouready;

	void Awake(){
		if (song == null) {
			song = GameObject.Find ("Song").GetComponent<Song> ();
		}
		if (result == null) {
			result = GameObject.Find("Result").GetComponent<Result>();
		}

		attachIReset(this);

		record = new int[5];
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (life <= MIN_LIFE) {
			song.stopSong();
			result.onResult(false);
		}
	}

	void LateUpdate(){
		if (!started){
			started = true;
			GameObject loader = GameObject.Find ("BMSLoader");
			loader.GetComponent<loadSong>().loadBMS("Songs/kiyurougetu/", "for4key");
			resetAll();
		}
	}

	public void addGauge(float add){
		energyGauge += add;
		energyGauge = (energyGauge < MIN_GAUGE) ? MIN_GAUGE
		            : (energyGauge > MAX_GAUGE) ? MAX_GAUGE
		            :                             energyGauge;
		//Debug.Log("gauge:"+energyGauge);
	}


	public float getGauge(){
		return energyGauge;
	}

	public void addLife(int add){
		life += add;
		life = (life < MIN_LIFE) ? MIN_LIFE
		     : (life > MAX_LIFE) ? MAX_LIFE
		     :                     life;
	}
	
	public float getLifeRatio(){
		return (float)life / MAX_LIFE;
	}
	
	public int getLife(){
		return life;
	}

	public void reset(){
		energyGauge = MIN_GAUGE;
		life = MAX_LIFE;
		for (int i=0; i < 5; ++i) {
			record[i] = 0;
		}
	}

	public void attachIReset(IReset obj){
		if (resetItems == null) {
			resetItems = new List<IReset>();
		}

		if (resetItems.Contains(obj)){
			Debug.Log("It's multipul attach!");
			return;
		}
		resetItems.Add(obj);
	}

	public void resetAll(){
		foreach (IReset reset in resetItems){
			reset.reset();
		}
		//song.beginSong();
		setAreYouReadyVisible(true);
	}

	public void setAreYouReadyVisible(bool visible){
		if (startbutton == null) {
			startbutton = GameObject.Find("Panels/Start/Start");
		}
		startbutton.SetActive(visible);

		if (areyouready == null) {
			areyouready = GameObject.Find("AreYouReady");
		}
		areyouready.SetActive(visible);
	}

	public void startGame(){
		setAreYouReadyVisible(false);
		song.beginSong();
	}

	public bool attack(){
		if (energyGauge < ATTACK_COST) {
			return false;
		}
		addGauge(-ATTACK_COST);
		++record[RECORD_ATTACK];
		return true;
	}

	public bool heal(){
		if (energyGauge < HEAL_COST) {
			return false;
		}
		addGauge (-HEAL_COST);
		addLife (HEAL_POINT);
		++record[RECORD_HEAL];
		return true;
	}

	public bool healmax(){
		if (energyGauge < HEALMAX_COST) {
			return false;
		}
		addGauge (-HEALMAX_COST);
		addLife (MAX_LIFE);
		++record[RECORD_MAXHEAL];
		return true;
	}
	
}
