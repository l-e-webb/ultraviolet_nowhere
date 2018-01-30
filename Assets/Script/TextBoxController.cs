using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour {

	bool inDialogue = false;

	Transmission transmission;
	int nodeIndex;

	Text fullTextBox;
	Text portraitTextBox;
	Text portraitMetadataBox;
	Image portrait;
	SpriteSwitcher commLightSwitcher;
	ConsoleController console;

	float delay = 0;

	// Use this for initialization
	void Start () {
		fullTextBox = GameObject.Find("FullTextBox").GetComponent<Text>();
		portraitTextBox = GameObject.Find("PortraitTextBox").GetComponent<Text>();
		portraitMetadataBox = GameObject.Find("PortraitMetadataBox").GetComponent<Text>();
		portrait = GameObject.Find("Portrait").GetComponent<Image>();
		commLightSwitcher = GameObject.Find("CommsLight").GetComponent<SpriteSwitcher>();
		console = GameObject.Find("ConsoleControlsCanvas").GetComponent<ConsoleController>();
		EndDialogue();
		UpdateTextDisplay();
	}
	
	// Update is called once per frame
	void Update () {
		if (delay > 0) {
			delay -= Time.deltaTime;
			if (delay < 0) {
				EndDialogue();
			}
		}

		if (!inDialogue)
			return;

		if (Input.GetKeyUp(KeyCode.Return)
			|| Input.GetKeyUp(KeyCode.Space)) {
			AdvanceDialogue();
		}
			
	}

	public void StartDialogue(Transmission transmission) {
		inDialogue = true;
		this.transmission = transmission;
		nodeIndex = -1;
		delay = 0;
		SetTextOneOff(Transmission.transmissionDecoded, true);
	}

	public void EndDialogue() {
		transmission = null;
		nodeIndex = 0;
		inDialogue = false;
		delay = 0;
		commLightSwitcher.SetSprite(1);
		UpdateTextDisplay();
	}

	//Ends dialogue after "delay" seconds.
	public void EndDialogue(float delay) {
		this.delay = delay;
		inDialogue = false;
		commLightSwitcher.SetSprite(1);
	}

	public void SetTextOneOff(string text, bool centerInBox) {
		portraitTextBox.text = "";
		portraitTextBox.enabled = false;
		portraitMetadataBox.text = "";
		portraitMetadataBox.enabled = false;
		portrait.sprite = null;
		portrait.enabled = false;
		fullTextBox.text = text;
		fullTextBox.enabled = true;
		fullTextBox.alignment = centerInBox ? TextAnchor.MiddleCenter : TextAnchor.UpperLeft;
	}

	public void SetTextOneOff(string text) {
		SetTextOneOff(text, false);
	}

	void AdvanceDialogue() {
		nodeIndex++;
		if (nodeIndex >= transmission.nodes.Length) {
			SetTextOneOff(Transmission.transmissionComplete, true);
			EndDialogue(4);
		} else {
			UpdateTextDisplay();
		}
	}

	void UpdateTextDisplay() {
		fullTextBox.text = "";
		fullTextBox.enabled = false;
		fullTextBox.alignment = TextAnchor.UpperLeft;
		portraitTextBox.text = "";
		portraitTextBox.enabled = false;
		portraitMetadataBox.text = "";
		portraitMetadataBox.enabled = false;
		portrait.sprite = null;
		portrait.enabled = false;

		if (!inDialogue) {
			return;
		}

		if (transmission == null
			|| transmission.nodes == null
			|| nodeIndex > transmission.nodes.Length
			|| transmission.nodes[nodeIndex] == null) {
			Debug.Log("Error: attempt to display improper dialogue node of index " + nodeIndex + ".");
			return;
		}

		Transmission.TransmissionNode currentNode = transmission.nodes[nodeIndex];
		if (currentNode.hasPortrait) {
			portraitTextBox.enabled = true;
			portraitTextBox.text = currentNode.text;
			portraitMetadataBox.enabled = true;
			portraitMetadataBox.text = transmission.metaData;
			portrait.enabled = true;
		} else {
			fullTextBox.enabled = true;
			fullTextBox.text = transmission.metaData + "\n\n" + currentNode.text;
		}
	}

	public bool InDialogue() {
		return inDialogue;
	}
}
