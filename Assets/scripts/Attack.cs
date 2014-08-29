using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	public GameObject effectCollider;
	private MoveStraight collidermove;

	void Awake(){
		if (collidermove == null) {
			collidermove = effectCollider.GetComponent<MoveStraight>();
		}
		//Debug.Log ("effectCollider:"+effectCollider);
		//Debug.Log ("move straight:" + effectCollider.GetComponent<MoveStraight> ());

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void play(){
		collidermove.setStartPosition(transform.localPosition);
		particleSystem.Play();
	}

	/*
	void OnParticleCollision(GameObject obj){
		Debug.Log("ParticleCollision");
		if (obj.name != "Enemy") {
			Debug.Log("Attack particle collision with '"+obj.name+"'. It is unknown.");
		}

		Vector3 pos = obj.transform.localPosition;
		GameObject.Instantiate(burst, pos, burst.transform.localRotation);
		Destroy(obj);
	}
	*/
}
