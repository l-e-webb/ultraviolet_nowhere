using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PortraitController : MonoBehaviour {

    private static string[] portraitKeys =
    {
        "GravityWong",
        "Hester",
        "Alleline",
        "Wei"
    };

    public string portraitDirectory;
    
    Dictionary<string, Sprite[]> portraitMap;
    Image portraitImage;
    Sprite[] currentPortraitArray;
	
	void Start() {
        //Build portrait database.
        Sprite[] portraits = Resources.LoadAll<Sprite>(portraitDirectory);
        Debug.Log("Found " + portraits.Length + " portrait sprites in " + portraitDirectory);
        portraitMap = new Dictionary<string, Sprite[]>();
        for (int i = 0; i < portraitKeys.Length; i++) {
            //Determine appropriate key.
            string key = portraitKeys[i];
            //Find number of portraits that begin with this key.
            int numPortraits = 0;
            for (int j = 0; j < portraits.Length; j++) {
                if (portraits[j].name.StartsWith(key)) {
                    numPortraits++;
                }
            }
            if (numPortraits == 0) continue;
            Sprite[] keyPortraits = new Sprite[numPortraits];
            //Add base portrait with this key.
            keyPortraits[0] = findSpriteByName(key, portraits);
            //Add alternate frames based on this key, of the form {Key}Frame{n}
            for (int j = 1; j < numPortraits; j++) {
                keyPortraits[j] = findSpriteByName(key + "Frame" + j, portraits);
            }
            //Add Sprite array to dictionary.
            portraitMap.Add(key, keyPortraits);
        }

        portraitImage = GetComponent<Image>();
	}
	
	void Update() {
        if (!portraitImage.enabled) return;

        //TODO: update portrait frame.
	}

    public void SetPortrait(string key) {
        if (!portraitMap.ContainsKey(key)) {
            Debug.Log("Attempting to specify invalid portrait key " + key);
            return;
        }
        currentPortraitArray = portraitMap[key];
        portraitImage.sprite = currentPortraitArray[0];
    }

    public static Sprite findSpriteByName(string name, Object[] sprites) {
        for (int i = 0; i < sprites.Length; i++) {
            if (sprites[i].name == name) return (Sprite) sprites[i];
        }
        return null;
    }
}
