using UnityEngine;
using System.Collections;

public class WarnBlink : MonoBehaviour {

	public Color mainColor = Color.clear;
	public Color blinkColor = Color.red;
	public bool blink = false;
	public float interval = 1.2f;
	public int blinkCount = 2;
	private float startTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (blink) {
			float time = Time.time - startTime;
			int count = Mathf.FloorToInt(time / interval);
			float ratio = 1 - Mathf.Abs((time - count * interval) / interval  - 0.5f)*2;
			renderer.material.color = Color.Lerp(mainColor, blinkColor, ratio);
			if(count >= blinkCount){
				blink = false;
				renderer.material.color = mainColor;
			}
		} else {
			renderer.material.color = mainColor;
		}
	}

	public void startBlink(){
		startTime = Time.time;
		blink = true;
	}
}
