using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleController : MonoBehaviour {

	static FrequencyBand[] bands = new FrequencyBand[] {
		FrequencyBand.RADIO_WAVE,
		FrequencyBand.INFRARED,
		FrequencyBand.VISIBLE,
		FrequencyBand.ULTRAVIOLET,
		FrequencyBand.X_RAY,
		FrequencyBand.GAMMA_RAY
	};
	static string[] bandNames = new string[] {
		"Radio wave",
		"Infrared",
		"Visible",
		"Ultraviolet",
		"X-Ray",
		"Gamma Ray"
	};

	public FrequencyBand band;
	int bandIndex;
	public float tuning;

	Slider tuningSlider;
	Text frequencyRangeIndicator;

	void Start () {
		band = FrequencyBand.VISIBLE;
		bandIndex = 2;
		tuning = 0.5f;
		tuningSlider = GameObject.Find("ConsoleSlider").GetComponent<Slider>();
		frequencyRangeIndicator = GameObject.Find("FBandIndicatorText").GetComponent<Text>();
	}

	public void UpdateAll() {
		tuning = tuningSlider.value;
		band = bands[bandIndex];
		frequencyRangeIndicator.text = bandNames[bandIndex];
		//TODO: update shit in space.
	}

	public void BandLeft() {
		if (bandIndex > 0) {
			bandIndex--;
			UpdateAll();
		} else {
			//TODO: sound effect??
		}
	}

	public void BandRight() {
		if (bandIndex < bands.Length - 1) {
			bandIndex++;
			UpdateAll();
		} else {
			//TODO: sound effect
		}
	}

	public static int BandDistance(FrequencyBand band1, FrequencyBand band2) {
		return Mathf.Abs(BandIndex(band1) - BandIndex(band2));
	}

	public static int BandIndex(FrequencyBand band) {
		for (int i = 0; i < bands.Length; i++) {
			if (band == bands[i])
				return i;
		}
		return -1;
	}

}
