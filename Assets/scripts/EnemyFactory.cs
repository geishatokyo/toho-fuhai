using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour, IReset {

	Master master;
	GameObject[] warns;
	GameObject enemyPrefab;
	Vector3[] appearPositions;

	List<float> nextTime;
	int nextItem;

	void Awake(){
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
		if (warns == null) {
			warns = new GameObject[4];
			for(int i = 1; i <=4; ++i){
				warns[i-1] = (GameObject)GameObject.Find("Warn/filter"+i);
			}
		}
		if (enemyPrefab == null) {
			enemyPrefab = (GameObject)Resources.Load("DeffensePrefab/Enemy");
		}
		nextTime = new List<float>();
		nextTime.Add (14.0f);
		nextTime.Add (20.0f);
		nextTime.Add (25.0f);
		nextTime.Add (34.0f);
		nextTime.Add (36.0f);
		nextTime.Add (45.0f);
		nextTime.Add (49.0f);
		nextTime.Add (57.0f);
		nextTime.Add (60.0f);
		nextTime.Add (66.0f);
		nextTime.Add (75.0f);
		nextTime.Add (79.0f);
		nextTime.Add (85.0f);
		nextTime.Add (91.0f);
		nextTime.Add (99.0f);
		nextTime.Add (100.0f);
		nextTime.Add (110.0f);
		nextTime.Add (115.0f);
		nextTime.Add (119.0f);
		nextTime.Add (122.0f);
		nextTime.Add (126.0f);
		nextTime.Add (129.0f);
		nextTime.Add (132.0f);
		nextTime.Add (135.0f);
		nextTime.Add (137.0f);

		appearPositions = new Vector3[4];
		appearPositions[0] = new Vector3(-10,7,10);
		appearPositions[1] = new Vector3(-10,-7,10);
		appearPositions[2] = new Vector3(10,-7,10);
		appearPositions[3] = new Vector3(10,7,10);

		reset();
		master.attachIReset(this);
	}

	public void reset(){
		nextItem = 0;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update(){
		if (nextItem >= nextTime.Count) {
			return;
		}
		if (master.song.time >= nextTime[nextItem]) {
			++nextItem;
			createEnemy();
		}
	}

	void createEnemy(){
		int lane = Random.Range(0, 4);

		warns[lane].GetComponent<WarnBlink>().startBlink ();
		Vector3 position = appearPositions[lane];

		GameObject enemyobj = (GameObject)GameObject.Instantiate(enemyPrefab);
		enemyobj.GetComponent<MoveStraight>().setStartPosition(position);

		Debug.Log("Enemy created.");
	}
}
