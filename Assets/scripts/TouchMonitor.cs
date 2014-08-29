using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchMonitor : MonoBehaviour {

	class TouchItem{
		public static readonly float FLICK_ALLOW_TIME = 0.1f;
		public static readonly float FLICK_LENGTH = 45;

		public int id;
		public float beganTime;
		public Vector2 beganPosition;

		public GameObject touchObj;

		public int judge;

		public TouchItem(Touch touch, GameObject touchObj, int judge){
			id = touch.fingerId;
			beganTime = Time.time;
			beganPosition = touch.position;

			this.touchObj = touchObj;

			this.judge = judge;
		}

		public bool isFlick(Touch touch){
			if (touch.fingerId != id){
				Debug.Log("'isFlick' argument 'touch' is difficlut item.");
				return false;
			}

			if (Time.time < beganTime + FLICK_ALLOW_TIME) {
				return false;
			}

			float distance = Vector2.Distance(touch.position, beganPosition);
			if (FLICK_LENGTH < distance) {
				return true;
			}

			return false;
		}

	}

	public Song song;
	public Master master;

	private Dictionary<int,TouchItem> touchItems = new Dictionary<int,TouchItem>();

	// Use this for initialization
	void Start () {
		if (song == null){
			song = GameObject.Find("Song").GetComponent<Song>();
		}
		if (master == null){
			master = GameObject.Find("Master").GetComponent<Master>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		int count = Input.touchCount;
		if (count <= 0) {
			return;
		}

		for (int i = 0; i < count; ++i) {
			Touch touch = Input.GetTouch(i);
			int judge;

			switch (touch.phase){
			case TouchPhase.Began:
				Vector3 touchPoint = Camera.main.ScreenToWorldPoint(touch.position);
				Collider2D collider = Physics2D.OverlapPoint(touchPoint);
				GameObject obj = null;

				if(collider){
					obj = collider.transform.gameObject;
					judge = touchBegan(obj);
					touchItems.Add(touch.fingerId, new TouchItem(touch, obj, judge));
				}

				break;

			case TouchPhase.Moved:
				if(touchItems.ContainsKey(touch.fingerId)){
					TouchItem ti = touchItems[touch.fingerId];
					if(ti != null && ti.isFlick(touch)){
						Vector2 v = touch.position - ti.beganPosition;
						touchFlick(ti.touchObj, v, ti.judge);
						touchItems.Remove(touch.fingerId);
					}
				}

				break;

			//case TouchPhase.Stationary:
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				touchItems.Remove(touch.fingerId);
				break;
			}
			/*
			if( touch.phase == TouchPhase.Began ){
				Vector3 touchPoint = Camera.main.ScreenToWorldPoint(touch.position);
				//Debug.Log("touched");
				//Debug.Log(touchPoint);
				Collider2D collider = Physics2D.OverlapPoint(touchPoint);
				if(collider){
					touched(collider.transform.gameObject);
				}
			}
			*/
		}
	}

	 int touchBegan(GameObject touchedObj){
		int id = getLaneID (touchedObj);
		//Debug.Log ("touched "+id+" item");
		/*
		if (id == -1) {
			Debug.Log("other item touched!");
			return;
		}
		*/

		int judge = -1;
		switch(touchedObj.name){
		case "Reset":
			Debug.Log("reset!");
			master.resetAll();
			break;
		case "Start":
			Debug.Log("Start!");
			master.startGame();
			break;
		case "Charge":
		case "Attack":
			judge = song.touch(id);
			touchedObj.GetComponent<Touchable>().touch(judge);
			break;
		}
		return judge;
	}

	void touchFlick(GameObject flickedObj, Vector2 flickVector, int judge){
		//Debug.Log ("flicked!" + flickedObj);
		Touchable obj = flickedObj.GetComponent<Touchable>();
		if (obj != null) {
			obj.flick(flickVector, judge);
		}
	}

	int getLaneID(GameObject panel){
		Transform parent = panel.transform.parent;
		if (parent == null){
			return -1;
		}
		switch (parent.name){
		case "LeftTop":
			return 1;
		case "LeftBottom":
			return 2;
		case "RightBottom":
			return 3;
		case "RightTop":
			return 4;
		default:
			return -1;
		}
	}

}
