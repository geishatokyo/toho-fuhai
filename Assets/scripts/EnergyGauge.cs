using UnityEngine;
using System.Collections;

public class EnergyGauge : MonoBehaviour {

	Master master;
	public float widthMax;
	public Color gaugeNormalColor = Color.blue;
	public Color gaugeMaxColor = Color.yellow;
	//public float startSizeBase = 1;


	// Use this for initialization
	void Start () {
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		float gauge = master.getGauge ();
		Vector3 v = transform.localScale;
		v.x = v.y = widthMax * gauge;
		transform.localScale = v;
		//Debug.Log("widthmax:"+widthMax + " gauge:"+gauge + " v:"+v);

		if (gauge >= 1){
			particleSystem.startColor = gaugeMaxColor;
			//particleSystem.startSize = startSizeBase;
		} else {
			particleSystem.startColor = gaugeNormalColor;
			//particleSystem.startSize = startSizeBase * (gauge * 0.7f + 0.3f);
			//Debug.Log("gauge:"+gauge+" maxpar...:"+particleSystem.maxParticles);
		}
	}

}
