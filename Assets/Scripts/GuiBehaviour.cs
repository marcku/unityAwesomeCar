using UnityEngine;
using System.Collections;

public class GuiBehaviour : MonoBehaviour {

	public float pastTime = 300;
	public GUIText guiTime;
	public WheelCollider flWheelCol;

	//Checkpoints
	public GUIText checkPoint1Text;
	public GUIText checkPoint2Text;
	public GUIText checkPoint3Text;
	public GUIText checkPoint1Value;
	public GUIText checkPoint2Value;
	public GUIText checkPoint3Value;
	public GUIText lapsText;

	public int laps = 0;
	public string recentCheckpoint;

	private CarBehaviour _carScript;
	private float _timeToBeat = 300;
	private bool _isFinished = false;
	private string _textTimeToBeat;
	private int _laps = 0;

	// Use this for initialization
	void Start() {
		_carScript = GameObject.Find ("COSWORTH").GetComponent<CarBehaviour> ();
	}
	
	// Update is called once per frame
	void Update() {
		if (_carScript.enabled) {
			_timeToBeat -= Time.deltaTime;
			_textTimeToBeat = _timeToBeat.ToString ("0.0");
			guiTime.text = "Time to beat: " + _textTimeToBeat;
		}
	}

	public void OnButtonMenuClick() {
		Application.LoadLevel (0);
	}

	public void updateCheckpoint1() {
		checkPoint1Text.enabled = true;
		checkPoint1Value.text = _textTimeToBeat;
		checkPoint1Value.enabled = true;
	}

	public void updateCheckpoint2() {
		checkPoint2Text.enabled = true;
		checkPoint2Value.text = _textTimeToBeat;
		checkPoint2Value.enabled = true;
	}

	public void updateCheckpoint3() {
		checkPoint3Text.enabled = true;
		checkPoint3Value.text = _textTimeToBeat;
		checkPoint3Value.enabled = true;
	}

	public void resetCheckpoints() {
		checkPoint1Text.enabled = false;
		checkPoint2Text.enabled = false;
		checkPoint3Text.enabled = false;

		checkPoint1Value.enabled = false;
		checkPoint2Value.enabled = false;
		checkPoint3Value.enabled = false;

		_laps++;

		lapsText.text = "Laps: " + _laps;
	}
}