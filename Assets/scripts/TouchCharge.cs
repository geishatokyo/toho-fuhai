using UnityEngine;
using System.Collections;

public class TouchCharge : Touchable {

	Master master;
	public static readonly float[] energyPoints = new float[]{0.050f, 0.030f, 0.020f, 0.010f, -0.010f};
	static readonly float comboBonas = 0.0005f;
	
	public int laneID;
	//public static readonly float ATTACK_COST = 0.2f;
	private Attack attackEffect;
	
	public GameObject healEffect;
	public GameObject healMaxEffect;

	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
		if (attackEffect == null) {
			attackEffect = GameObject.Find("Attack/Attack"+laneID).GetComponent<Attack>();
		}
		if (healEffect == null){
			healEffect = GameObject.Find("Heal/HealNormal");
		}
		if (healMaxEffect == null){
			healMaxEffect = GameObject.Find("Heal/HealMax");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void touch(int judge){
		if (judge != -1) {
			float additional = energyPoints [judge] + master.song.combo * comboBonas;
			master.addGauge(additional);
		}
	}

	public override void flick(Vector2 flickVector, int judge){
		//Vector2 thisvec = Camera.main.WorldToScreenPoint(transform.localPosition);
		Vector2 thisvec = transform.localPosition;
		float dot = Vector2.Dot (thisvec, flickVector);

		//Debug.Log (transform.localPosition);
		//Debug.Log (thisvec);
		//Debug.Log (flickVector);
		//Debug.Log (dot);

		if(judge >= 3 || judge == -1){
			return;
		}
		if(dot > 0){
			//Debug.Log("Attack!");
			//master.addGauge(-ATTACK_COST);
			if(master.attack()){
				attackEffect.play();
			}
		}
		if (dot < 0) {
			//Debug.Log("heel!");
			//master.addGauge(-HEAL_COST);
			//master.addLife(HEAL_POINT);
			if(master.healmax()){
				healMaxEffect.particleSystem.Play();
			}else if(master.heal()){
				healEffect.particleSystem.Play();
			}
		}
	}
}
