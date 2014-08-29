using UnityEngine;
using System.Collections;

public class HealMark : MonoBehaviour {
	Master master;
	static Material on;
	static Material off;
	bool started;
	public GameObject FlashEffect = null;
	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
		if (on == null) {
			on = (Material)Resources.Load("DeffenseMat/Default", typeof(Material));
		}
		if (off == null) {
			off = (Material)Resources.Load("DeffenseMat/Gray", typeof(Material));
		}
	}

	// Update is called once per frame
	void Update () {
		if (master.getGauge () >= Master.HEAL_COST) {
			//renderer.enabled = true;
			if(!started){
				renderer.material = on;
				if(FlashEffect != null){
					FlashEffect.particleSystem.Play();
				}
				started = true;
			}

		} else {
			//renderer.enabled = false;
			renderer.material = off;
			started = false;
		}
	}
}
