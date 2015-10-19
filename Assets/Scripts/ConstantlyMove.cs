using UnityEngine;
using System.Collections;

public class ConstantlyMove : MonoBehaviour {
	
	public Vector3 velocity;
	public Space space;
	void FixedUpdate() {
		transform.Translate( velocity * Time.fixedDeltaTime, space );
	}
}
