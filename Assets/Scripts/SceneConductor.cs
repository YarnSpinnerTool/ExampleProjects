using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SceneConductor : MonoBehaviour {
    private DialogueRunner dialogueRunner; // utility object that serves lines of dialogue
    private FadeLayer fadeLayer; // black overlay used to fade in/out of scenes

    // when this scene conductor object is created
    // (in our example, this happens when the scene is created)
    private void Awake() {
        // get handles of utility objects in the scene that we need
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        fadeLayer = FindObjectOfType<FadeLayer>();

        // <<camera NAME_OF_LOCATION>>
        dialogueRunner.AddCommandHandler<string>("camera", MoveCamera);

        // <<place NAME_OF_CHARACTER NAME_OF_LOCATION NAME_OF_MARKER>>
        dialogueRunner.AddCommandHandler<string, string,string>("place", MoveCharacter);

        // <<fadeIn DURATION>> and <<fadeOut DURATION>>
        dialogueRunner.AddCommandHandler<float>("fadeIn", FadeIn);
        dialogueRunner.AddCommandHandler<float>("fadeOut", FadeOut);
        Debug.Log("SceneConductor created.");
    }

    // moves camera to camera location {location}>Camera in the scene
    private void MoveCamera(string locationName) {
        Transform destination = GetLocationMarkerWithName(locationName, "Camera");
        if (destination != null) {
            MoveTransform(Camera.main.transform, destination);
            Debug.Log($"Main Camera moved to {locationName}>Camera.");
        }
    }

    // moves character named {characterName} to location 
    // {locationName}>{markerName} in the scene
    private void MoveCharacter(string characterName, string locationName, string markerName) {
        GameObject character = GameObject.Find(characterName);
        if (character == null) {
            Debug.LogError($"There is no Character named {characterName}.");
            return;
        }
        Transform destination = GetLocationMarkerWithName(locationName, markerName);
        if (destination != null) {
            MoveTransform(character.transform, destination);
            Debug.Log($"{characterName} moved to {locationName}>{markerName}.");
        }
    }

    private void MoveTransform(Transform target, Transform destination) {
        target.position = destination.position;
        target.rotation = destination.rotation;
    }

    private Transform GetLocationMarkerWithName(string locationName, string markerName) {
        GameObject[] locations = GameObject.FindGameObjectsWithTag("Location");
        GameObject location = Array.Find(locations, t => t.name == locationName);
        if (location == null) {
            Debug.LogError($"There is no Location named {locationName}.");
            return null;
        }
        Transform marker = location.transform.Find(markerName);
        if (marker == null) {
             Debug.LogError($"There is no marker named {markerName} in Location {locationName}.");
            return null;           
        }
        return marker;
    }

    // fades in a black screen over {time} seconds
    private Coroutine FadeIn(float time = 1f) {
        Debug.Log($"Fading in from black over {time} seconds.");
        return StartCoroutine(fadeLayer.ChangeAlphaOverTime(0, time));
    }

    // fades out a black screen over {time} seconds
    private Coroutine FadeOut(float time = 1f) {
        Debug.Log($"Fading out to black over {time} seconds.");
        return StartCoroutine(fadeLayer.ChangeAlphaOverTime(1, time));
    }
}