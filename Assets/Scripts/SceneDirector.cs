using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SceneDirector : MonoBehaviour {
    private DialogueRunner dialogueRunner; // utility object that serves lines of dialogue
    private FadeLayer fadeLayer; // black overlay used to fade in/out of scenes

    // when this scene conductor object is created
    // (in our example, this happens when the scene is created)
    private void Awake() {
        // get handles of utility objects in the scene that we need
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        fadeLayer = FindObjectOfType<FadeLayer>();

        // <<camera NAME_OF_LOCATION>>

        // <<fadeIn DURATION>> and <<fadeOut DURATION>>
        Debug.Log("SceneConductor created.");
    }

    // moves camera to camera location {location}>Camera in the scene
    private void MoveCamera(Location location) {
        Transform destination = location.GetMarkerWithName("Camera");
        if (destination != null) {
            Camera.main.transform.position = destination.position;
            Camera.main.transform.rotation = destination.rotation;
            Debug.Log($"Main Camera moved to {location.name}>Camera.");
        }
    }

    // fades in from a black screen over {time} seconds
    private Coroutine FadeIn(float time = 1f) {
        Debug.Log($"Fading in from black over {time} seconds.");
        return StartCoroutine(fadeLayer.ChangeAlphaOverTime(0, time));
    }

    // fades out to a black screen over {time} seconds
    private Coroutine FadeOut(float time = 1f) {
        Debug.Log($"Fading out to black over {time} seconds.");
        return StartCoroutine(fadeLayer.ChangeAlphaOverTime(1, time));
    }
}