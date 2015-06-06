using UnityEngine;
using System.Collections;

public class Checkpoints : MonoBehaviour {

	private GuiBehaviour _guiScript;
	private string tempCheckpoint;
	
	void Start () {	
		_guiScript = GameObject.Find ("ScriptContainer").GetComponent<GuiBehaviour> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "PlayerCol") {
			tempCheckpoint = _guiScript.recentCheckpoint;

			if ((tempCheckpoint == "" || tempCheckpoint == "Finished") && this.name == "Checkpoint1") {
				_guiScript.recentCheckpoint = this.name;
				_guiScript.updateCheckpoint1();
				Debug.Log("Checkpoint: " + this.name);
			}

			if (tempCheckpoint == "Checkpoint1" && this.name == "Checkpoint2") {
				_guiScript.recentCheckpoint = this.name;
				_guiScript.updateCheckpoint2();
				Debug.Log("Checkpoint: " + this.name);
			}
			
			if (tempCheckpoint == "Checkpoint2" && this.name == "Checkpoint3") {
				_guiScript.recentCheckpoint = this.name;
				_guiScript.updateCheckpoint3();
				Debug.Log("Checkpoint: " + this.name);
			}
			
			if (tempCheckpoint == "Checkpoint3" && this.name == "Finished") {
				_guiScript.recentCheckpoint = this.name;
				_guiScript.resetCheckpoints();
				Debug.Log("Checkpoint: " + this.name);
			}
		}
	}
}
