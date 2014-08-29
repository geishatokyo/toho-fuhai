using UnityEngine;
using System.Collections;

public class EnergyBG : MonoBehaviour, IReset {
	Master master;
	Color defaultColor;
	public Color maxColor;
	public Color DamageColor;
	Color oldColor;
	Color newColor;
	Color nowColor;

	float startTime;
	float changeTime = 1.5f;
	int lastLife;

	public void reset(){
		lastLife = Master.MAX_LIFE;
	}
	
	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
			defaultColor = renderer.material.color;
			master.attachIReset(this);
			lastLife = Master.MAX_LIFE;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (master.getGauge () > Master.HEALMAX_COST) {
			nowColor = maxColor;
		} else {
			nowColor = defaultColor;
		}
		oldColor = newColor = nowColor;

		if (lastLife != master.getLife ()) {
			startTime = Time.time;
			lastLife = master.getLife();
			oldColor = DamageColor;
			newColor = nowColor;
		}

		float time = Time.time - startTime;
		float ratio = time - Mathf.Floor(time/changeTime)*changeTime;
		ratio = (time > changeTime) ? 1 : ratio;
		//Debug.Log ("ratio:"+ratio);

		renderer.sharedMaterial.color = Color.Lerp (oldColor, nowColor, ratio);

	}

}
