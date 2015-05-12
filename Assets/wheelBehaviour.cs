using UnityEngine;
using System.Collections;

public class wheelBehaviour : MonoBehaviour {
	public WheelCollider wheelCol; // wheel colider object
	// Use this for initialization

	private SkidmarkBehaviour _skidmarks; // skidmark script
	private int _skidmarkLast; // index of last skidmark
	private Vector3 _skidmarkLastPos;// position of last skidmark
	void Start ()
	{
		// Get skidmarks script (not available in sceneMenu)
		GameObject skidmarksGO = GameObject.Find("Skidmarks");
		if (skidmarksGO)
			_skidmarks = skidmarksGO.GetComponent<SkidmarkBehaviour>();
		_skidmarkLast = -1;
	}

	// Update is called once per frame
	void Update ()
	{
		// Get the wheel position and rotation from the wheelcolider
		Quaternion quat;
		Vector3 position;
		wheelCol.GetWorldPose(out position, out quat);
		transform.position = position;

		if (Application.loadedLevel != 0) {
			transform.rotation = quat;
		}

		// if wheel touches the ground: place it on the ground
		WheelHit hit;
		if (wheelCol.GetGroundHit(out hit))
			DoSkidmarking(hit);
	}

	// Creates skidmarks if handbraking
	void DoSkidmarking(WheelHit hit)
	{
		// absolute velocity at wheel in world space
		Vector3 wheelVelo = wheelCol.attachedRigidbody.GetPointVelocity(hit.point);
		if(Input.GetKey("space"))
		{ if (Vector3.Distance(_skidmarkLastPos, hit.point) > 0.1f) {
				_skidmarkLast = _skidmarks.Add(hit.point + wheelVelo*Time.deltaTime,
				                                 hit.normal,
				                                 0.5f,
				                                 _skidmarkLast,
				                               	 wheelCol.name);
				_skidmarkLastPos = hit.point;
			}
		} else _skidmarkLast = -1;
	}
}
