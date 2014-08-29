using UnityEngine;
using System.Collections;

public class BallTest : MonoBehaviour {

	public Vector3 vel;

	void Awake(){
		rigidbody.AddForce (vel, ForceMode.VelocityChange);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Rotate(0, 5*Time.deltaTime, 0);
	}
}
