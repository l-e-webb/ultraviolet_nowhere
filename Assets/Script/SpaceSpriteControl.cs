using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSpriteControl : MonoBehaviour {

    public bool overlay = false;
    public float multiplier;
    public FrequencyBand localFrequencyBand;
    public SpriteRenderer spriteRenderer;
    public Sprite frequencySprite;
    public ConsoleController consoleController;
    //public List<Sprite> frequencySprites;


   // public string spriteName;


    public Sprite GammaSprite;
    public Sprite GammaSpriteHigh;
    public Sprite XraySprite;
    public Sprite XraySpriteHigh;
    public Sprite UltravioletSprite;
    public Sprite UltravioletSpriteHigh;
    public Sprite VisibleSprite;
    public Sprite VisibleSpriteHigh;
    public Sprite InfraredSprite;
    public Sprite InfraredSpriteHigh;
    public Sprite RadioSprite;
    public Sprite RadioSpriteHigh;
 	// Use this for initialization
	void Start ()
    {
        consoleController = FindObjectOfType<ConsoleController>();
        //spaceParent = this.transform.parent.GetComponent<SpaceControls>();
        //localFrequencyBand = spaceParent.frequencyBand;

        spriteRenderer = this.GetComponent<SpriteRenderer>();
        frequencySprite = spriteRenderer.sprite;

        //frequencySprites = new List<Sprite>();
        //frequencySprites.AddRange(Resources.LoadAll<Sprite>("Environment/frequencyBands/"));
        

    }
	
	// Update is called once per frame
	void Update ()
    {
        localFrequencyBand = consoleController.band;
        multiplier = consoleController.tuning;

        if (overlay)
        {
            Color overlayColor = spriteRenderer.color;
            float alpha = overlayColor.a;
            overlayColor.a = alpha * multiplier;
            spriteRenderer.color = overlayColor;

            switch (localFrequencyBand)
            {

                case FrequencyBand.GAMMA_RAY:
                    frequencySprite = GammaSpriteHigh;
                    break;

                case FrequencyBand.X_RAY:
                    frequencySprite = XraySpriteHigh;
                    break;

                case FrequencyBand.ULTRAVIOLET:
                    frequencySprite = UltravioletSpriteHigh;
                    break;

                case FrequencyBand.VISIBLE:
                    frequencySprite = VisibleSpriteHigh;
                    break;

                case FrequencyBand.INFRARED:
                    frequencySprite = InfraredSpriteHigh;
                    break;

                case FrequencyBand.RADIO_WAVE:
                    frequencySprite = RadioSpriteHigh;
                    break;

            }

        }

        else
        {
            switch (localFrequencyBand)
            {

                case FrequencyBand.GAMMA_RAY:
                    frequencySprite = GammaSprite;
                    break;

                case FrequencyBand.X_RAY:
                    frequencySprite = XraySprite;
                    break;

                case FrequencyBand.ULTRAVIOLET:
                    frequencySprite = UltravioletSprite;
                    break;

                case FrequencyBand.VISIBLE:
                    frequencySprite = VisibleSprite;
                    break;

                case FrequencyBand.INFRARED:
                    frequencySprite = InfraredSprite;
                    break;

                case FrequencyBand.RADIO_WAVE:
                    frequencySprite = RadioSprite;
                    break;
        
            }

        }

        spriteRenderer.sprite = frequencySprite;
	}
}
