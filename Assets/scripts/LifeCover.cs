using UnityEngine;
using System.Collections;

public class LifeCover : MonoBehaviour {
	private Master master;
	public Vector3 lifeMaxPosition;
	public Vector3 lifeMinPosition;
	public float shake;

	void Awake(){
		if (master == null) {
			master = GameObject.Find("Master").GetComponent<Master>();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float ratio = master.getLifeRatio();
		Vector3 newPos = Vector3.Lerp(lifeMinPosition, lifeMaxPosition, ratio);
		newPos.y += Mathf.Sin(Time.time * 3) * shake;

		transform.localPosition = newPos;
	}
}
