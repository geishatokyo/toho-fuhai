using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour, IReset {
	Master master;
	GameObject resultCamera;
	public bool active = false;
	static readonly int[] judgePoint = {50, 20, 10, 1, 0};
	static readonly int[] recordPoint = {10, 1000, -2000, 20, 100, 100};
	public int score;

	public bool clear;

	float startTime;
	public readonly float resultVisibleTime = 1.5f;


	string strJudgeBase = "PERFECT\nGREAT\nGOOD\nBAD\nMISS\nTOTAL\n";
	string strRecordBase = "ATTACK\nHIT\nDAMAGE\nHEAL\nMAX HEAL\nMAX COMBO\nSCORE";
	string strJudgeScore;

	string strClear = "\n\n\n\n\n\n";

	string strJudgeNum;
	string strRecordScore;

	void Awake(){
		for (int i = 0; i < 5; ++i) {
			strJudgeScore += "*"+judgePoint[i]+"\n";
		}
		strJudgeScore += "\n\n";
		
		TextMesh recordScore = getTextMesh("ResultRecordScore");
		string recordScoreStr = "";
		for (int i = 0; i < 6; ++i) {
			recordScoreStr += "*" + recordPoint[i] + "\n";
		}
		recordScoreStr += "\n";
		recordScore.text = recordScoreStr;

		resultCamera = transform.FindChild ("ResultCamera").gameObject;
	}

	TextMesh getTextMesh(string objname){
		return transform.FindChild(objname).GetComponent<TextMesh>();
	}

	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
			master.attachIReset(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!active){
			return;
		}

		if (startTime + resultVisibleTime < Time.time) {
			resultCamera.SetActive(true);
		}

		
		TextMesh judgeBase = getTextMesh("ResultJudgeBase");
		judgeBase.text = strJudgeBase;
		TextMesh recordBase = getTextMesh ("ResultRecordBase");
		recordBase.text = strRecordBase;
		TextMesh judgeScore = getTextMesh("ResultJudgeScore");
		judgeScore.text = strJudgeScore;

		TextMesh resultClear = getTextMesh("ResultClear");
		resultClear.text = strClear + ((clear) ? "-  CLEAR!!!  -" : "- GAME OVER -");

		score = calcScore();

		strJudgeNum = "";
		for (int i = 0; i < master.song.judgeNum.Length; ++i) {
			strJudgeNum += master.song.judgeNum[i]+"\n";
		}
		strJudgeNum += master.song.totalNotes + "\n";

		strRecordScore = "";
		for (int i = 0; i < 5; ++i) {
			strRecordScore += master.record[i] + "\n";
		}
		strRecordScore += master.song.maxcombo + "\n";
		strRecordScore += score;


		TextMesh judgeNum = getTextMesh ("ResultJudgeNum");
		judgeNum.text = strJudgeNum;
		TextMesh recordNum = getTextMesh("ResultRecordNum");
		recordNum.text = strRecordScore;

	}

	public int calcScore(){
		int score = 0;
		for (int i = 0; i < 5; ++i) {
			score += master.song.judgeNum[i] * judgePoint[i];
		}

		for (int i = 0; i < 5; ++i) {
			score += master.record[i] * recordPoint[i];
		}

		score += master.song.maxcombo * recordPoint [5];

		return score;
	}

	public void reset(){
		active = false;
		resultCamera.SetActive(false);
	}

	public void onResult(bool isClear){
		if (active) {
			return;
		}
		clear = isClear;
		startTime = Time.time;
		active = true;
		Debug.Log ("on result");
		//resultCamera.SetActive (true);
	}
}
