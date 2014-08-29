using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {
	public Vector3 direction;
	public bool shakeing;
	public float interval = 1.0f;

	public Vector3 position;

	void Awake(){
		//if (position == null) {
			position = transform.localPosition;
		//}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (shakeing || interval != 0) {
			float time = Time.time * 2 * Mathf.PI / interval;
			transform.localPosition = position + direction * Mathf.Sin(time);
		}
	}
}
