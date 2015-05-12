using UnityEngine;
using System.Collections;

public class StartCountdown : MonoBehaviour {
	public int countMax = 3;
	private int _countDown;
	private CarBehaviour _carScript;
	public GUIText guiCountdown;


	// Use this for initialization
	void Start() { 
		_carScript = GameObject.Find("COSWORTH").GetComponent<CarBehaviour>();

		Debug.Log ("Car script:" + _carScript);
		Debug.Log("Begin Start:" + Time.time);
		StartCoroutine(GameStart());

		Debug.Log("End Start:" + Time.time);
	}

	// GameStart CoRoutine
	IEnumerator GameStart() { 
		Debug.Log (" Begin GameStart:" + Time.time);
		for (_countDown = countMax; _countDown > 0; _countDown--) { 
			yield return new WaitForSeconds (1);
			Debug.Log (" WaitForSeconds:" + Time.time);
		}

		guiCountdown.enabled = false;

		// enable script
		_carScript.enabled = true;

		Debug.Log (" End GameStart:" + Time.time);
	}

	void OnGUI() {
		guiCountdown.text = _countDown.ToString();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
