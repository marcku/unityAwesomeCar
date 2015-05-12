using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour {
	public Material carMaterial;
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;

	private Slider _hueSlider;
	private Slider _saturationSlider;
	private Slider _luminanceSlider;

	private Slider _distanceSlider;
	private Slider _springSlider;
	private Slider _damperSlider;

	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.Find("HueSlider");
		if (temp != null) {
			_hueSlider = temp.GetComponent<Slider> ();
		}

		temp = GameObject.Find("SaturationSlider");
		if (temp != null) {
			_saturationSlider = temp.GetComponent<Slider> ();
		}


		temp = GameObject.Find("LuminanceSlider");
		if (temp != null) {
			_luminanceSlider = temp.GetComponent<Slider> ();
		}

		temp = GameObject.Find("DistanceSlider");
		if (temp != null) {
			_distanceSlider = temp.GetComponent<Slider> ();
		}

		temp = GameObject.Find("SpringSlider");
		if (temp != null) {
			_springSlider = temp.GetComponent<Slider> ();
		}

		temp = GameObject.Find("DamperSlider");
		if (temp != null) {
			_damperSlider = temp.GetComponent<Slider> ();
		}
		Prefs.Load ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onStartClick() {
		Application.LoadLevel(1);
	}

	public void onMenuClick() {
		Application.LoadLevel(0);
	}

	public void OnSliderHueChanged() {
		Prefs.carBodyHue = _hueSlider.normalizedValue;
		Prefs.SetBodyMaterial (ref carMaterial);
	}
	
	public void OnSliderSaturationChanged() {
		Prefs.carBodySaturation = _saturationSlider.normalizedValue;
		Prefs.SetBodyMaterial (ref carMaterial);
	}
	
	public void OnSliderLuminanceChanged() {
		Prefs.carBodyLuminance = _luminanceSlider.normalizedValue;
		Prefs.SetBodyMaterial (ref carMaterial);
	}

	public void OnSliderDistanceChanged() {
		Prefs.carSuspensionDistance = _distanceSlider.normalizedValue;
		updateWheelSuspension ();
	}
	
	public void OnSliderSpringChanged() {
		Prefs.carSuspensionSpring = _springSlider.value;
		updateWheelSuspension ();
	}
	
	public void OnSliderDamperChanged() {
		Prefs.carSuspensionDamper = _damperSlider.value	;
		updateWheelSuspension ();
	}

	private void updateWheelSuspension() {
		Prefs.SetWheelSuspension (ref wheelFL);
		Prefs.SetWheelSuspension (ref wheelFR);
		Prefs.SetWheelSuspension (ref wheelRR);
		Prefs.SetWheelSuspension (ref wheelRL);
		Prefs.Save();
	}

	void OnApplicationQuit() {
		Prefs.Save();
	}
}
