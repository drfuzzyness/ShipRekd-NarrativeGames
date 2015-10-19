using UnityEngine;
using System.Collections;

public class ShipMovementController : MonoBehaviour {
	
	public Vector3 impulse;
	
	private Rigidbody rbody;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody>();
		rbody.AddForce( impulse, ForceMode.VelocityChange );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
