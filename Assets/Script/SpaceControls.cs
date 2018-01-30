using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceControls : MonoBehaviour {

	public float leftEdge;
	public float rightEdge;
	public float topEdge;
	public float bottomEdge;
	public float speed;
    public FrequencyBand frequencyBand;

	TextBoxController textBoxController;
	AudioSource motorSound;

	void Start () {
		textBoxController = GameObject.Find("TextCanvas").GetComponent<TextBoxController>();
		motorSound = GetComponent<AudioSource>();
	}

	void Update () {
		if (textBoxController.InDialogue())
			return;

		float x = -Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		float y = -Input.GetAxis("Vertical") * Time.deltaTime * speed;
		if (x == 0 && y == 0) {
			motorSound.Stop();
			return;
		}
		transform.Translate(new Vector3(x, y, 0));
		Vector3 position = transform.position;
		if (position.x > rightEdge) {
			position.x = leftEdge + 0.01f;
		} else if (position.x < leftEdge) {
			position.x = rightEdge - 0.01f;
		}
		if (position.y > topEdge) {
			position.y = bottomEdge + 0.01f;
		} else if (position.y < bottomEdge) {
			position.y = topEdge - 0.01f;
		}
		transform.position = position;
		if (!motorSound.isPlaying)
			motorSound.Play();
	}
}
