using UnityEngine;
using System.Collections;

public class InitSize : MonoBehaviour {
	
	public static readonly float RATIO_NOTEAREA = 0.75f;
	public static readonly float RATIO_TOUCHAREA = 1 - RATIO_NOTEAREA;
	public static readonly float NOTE_WIDTH = 0.2f;
	public static readonly float ROOT3 = 1.73205080757f;
	public static readonly float ROOT2 = 1.41421356237f;

	private Vector3 viewRightBottom;

	void Awake() {
		initArea ();
		initNotes ();
	}

	private void initArea (){
		Camera targetCamera = camera;
		float depth = -targetCamera.transform.localPosition.z;
		Vector3 forwardPosition = targetCamera.transform.forward * depth;
		viewRightBottom = targetCamera.ViewportToWorldPoint (new Vector3 (0, 0, forwardPosition.z));
	}

	// notes are faired hexagon.
	private void initNotes(){
		GameObject obj;

		float length =  -viewRightBottom.y * RATIO_NOTEAREA * ROOT2;
		float additionalLength = NOTE_WIDTH;
		obj = GameObject.Find ("Note1");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (-length/(2*ROOT2), length/(2*ROOT2), 1);
		obj = GameObject.Find ("Note2");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (-length/(2*ROOT2), -length/(2*ROOT2), 1);
		obj = GameObject.Find ("Note3");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (length/(2*ROOT2), -length/(2*ROOT2), 1);
		obj = GameObject.Find ("Note4");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (length/(2*ROOT2), length/(2*ROOT2), 1);
		
		//float length =  -viewRightBottom.y * RATIO_NOTEAREA;
		//float additionalLength = NOTE_WIDTH / ROOT3;
		//float lengthroot3 = length * ROOT3;
		/*
		obj = GameObject.Find ("Note1");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (-lengthroot3 / 4, length * 0.75f , 0);
		obj = GameObject.Find ("Note2");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (-lengthroot3 / 2, 0, 0);
		obj = GameObject.Find ("Note3");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (-lengthroot3 / 4, -length * 0.75f, 0);
		obj = GameObject.Find ("Note4");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (lengthroot3 / 4, -length * 0.75f, 0);
		obj = GameObject.Find ("Note5");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (lengthroot3 / 2, 0, 0);
		obj = GameObject.Find ("Note6");
		obj.transform.localScale = new Vector3 (NOTE_WIDTH, length + additionalLength, 1);
		obj.transform.localPosition = new Vector3 (lengthroot3 / 4, length * 0.75f, 0);
		*/
		Vector3 v;

		obj = GameObject.Find("Energy/EnergyGauge");
		((GameObject)obj).GetComponent<EnergyGauge>().widthMax = length;
		obj = GameObject.Find("Energy/Filter");
		v = obj.transform.localScale;
		obj.transform.localScale = new Vector3(length, length, v.z);
		obj = GameObject.Find("Energy/BG");
		v = obj.transform.localScale;
		obj.transform.localScale = new Vector3(length, length, v.z);
	}

	 /*
	private void initPanels(){
		float ratioTouchAreaW = viewRightBottom.x - viewRightBottom.y * RATIO_NOTEAREA;

		GameObject obj = GameObject.Find ("Panel");


	}
	*/


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
