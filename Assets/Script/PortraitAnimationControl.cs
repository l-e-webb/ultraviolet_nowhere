using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitAnimationControl : MonoBehaviour {

    public float averageGlitchDelay;
    public float glitchDelayVariation;

    SpriteRenderer spriteRenderer;
    Animator anim;

    int glitchDelayId = Animator.StringToHash("Glitch Delay");

	void Start() {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Disable();
	}
	
	// Update is called once per frame
	void Update() {
        if (!anim.enabled) return;

        float glitchDelay = anim.GetFloat(glitchDelayId);
        if (glitchDelay < 0) {
            glitchDelay = NewGlitchDelay();
        }

        glitchDelay -= Time.deltaTime;
        anim.SetFloat(glitchDelayId, glitchDelay);
	}

    float NewGlitchDelay() {
        return averageGlitchDelay * (1 + Random.Range(-glitchDelayVariation, glitchDelayVariation));
    }

    public bool SetPortrait(string portraitKey) {
        int idleState = Animator.StringToHash(portraitKey + "Idle");
        if (!anim.HasState(0, idleState)) {
            Debug.Log("Attempting to activate invalid portrait " + portraitKey);
            return false;
        }
        anim.enabled = true;
        spriteRenderer.enabled = true;
        anim.CrossFadeInFixedTime(idleState, 0);
        anim.SetFloat(glitchDelayId, NewGlitchDelay());
        return true;
    }

    public void Disable() {
        anim.enabled = false;
        spriteRenderer.enabled = false;
    }

}
