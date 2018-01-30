using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour {

	public Sprite sprite1;
	public Sprite sprite2;

	SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Switch() {
		if (spriteRenderer.sprite == sprite1) {
			spriteRenderer.sprite = sprite2;
		} else {
			spriteRenderer.sprite = sprite1;
		}
	}

	public void SetSprite(int sprite) {
		if (spriteRenderer == null)
			return;
		spriteRenderer.sprite = sprite == 1 ? sprite1 : sprite2;
	}
}
