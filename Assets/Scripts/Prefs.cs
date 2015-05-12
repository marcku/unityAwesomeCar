using UnityEngine;
using System.Collections;
using UnityEditor;

public class Prefs : MonoBehaviour {
	public static float carBodyHue;
	public static float carBodySaturation;
	public static float carBodyLuminance;

	public static float carSuspensionDistance;
	public static float carSuspensionSpring;
	public static float carSuspensionDamper;

	private static JointSpring spring;

	public static void SetBodyMaterial(ref Material bodyMat) {
		bodyMat.color = EditorGUIUtility.HSVToRGB(carBodyHue,
		                                          carBodySaturation,
		                                          carBodyLuminance);
	}

	public static void SetWheelSuspension(ref WheelCollider wheelCol) {
		spring = new JointSpring ();
		spring.damper = carSuspensionDamper;
		spring.spring = carSuspensionSpring;
		spring.targetPosition = carSuspensionDistance;

		wheelCol.suspensionSpring = spring;
	}

	public static void Load() {
		carBodyHue = PlayerPrefs.GetFloat("carBodyHue", 0.0f);
		carBodySaturation = PlayerPrefs.GetFloat("carBodySaturation", 1.0f);
		carBodyLuminance = PlayerPrefs.GetFloat("carBodyLuminance", 1.0f);

		carSuspensionDistance = PlayerPrefs.GetFloat ("carSuspensionDistance", 0.5f);
		carSuspensionSpring = PlayerPrefs.GetFloat ("carSuspensionSpring", 35000.0f);
		carSuspensionDamper = PlayerPrefs.GetFloat ("carSuspensionDamper", 4500f);
	}
	public static void Save() {
		PlayerPrefs.SetFloat("carBodyHue", carBodyHue);
		PlayerPrefs.SetFloat("carBodySaturation", carBodySaturation);
		PlayerPrefs.SetFloat("carBodyLuminance", carBodyLuminance);

		PlayerPrefs.SetFloat ("carSuspensionDistance", carSuspensionDistance);
		PlayerPrefs.SetFloat ("carSuspensionSpring", carSuspensionSpring);
		PlayerPrefs.SetFloat ("carSuspensionDamper", carSuspensionDamper);
	}
}
