using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAnimation : MonoBehaviour {

	private IList<Vector3> childposs;
	public Vector3 rotate = new Vector3(0.0001f, 0.0002f, 0.0003f);
	private Quaternion startRotate;

	// Use this for initialization
	void Start () {
		startRotate = transform.localRotation;
		if (childposs == null) {
			childposs = new List<Vector3>();
			
			int c = transform.childCount;
			Transform child;
			for (int i = c-1; i >= 0; --i) {
				childposs.Add(new Vector3());
			}
			for (int i = c-1; i >= 0; --i) {
				child = transform.GetChild(i);
				child.localPosition = Vector3.Normalize(child.localPosition);
				Vector3 lpdash = child.localPosition;
				lpdash.x += 1;
				Vector3 axis = Vector3.Cross(lpdash, child.localPosition);
				//Debug.Log("lpos:"+child.localPosition);
				//Debug.Log("axis:"+axis);
				childposs[i] = axis;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.localRotation = Quaternion.AngleAxis(20f*Time.time, rotate) * startRotate;

		int c = transform.childCount;
		Transform child;
		for (int i = c-1; i >= 0; --i) {
			child = transform.GetChild(i);
			Vector3 pos = child.localPosition;
			Vector3 axis = childposs[i];
			child.localPosition = Quaternion.AngleAxis(2.2f, axis) * pos;
		}
	}
}
