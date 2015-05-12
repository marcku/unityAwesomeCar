using UnityEngine;
using System.Collections;

public class GuiBehaviour : MonoBehaviour {

	public float pastTime = 0;
	public GUIText guiTime;
	public WheelCollider flWheelCol;

	private CarBehaviour _carScript;

	private bool _isFinished = false;

	// Use this for initialization
	void Start () {

		_carScript = GameObject.Find ("COSWORTH").GetComponent<CarBehaviour> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (_carScript.enabled) {
			WheelHit hit;
			if (flWheelCol.GetGroundHit (out hit)) {
		
				if (hit.collider.gameObject.tag == "Finish") {
					_isFinished = true;
				}
				if (!_isFinished)
					pastTime += Time.deltaTime;

				guiTime.text = pastTime.ToString ("0.0");

			}

		}
	}

	public void OnButtonMenuClick() {
	
		Application.LoadLevel (0);
	
	}

}
