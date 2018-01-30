using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transmitter : MonoBehaviour {

	public bool hasText;
	public bool hasAudio;
	public TextAsset textAsset;
	public FrequencyBand band;
	public float tuning;
	public float minVolume;
	public float maxVolume;
	public float proximityTrigger;
	public float advanceDelay;

	Transmission transmission;
	TextBoxController controller;
	AudioSource audioSource;
	CircleCollider2D circleCollider;
	SpriteSwitcher commLightSwitcher;
	ConsoleController console;
	Image signalBar;


	bool transmitting;
	float timer = 0;
	float proximity;
	float distance;
	float radius;

	void Start () {
		controller = GameObject.Find("TextCanvas").GetComponent<TextBoxController>();
		if (hasText) {
			transmission = Transmission.ParseMessageText(textAsset.text);
		} 
		if (hasAudio) {
			audioSource = GetComponent<AudioSource>();
		}
		circleCollider = GetComponent<CircleCollider2D>();
		commLightSwitcher = GameObject.Find("CommsLight").GetComponent<SpriteSwitcher>();
		console = GameObject.Find("ConsoleControlsCanvas").GetComponent<ConsoleController>();
		signalBar = GameObject.Find("SignalBar").GetComponent<Image>();
	}

	void Update () {
		if (!transmitting)
			return;

		UpdateProximity(distance);

		if (hasAudio) {
			audioSource.volume = minVolume + proximity * (maxVolume - minVolume);
		}
		if (hasText && proximity > proximityTrigger) {
			Debug.Log("Starting dialogue.");
			controller.StartDialogue(transmission);
			transmitting = false;
		}
		timer += Time.deltaTime;
		if (hasText && timer > advanceDelay) {
			Debug.Log(proximity);
			timer = 0;
			PushNonsenseText();
		}
	}

	void OnTriggerEnter2D(Collider2D triggerCollider) {
		Debug.Log(gameObject.name + " colliding with trigger.");
		transmitting = true;
		commLightSwitcher.SetSprite(2);
		timer = 0;
		distance = Vector3.Distance(circleCollider.transform.position, triggerCollider.transform.position);
		Debug.Log(distance);
		UpdateProximity(distance);
		Debug.Log(proximity);
		if (hasText) {
			controller.SetTextOneOff(Transmission.incomingTransmission, true);
		}
		if (hasAudio) {
			audioSource.volume = minVolume + proximity * (maxVolume - minVolume);
			audioSource.Play();
			if (!hasText) {
				controller.SetTextOneOff(Transmission.streamingAudioTransmission, true);
			}
		}
	}

	void OnTriggerStay2D(Collider2D triggerCollider) {
		if (!transmitting)
			return;
		
		distance = Vector3.Distance(circleCollider.transform.position, triggerCollider.transform.position);
	}

	void OnTriggerExit2D(Collider2D triggerCollider) {
		Debug.Log(gameObject.name + " no longer colliding with trigger.");
		//If the player reads a whole transmission, "transmitting" will be false,
		//and we do not want to show a "transmission" lost message when they leave
		//the area in that acse.
		if (transmitting) {
			controller.SetTextOneOff(Transmission.transmissionLost, true);
			controller.EndDialogue(advanceDelay);
		}
		if (hasAudio) {
			audioSource.Stop();
		}
		timer = 0;
		transmitting = false;
		commLightSwitcher.SetSprite(1);
		signalBar.fillAmount = 0;
	}

	void UpdateProximity(float distance) {
		float radius = circleCollider.radius * transform.lossyScale.x;
		float distanceComponent = (radius - distance) / radius;
		float frequencyComponent;
		int bandDistance = ConsoleController.BandDistance(band, console.band);
		switch (bandDistance) {
		case 0:
			//Frequencies are on the same band.
			frequencyComponent = 0.5f + 0.5f * (1 - Mathf.Abs(tuning - console.tuning));
			break;
		case 1:
			//Frequencies are on adjacent bands.
			frequencyComponent = 0.5f;
			break;
		case 2:
			//Frequencies are on semi-adjacent bands.
			frequencyComponent = 0.25f;
			break;
		case 3:
			//Frequencies are 3 bands apart.
			frequencyComponent = 0.1f;
			break;
		default:
			//Otherwise.
			frequencyComponent = 0;
			break;
		}
		proximity = 0.05f + 0.30f * distanceComponent + 0.65f * frequencyComponent;
		if (proximity > 1)
			proximity = 1;
		signalBar.fillAmount = proximity;
	}

	void PushNonsenseText() {
		Debug.Log("Pushing scrambled text.");
		string text = transmission.metaData + "\n\n" +
			transmission.nodes[Random.Range(0, transmission.nodes.Length)].text;
		int iterations = (int)((1 - proximity) * 15);
		for (int i = 0; i < iterations; i++) {
			text = ScrambleText(text);
		}
		controller.SetTextOneOff(text);
	}

	static string ScrambleText(string text) {
		int scramble = Random.Range(0, 6);
		int startIndex;
		int length;
		string replacement;
		char[] randomChars;
		switch (scramble) {
		//Randomly replace some characters with "--.....--".  Twice as likely as other edits.
		case 0:
		case 1:
		default:
			startIndex = Random.Range(0, text.Length - 10);
			length = Random.Range(2, 10);
			replacement = "--.....--";
			text = text.Replace(text.Substring(startIndex, length), replacement);
			break;
			//Randomly replace some characters with random ones.
		case 2:
			startIndex = Random.Range(0, text.Length - 10);
			length = Random.Range(2, 10);
			randomChars = new char[Random.Range(0, 10)];
			for (int i = 0; i < randomChars.Length; i++) {
				randomChars[i] = (char)Random.Range(0, 255);
			}
			replacement = new string(randomChars);
			text = text.Replace(text.Substring(startIndex, length), replacement);
			break;
			//Randomly move a substring somewhere else.
		case 3:
			startIndex = Random.Range(0, text.Length - 10);
			length = Random.Range(2, 10);
			replacement = text.Substring(startIndex, length);
			text = text.Replace(replacement, "");
			text = text.Insert(Random.Range(0, text.Length), replacement);
			break;
			//Replace all instances of one character with a random one.
		case 4:
			char randomCharacter = text[Random.Range(0, text.Length)];
			text = text.Replace(randomCharacter, (char)Random.Range(0, 255));
			break;
		case 5:
			randomChars = new char[Random.Range(0, 10)];
			for (int i = 0; i < randomChars.Length; i++) {
				randomChars[i] = (char)Random.Range(0, 255);
			}
			replacement = new string(randomChars);
			text = text.Insert(Random.Range(0, text.Length), replacement);
			break;
		}
		return text;
	}
}
