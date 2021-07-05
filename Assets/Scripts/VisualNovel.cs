using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class VisualNovel : MonoBehaviour {
    [SerializeField] CharacterList characterList;
    private Dictionary<string, Character> characters = new Dictionary<string, Character>();
    private DialogueRunner dialogueRunner; // utility object that serves lines of dialogue
    private FadeOverlay fadeOverlay; // black overlay used to fade in/out of scenes

    // when this visual novel object is created
    // (in our example, this happens when the scene is created)
    private void Awake() {
        // get handles of utility objects in the scene that we need
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        fadeOverlay = FindObjectOfType<FadeOverlay>();

        // <<camera NAME_OF_LOCATION>>
        dialogueRunner.AddCommandHandler<Location>("camera", ChangeCameraLocation);

        // <<place NAME_OF_CHARACTER>>
        dialogueRunner.AddCommandHandler<string,string>("place", PlaceCharacter);

        // <<fadeIn DURATION>>
        dialogueRunner.AddCommandHandler<float>("fadeIn", FadeIn);
        dialogueRunner.AddCommandHandler<float>("fadeOut", FadeOut);
        // <<fadeOut DURATION>>
    }

    // moves camera to camera location {location} in the scene
    private void ChangeCameraLocation(Location location) {
        Camera.main.transform.position = location.cameraMarker.position;
        Camera.main.transform.rotation = location.cameraMarker.rotation;
    }

    // looks for character named {characterName} and moves it to the location 
    // of marker named {markerName} in the scene
    private void PlaceCharacter(string characterName, string markerName) {
        Character character;

        // if this character has not been instantiated before, do so now
        if (!characters.ContainsKey(characterName)) {
            var characterPrefab = characterList.FindCharacterPrefab(characterName);
            character = Instantiate(characterPrefab);
            // and place it in the list of characters so we can find it next time
            characters[characterName] = character;
            character.name = characterName;
        } else {
            // otherwise get the one we prepared earlier
            character = characters[characterName];
        }
        // get the position/rotation of the destination marker in the scene
        // and set the position/rotation of the Character to there
        var marker = GameObject.Find(markerName);
        character.transform.position = marker.transform.position;
        character.transform.rotation = marker.transform.rotation;
    }

    // fades in a black screen over {time} seconds
    private Coroutine FadeIn(float time = 1f) {
        return StartCoroutine(fadeOverlay.FadeIn(time));
    }

    // fades out a black screen over {time} seconds
    private Coroutine FadeOut(float time = 1f) {
        return StartCoroutine(fadeOverlay.FadeOut(time));
    }
}