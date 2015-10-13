using UnityEngine;
using System.Collections;

// All I need is to be struck by your electric love
public class PlayerController : MonoBehaviour {
	
	public float timeBetweenStrokes;
	public float strokeForce;
	private bool swimming = false;
	private Rigidbody rbody;

	void Start () {
		Cardboard.SDK.Recenter();
		StartSwimming();
		rbody = GetComponent<Rigidbody>();
	}
	
	void Update () {
		
	}
	
	void FixedUpdate() {
		//  Head tracking in one line
		//  rbody.MoveRotation( Cardboard.SDK.HeadRotation );
		transform.rotation = Cardboard.SDK.HeadPose.Orientation;
	}
	
	void StartSwimming() {
		swimming = true;
		StartCoroutine( SwimRoutine() );
	}
	
	IEnumerator SwimRoutine() {
		while( swimming ) {
			// swim sound here
			GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * strokeForce, ForceMode.Impulse );
			//  Debug.Log( transform.forward );
			yield return new WaitForSeconds( timeBetweenStrokes );
		}
	}
}
