using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {
	public WheelCollider 	wheelFL;
	public WheelCollider 	wheelFR;
	public WheelCollider 	wheelRL;
	public WheelCollider 	wheelRR;
	public float 			maxTorque = 1500;
	public float			maxBrakeTorque = 5000;
	public GUIText 			guiSpeed;
	public Texture2D        guiSpeedDisplay;
	public Texture2D        guiSpeedPointer;
	private float           currentSpeedKMH;
	public float			maxSpeedKMH = 50;
	public float			maxSpeedBackwardKMH = 30;
	private Rigidbody		_rigidBody;
	private bool			_isFullBraeking;

	public float			fullBrakeTorque = 10000;

	private ParticleSystem _dustR;
	private ParticleSystem _dustL;

	public AudioClip brakeAudioClip;
	private AudioSource _brakeAudioSource;

	public Material brakeLightMaterial;
	public Material backLightMaterial;
	private Material _stdBackLightMaterial;
	public GameObject backLightL;
	public GameObject backLightR;

	private bool velocityIsForeward;
	private bool doBraking;

	// Use this for initialization
	void Start () {
		_rigidBody = GetComponent<Rigidbody>();
		_dustR = GameObject.Find ("dustR").GetComponent<ParticleSystem> ();
		_dustL = GameObject.Find ("dustL").GetComponent<ParticleSystem> ();

		_rigidBody.GetComponent<AudioSource> ().enabled = true;

		_stdBackLightMaterial = backLightL.GetComponent< Renderer>().material;

		_brakeAudioSource = (AudioSource)gameObject.AddComponent<AudioSource> ();
		_brakeAudioSource.clip = brakeAudioClip;
		_brakeAudioSource.loop = true;
		_brakeAudioSource.volume = 0.7f;
		_brakeAudioSource.playOnAwake = false;


		// set suspensions of the wheels
		Prefs.SetWheelSuspension (ref wheelFL);
		Prefs.SetWheelSuspension (ref wheelFR);
		Prefs.SetWheelSuspension (ref wheelRR);
		Prefs.SetWheelSuspension (ref wheelRL);
	}
	
	// Update is called once per frame constanc time per frame
	void FixedUpdate () {
		currentSpeedKMH = _rigidBody.velocity.magnitude * 3.6f;
		guiSpeed.text = currentSpeedKMH.ToString("0");
		
		
		velocityIsForeward = Vector3.Angle(transform.forward, _rigidBody.velocity) < 50f;
		doBraking = ((Input.GetAxis("Vertical") < 0 && velocityIsForeward) || (Input.GetAxis("Vertical") > 0 && !velocityIsForeward));
		
		_isFullBraeking = FullBraking();
		
		
		if (doBraking) {
			wheelFL.brakeTorque = maxBrakeTorque;
			wheelFR.brakeTorque = maxBrakeTorque;
			wheelRR.brakeTorque = maxBrakeTorque;
			wheelRL.brakeTorque = maxBrakeTorque;
			wheelFL.motorTorque = 0;
			wheelFR.motorTorque = 0;
		} else {
			if (!_isFullBraeking){
				wheelFL.brakeTorque = 0;
				wheelFR.brakeTorque = 0;
				wheelRR.brakeTorque = 0;
				wheelRL.brakeTorque = 0;
			}
			if(velocityIsForeward){
				if (currentSpeedKMH < maxSpeedKMH) {
					wheelFL.motorTorque = maxTorque * Input.GetAxis ("Vertical");
					wheelFR.motorTorque = wheelFL.motorTorque;
				}else {
					wheelFL.motorTorque = 0;
					wheelFR.motorTorque = 0;
					_rigidBody.velocity = (maxSpeedKMH/3.6f) * _rigidBody.velocity.normalized;
				}
			} else {
				if (currentSpeedKMH < maxSpeedBackwardKMH) {
					wheelFL.motorTorque = maxTorque * Input.GetAxis ("Vertical");
					wheelFR.motorTorque = wheelFL.motorTorque;
				}else {
					wheelFL.motorTorque = 0;
					wheelFR.motorTorque = 0;
					_rigidBody.velocity = (maxSpeedBackwardKMH/3.6f) * _rigidBody.velocity.normalized;
				}
			}
			
		}
		
		wheelFL.steerAngle = 10 * Input.GetAxis("Horizontal");
		wheelFR.steerAngle = wheelFL.steerAngle;
		
		
		SetBackLights ();
		SetAudioPitch ();
		
		//Debug.Log (Vector3.Angle(transform.forward, _rigidBody.velocity).ToString());
		
	}
	
	// OnGUI is called on every frame when the orthographic GUI is rendered
	void OnGUI() 
	{   GUI.Box(new Rect(0, 0, 140, 140), guiSpeedDisplay);
		GUIUtility.RotateAroundPivot(Mathf.Abs(currentSpeedKMH) + 40, new Vector2(70,70));
		GUI.DrawTexture(new Rect(0, 0, 140, 140), guiSpeedPointer, ScaleMode.StretchToFill);
	}
	
	void SetAudioPitch() {
		float gearSpeedDelta = 30.0f;
		int gear = System.Math.Min(( int)(currentSpeedKMH / gearSpeedDelta), 5);
		float gearSpeedMin = gear * gearSpeedDelta;
		
		if (_isFullBraeking || doBraking) {
			GetComponent< AudioSource>().pitch = 0.4f;
		} else {
			GetComponent< AudioSource>().pitch = (currentSpeedKMH - gearSpeedMin) / gearSpeedDelta * 0.5f + 0.4f;
		}
		
	}
	
	bool FullBraking(){
		if (Input.GetKey("space"))
		{ 
			wheelFL.brakeTorque = fullBrakeTorque;
			wheelFR.brakeTorque = fullBrakeTorque;
			wheelRL.brakeTorque = fullBrakeTorque;
			wheelRR.brakeTorque = fullBrakeTorque;
			wheelFL.motorTorque = 0;
			wheelFR.motorTorque = 0;
			
			if (currentSpeedKMH > 1){
				_dustR.enableEmission = true;
				_dustL.enableEmission = true;
				_brakeAudioSource.Play();
			} else {
				_dustR.enableEmission = false;
				_dustL.enableEmission = false;
				_brakeAudioSource.Stop();
			}
			return true;
		}
		else
		{ 
			wheelFL.brakeTorque = 0;
			wheelFR.brakeTorque = 0;
			wheelRL.brakeTorque = 0;
			wheelRR.brakeTorque = 0;
			_dustR.enableEmission = false;
			_dustL.enableEmission = false;
			_brakeAudioSource.Stop();
			return false;
		}
	}
	
	void SetBackLights() {
		if (_isFullBraeking || (doBraking && currentSpeedKMH > 1)) { 
			//Brake
			backLightL.GetComponent< Renderer>().material = brakeLightMaterial;
			backLightR.GetComponent< Renderer>().material = brakeLightMaterial;
		} else if(!velocityIsForeward && currentSpeedKMH > 1) {
			//Back
			backLightL.GetComponent< Renderer>().material = backLightMaterial;
			backLightR.GetComponent< Renderer>().material = backLightMaterial;
		} else { 
			//Normal (standard)
			backLightL.GetComponent< Renderer>().material = _stdBackLightMaterial;
			backLightR.GetComponent< Renderer>().material = _stdBackLightMaterial;
		}
	}
}