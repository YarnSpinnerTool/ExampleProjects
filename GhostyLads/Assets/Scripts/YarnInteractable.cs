using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnInteractable : MonoBehaviour {
    // internal properties exposed to editor
    [SerializeField] private string conversationStartNode;
    [SerializeField] private bool beginsInteractable;
    [SerializeField] private bool useIndicatorLight;

    // internal properties not exposed to editor
    private DialogueRunner dialogueRunner;
    private Light lightIndicatorObject;
    private bool interactable;
    private bool isCurrentConversation;
    private float defaultIndicatorIntensity;

    public void Start() {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        lightIndicatorObject = GetComponentInChildren<Light>();
        interactable = beginsInteractable;
        // get starter intensity of light then
        // if we're using it as an indicator => hide it 
        defaultIndicatorIntensity = lightIndicatorObject.intensity;
        if (useIndicatorLight) {
            lightIndicatorObject.intensity = 0;
        }
    }

    public void OnMouseDown() {
        if (interactable && !dialogueRunner.IsDialogueRunning) {
            StartConversation();
        }
    }

    private void StartConversation() {
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        if (useIndicatorLight) {
            lightIndicatorObject.intensity = defaultIndicatorIntensity;
        }
        dialogueRunner.StartDialogue(conversationStartNode);
    }

    private void EndConversation() {
        if (isCurrentConversation) {
            if (useIndicatorLight) {
                lightIndicatorObject.intensity = 0;
            }
            isCurrentConversation = false;
            Debug.Log($"Ended conversation with {name}.");
        }
    }

    [YarnCommand("enable")]
    public void EnableConversation() {
        interactable = true;
    }

    [YarnCommand("disable")]
    public void DisableConversation() {
        interactable = false;
    }
}
