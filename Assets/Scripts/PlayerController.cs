using UnityEngine;
using System.Collections;

// All I need is to be struck by your electric love
public class PlayerController : MonoBehaviour {
	
	public float timeBetweenStrokes;
	public float strokeCountdown;
	public float strokeForce;
	public bool swimming;
	private Rigidbody rbody;

	void Start () {
		rbody = GetComponent<Rigidbody>();
		swimming = false;
		Cardboard.SDK.Recenter();
		strokeCountdown = 0;
		StartSwimming();
		
	}
	
	void Update () {
		if( Cardboard.SDK.Triggered ) {
			if( swimming ) {
				swimming = false;
			} else {
				StartSwimming();
			}
		}
		// update oxygen visual effect
		// 
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
		while( strokeCountdown > 0 ) {
			strokeCountdown -= Time.deltaTime;
			yield return 0;
		}
		while( swimming ) {
			// swim sound here
			rbody.AddForce( transform.forward * strokeForce, ForceMode.Impulse );
			//  Debug.Log( transform.forward );
			strokeCountdown = timeBetweenStrokes;
			while( strokeCountdown > 0 ) {
				strokeCountdown -= Time.deltaTime;
				yield return 0;
			}
		}
	}
}
