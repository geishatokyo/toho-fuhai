using UnityEngine;
using System.Collections;

public class AttackMark : MonoBehaviour {
	Master master;
	static Material on;
	static Material off;
	float startTime;
	public float flashTime = 0.4f;
	public Color flashColor = Color.white;
	Color onDefaultColor;
	bool started;
	public GameObject FlashEffect = null;
	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
		if (on == null) {
			on = (Material)Resources.Load("DeffenseMat/Default", typeof(Material));
			onDefaultColor = on.color;
		}
		if (off == null) {
			off = (Material)Resources.Load("DeffenseMat/Gray", typeof(Material));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (master.getGauge () >= Master.ATTACK_COST) {
			//renderer.enabled = true;
			if(started){
				/*
				if(startTime + flashTime > Time.time){
					float ratio = Mathf.Abs(((startTime - Time.time) * 3 / flashTime)+1.5f)*2/3;
					renderer.sharedMaterial.color = Color.Lerp(onDefaultColor, flashColor, ratio);
				}
				*/
			}else{
				renderer.material = on;
				//startTime = Time.time;
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
