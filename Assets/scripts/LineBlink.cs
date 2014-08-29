using UnityEngine;
using System.Collections;

public class LineBlink : MonoBehaviour {

	public Song song;
	public bool blink = true;
	public float interval = 1.0f;
	public float blinkTime = 1.0f;
	public Color lightColor = Color.white;

	private Material mat;
	private Color originalColor;

	// Use this for initialization
	void Start () {
		if(mat == null) {
			mat = gameObject. renderer.material;
			originalColor = mat.color;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (blink){
			blinkMat ();
		}
	}

	void blinkMat(){
		float time = song.time - Mathf.Floor ((song.time + blinkTime/2) / interval)*interval;
		if(time > blinkTime){
			mat.color = originalColor;
			return;
		}

		float ratio = time/blinkTime - 0.5f;
		ratio += ratio;
		//ratio : -1 ~ 1
		Mathf.Abs(ratio);
		ratio = 1 - ratio;

		mat.color = Color.Lerp(originalColor, lightColor, ratio);
	}
}
