using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	static Master master;
	static GameObject burst;
	public int attackPoint = 200;
	
	void Awake(){
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
		if (burst == null) {
			burst = (GameObject)Resources.Load("DeffensePrefab/Burst");
		}
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (!master.song.playing) {
			Object.Destroy(gameObject);
		}
	}

	/*
	void OnParticleCollision(GameObject obj){
		Debug.Log("ParticleCollision with "+obj.name);

		GameObject.Instantiate(burst, transform.localPosition, burst.transform.localRotation);
		Destroy(this);
	}
	*/

	void OnTriggerEnter(Collider obj){
		//Debug.Log("Collision with "+obj.tag);

		switch (obj.tag){
		case "Attack":
			obj.GetComponent<MoveStraight>().setStartPosition(new Vector3(100,100,100));
			GameObject.Instantiate(burst, transform.localPosition, burst.transform.localRotation);
			Object.Destroy(gameObject);

			++master.record[Master.RECORD_HIT];
			break;

		case "Life":
			GameObject.Instantiate(burst, transform.localPosition, burst.transform.localRotation);
			Object.Destroy(gameObject);
			master.addLife(-attackPoint);
			++master.record[Master.RECORD_DAMAGE];
			break;

		default:
			Debug.Log("unknown collider item");
			break;
		}


	}
}
