using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using System.IO;
using System.Text;

public class loadSong : MonoBehaviour {
	static int testcount = 0;

	public Song song;
	public GameObject notePrefab;
	private bool testflag;
	public Material keyNoteMat;

	void Awake(){
		Debug.Log("in LoadTest's Awake.");
		testflag = true;
		Debug.Log ("Application.streamingAssetsPath: " + Application.streamingAssetsPath);

		if (keyNoteMat == null){
			keyNoteMat =  (Material)Resources.Load("SongMat/KeyNoteMat");
		}
		}
	// Use this for initialization
	void Start () {
		if (testflag) {
			//loadTest ();
			testflag = false;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void loadTest(){
		//loadBMS("Songs/yozakura/", "_71.bms");
		//loadBMS("Songs/yozakura/", "_71");
		loadBMS("Songs/kiyurougetu/", "for6key");
		//StartCoroutine(loadBMS("songs/yozakura/", "_71.bms"));
	}

	//public IEnumerator loadBMS(string path, string bmsfile){
	public void loadBMS(string path, string bmsfile){
		string[] bmsfilelines;
		/*
		StreamReader sr;
#if UNITY_EDITOR
		sr = new StreamReader(
			"Assets/Resources/" + path + bmsfile, Encoding.GetEncoding("UTF-8"));
		yield return new WaitForSeconds(0f);
#elif UNITY_ANDROID
		string tmpPath =
			Application.streamingAssetsPath+"/resources/" + path + bmsfile;
		Debug.Log ("tmpPath:" + tmpPath);
		WWW www = new WWW(tmpPath);
		yield return www;
		if(www.error != null)
			Debug.Log(www.error);
		sr = new StreamReader(www.text, Encoding.GetEncoding("UTF-8"));
#endif
		string bmsfileall = sr.ReadToEnd();
		bmsfilelines = bmsfileall.Split('\n');
		Debug.Log ("log:" + bmsfilelines[1]);
		sr.Close();
		*/
		Debug.Log("start load file");
		TextAsset t = (TextAsset)Resources.Load(path + bmsfile);
		bmsfilelines = t.text.Split('\n');

		song.setPath (path);

		Debug.Log("loaded:"+path+bmsfile);

		parseBMS(bmsfilelines);
	}

	GameObject createKeyNote(int wavid, float time, int nodeID){
		GameObject line = GameObject.Find ("Note" + nodeID);

		GameObject newNote = (GameObject)Instantiate(line);
		newNote.renderer.material = keyNoteMat;

		LineBlink blinkScript = newNote.GetComponent<LineBlink> ();
		blinkScript.blink = false;

		KeyNote noteScript = newNote.AddComponent<KeyNote>();

		noteScript.linePosition = line.transform.localPosition;
		noteScript.lineLength = line.transform.localScale.y;

		noteScript.wavid = wavid;
		noteScript.playtime = time;
		noteScript.song = song;

		noteScript.latency = song.latency;
		return newNote;
	}

	GameObject createNote(int wavid, float time){
		GameObject newNote = (GameObject)Instantiate (notePrefab);

		Note noteScript = newNote.GetComponent<Note> ();

		noteScript.wavid = wavid;
		noteScript.playtime = time;
		noteScript.song = song;

		song.setNote (newNote);
		//Debug.Log ("wavid:"+wavid+" time:"+time);
		return newNote;
	}

	int radixConvert(int baseNum, string numstr){
		int ret = -1;
		switch(baseNum){
		case 36:
			int i;
			int l = numstr.Length;
			char c;
			ret = 0;

			for(i = 0; i < l; ++i){
				ret *= baseNum;
				c = numstr[i];
				if('0' <= c && c <= '9'){
					ret += (c - '0');
				}else if('a' <= c && c <= 'z'){
					ret += (c - 'a')+10;
				}else if('A' <= c && c <= 'Z'){
					ret += (c - 'A')+10;
				}else{
					return -1;
				}
			}
			//Debug.Log("l:"+l+" ret:"+ret);
			break;
		default:
			break;
		}
		return ret;
	}

	int[] decodeCommand(string commandData){
		int l = commandData.Length / 2;
		int[] ret = new int[l];

		for (int i = 0; i < l; ++i){
			ret[i] = radixConvert(36, commandData.Substring(2*i, 2));
		}
		return ret;
	}

	string line2command(string line){
		string command = line.Substring(6);
		command = command.Replace(" ", "").Replace(":", "");
		return command;
	}

	BMS parseBMS(string[] lines){
		BMS bms = new BMS();
		// set BPMs and header
		Dictionary<int,  float> BPMs = new Dictionary<int, float>();
		Regex regexBPMxx = new System.Text.RegularExpressions.Regex(@"^\#BPM[\da-zA-Z][\da-zA-Z]");
		Regex regexBPM = new System.Text.RegularExpressions.Regex(@"^\#BPM[^\da-zA-Z]");
		Regex regexTitle = new System.Text.RegularExpressions.Regex(@"^\#TITLE");
		Regex regexWAVxx = new System.Text.RegularExpressions.Regex(@"^\#WAV[\da-zA-Z][\da-zA-Z]");

		foreach(string line in lines){
			if( regexBPMxx.IsMatch(line) ){
				int key;
				string bpmstr;
				key = radixConvert(36, line.Substring ( 4, 2 ) );
				bpmstr = line.Substring(6);
				float tmpbpm = (float)System.Convert.ToDouble( bpmstr.Replace(" ", "") );
				BPMs.Add(key, tmpbpm);
			}else if( regexTitle.IsMatch (line) ){
				bms.title = line.Substring(7);
			}else if( regexBPM.IsMatch(line) ){
				bms.bpm = line.Substring(4).Replace(" ","");
			}else if( regexWAVxx.IsMatch(line) ){
				int wavid = radixConvert(36, line.Substring ( 4, 2 ) );
				string file = line.Substring(6).Replace(" ","");
				file = System.IO.Path.GetFileNameWithoutExtension(file);
				//Debug.Log ("wav set - wavid:"+wavid+" file:"+file + " wavidstr:" + line.Substring ( 4, 2 ));
				song.setWav(wavid, file);
			}
		}
		Debug.Log("loaded BPMs and other header");

		//create array of measure to start time

		//first, get endmeasure and commands(02-measure length, 03-BPM change, )
		int endmeasure = 0;
		Dictionary<int, float> measureLengths = new Dictionary<int, float>();
		Dictionary<int, string> commandBPMs = new Dictionary<int, string>();
		Regex regexCommand = new System.Text.RegularExpressions.Regex(@"^\#\d\d\d\d\d");
		foreach (string line in lines) {
			int commandID;
			if( regexCommand.IsMatch(line) ){
				int measure = System.Convert.ToInt32( line.Substring(1,3) );
				endmeasure = (endmeasure < measure) ? measure : endmeasure;
				commandID = int.Parse(line.Substring(4,2));

				switch(commandID){
				case 2:
					measureLengths.Add (measure, float.Parse( line2command(line) ));
					break;
				case 3:
					string bpmcommandstr = line.Substring(6);
					commandBPMs.Add (measure, bpmcommandstr);
					break;
				}
			}
		}
		//Debug.Log ("endmeasure:"+endmeasure);
		Debug.Log ("loaded measureLength and BPM change");
		
		float[] m2time = new float[endmeasure + 1];
		float[] timeofmeasure = new float[endmeasure + 1];
		m2time[0] = 0;
		float measurelength;
		float bpm = bms.getBPM();
		float timePerMeasure;

		for (int i = 0; i <= endmeasure; ++i){
			if(!measureLengths.ContainsKey(i)){
				measurelength = 1.0f;
			}else{
				measurelength = measureLengths[i];
			}
			
			timePerMeasure = 0;
			if(!commandBPMs.ContainsKey(i)){
				timePerMeasure =  measurelength * 240.0f / bpm;
			}else{
				int[] bpmids = decodeCommand(commandBPMs[i]);
				int j;
				int bpmid;
				for( j = 0; j < bpmids.Length; ++j ){
					bpmid = bpmids[j];
					if(bpmid != 0){
						bpm = BPMs[ bpmid ];
					}
					timePerMeasure += measurelength * 240.0f / bpm / bpmids.Length;
				}
			}
			timeofmeasure[i] = timePerMeasure;
		}
		m2time[0] = timeofmeasure[0];
		for(int i = 1; i <= endmeasure; ++i){
			m2time[i] = m2time[i-1] + timeofmeasure[i];
		}

		Debug.Log("created array of measure to start time.");

		// perse commands and create note
		
		foreach (string line in lines) {
			int commandID;
			if( regexCommand.IsMatch(line) ){
				int measure = System.Convert.ToInt32( line.Substring(1,3) );
				commandID = int.Parse(line.Substring(4,2));
				int[] items = decodeCommand(line2command(line));
				int itemnum = items.Length;
				//Debug.Log("itemnum:"+itemnum);
				
				float startTime = m2time[measure];

				switch(commandID){
				case 1:	// BGM lane
				case 15:
				case 16:
				case 18:
				case 19:
					for( int i = 0; i < itemnum; ++i){
						//Debug.Log("bgmitem:"+items[i]);
						if(items[i] != 0){
							float dt = i*timeofmeasure[measure]/itemnum;
							createNote(items[i], startTime + dt);

							/*
							if(measure >= 40){
								createNode(items[i], startTime + dt-60);
							}
							*/
							//Debug.Log(line);
							//Debug.Log("item:"+items[i]+" startTime:"+startTime+" dt:"+dt);
						}
					}

					break;
				/**/
				case 11:
				case 12:
				case 13:
				case 14:
				/**/
					int laneid = commandID - 10;
					for( int i = 0; i < itemnum; ++i){
						if(items[i] != 0){
							float dt = i*timeofmeasure[measure]/itemnum;
							GameObject note = createKeyNote(items[i], startTime + dt, laneid);
							song.setKeyNote(laneid, startTime + dt, note);
							testcount++;
							Debug.Log("key note:"+testcount);
						}
					}

					break;
				}
			}
		}

		Debug.Log("created notes.");
		return bms;
	}
}

class BMS{
	public string title;
	public string bpm;

	public string getTitle(){
		return title;
	}
	public float getBPM(){
		Regex reg = new Regex(@"[^\d.]+");
		return float.Parse(reg.Replace(bpm, ""));
	}
}

