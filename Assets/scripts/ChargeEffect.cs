using UnityEngine;
using System.Collections;

public class ChargeEffect : MonoBehaviour {

	Master master;
	float startTime;
	Vector3 startPosition;
	Vector3 goalPosition;

	void Awake(){
		startTime = Time.time;
		startPosition = transform.localPosition;
	}

	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		float time = master.song.time;

	}
}
