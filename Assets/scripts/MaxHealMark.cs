using UnityEngine;
using System.Collections;

public class MaxHealMark : MonoBehaviour {
	
	Master master;
	bool started;
	public GameObject FlashEffect = null;
	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (master.getGauge () >= Master.HEALMAX_COST) {
			if(!started){
				if(FlashEffect != null){
					FlashEffect.particleSystem.Play();
				}
				renderer.enabled = true;

				started = true;
			}
			
		} else {
			renderer.enabled = false;
			started = false;
		}
	}
}
