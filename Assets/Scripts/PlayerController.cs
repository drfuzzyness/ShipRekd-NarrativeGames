using UnityEngine;
using System.Collections;

// All I need is to be struck by your electric love
public class PlayerController : MonoBehaviour {
	
	public float timeBetweenStrokes;
	public float strokeForce;
	public float oxygenTime;
	private bool swimming;
	private Rigidbody rbody;

	void Start () {
		rbody = GetComponent<Rigidbody>();
		swimming = false;
		Cardboard.SDK.Recenter();
		StartSwimming();
		
	}
	
	void Update () {
		oxygenTime -= Time.deltaTime;
		if( oxygenTime < 0 ) {
			// gameover
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
		while( swimming ) {
			// swim sound here
			rbody.AddForce( transform.forward * strokeForce, ForceMode.Impulse );
			//  Debug.Log( transform.forward );
			yield return new WaitForSeconds( timeBetweenStrokes );
		}
	}
}
