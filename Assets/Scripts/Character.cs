using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Character : MonoBehaviour {
    [System.Serializable] // map of textures for facial expressions
    public class Expression {
        public string name;
        public Texture2D texture;
    }

    // objects needed to render new textures on Character face
    [SerializeField] List<Expression> expressions = new List<Expression>();
    [SerializeField] Renderer faceRenderer;
    [SerializeField] int faceMaterialIndex;

    // object needed to set Character pose
    private Animator animator;

    // when this character is first created
    public void Awake() {
        // set initial pose and facial expression to defaults
        animator = GetComponentInChildren<Animator>();
        if (expressions.Count < 1) {
            Debug.LogError($"Character {name} has no available facial textures.");
            return;
        }
        SetFaceTexture(expressions[0].texture);
        Debug.Log($"Character {name} created.");
    }

    // moves character to location {location}>{markerName} in the scene
    [YarnCommand("place")]
    public void Move(Location location, string markerName) {
        Transform destination = location.GetMarkerWithName(markerName);
        if (destination != null) {
            transform.position = destination.position;
            transform.rotation = destination.rotation;
            Debug.Log($"Character {name} moved to {location.name}>{markerName}.");
        }
    }

    // tells the animator to jump to state {poseName} 
    [YarnCommand("pose")]
    public void SetPose(string poseName) {
        animator.Play("Base Layer." + poseName, 0);
        Debug.Log($"{name} adopting {poseName} pose.");
    }

    // sets character expression texture to {expressionName} texture
    [YarnCommand("expression")]
    public void SetExpression(string expressionName){
        // find the expression with the same name as we are looking for
        Expression expressionToUse = FindExpressionWithName(expressionName);
        if (expressionToUse == null) {
            Debug.LogError($"Character {name} has no Expression named {expressionName}.");
            return;
        }
        SetFaceTexture(expressionToUse.texture);
    }

    private Expression FindExpressionWithName(string expressionName) {
        var caseInsensitiveMode = System.StringComparison.InvariantCultureIgnoreCase;
        foreach (Expression expression in expressions) {
            if (expression.name.Equals(expressionName, caseInsensitiveMode)) {
                return expression;
            }
        }
        return null;
    }

    private void SetFaceTexture(Texture2D texture) {
        faceRenderer.materials[faceMaterialIndex].mainTexture = texture;
    }
}