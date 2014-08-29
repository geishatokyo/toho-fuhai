using UnityEngine;
using System.Collections;

public class TouchAttack : Touchable {

	Master master;
	private readonly float ATTACK_COST = 0.2f;
	public int laneID;
	private Attack effect;

	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
		if (effect == null) {
			effect = GameObject.Find("Attack/Attack"+laneID).GetComponent<Attack>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (master.getGauge() >= ATTACK_COST) {
			renderer.enabled = true;
		} else {
			renderer.enabled = false;
		}
	}

	public override void touch(int judge){
		master.addGauge(-ATTACK_COST);
		effect.play();
	}
}
