using UnityEngine;
using System.Collections;

public class MoveStraight : MonoBehaviour {

	float startTime;
	public float moveTime;

	Vector3 startPosition;
	public Vector3 goalPosition;

	void Awake(){
		startTime = Time.time;
		startPosition = transform.localPosition;
		Debug.Log ("MoveStraight :"+gameObject.name);
	}

	public void setStartPosition(Vector3 start){
		transform.localPosition = startPosition = start;
		startTime = Time.time;
	}


	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		float time = Time.time - startTime;
		float ratio = time / moveTime;

		transform.localPosition = Vector3.Lerp (startPosition, goalPosition, ratio);
	}
}
