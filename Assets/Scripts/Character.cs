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

    // tell the animator to jump to state {poseName} 
    [YarnCommand("pose")]
    public void SetPose(string poseName) {
        animator.Play("Base Layer." + poseName, 0);
        Debug.Log($"{name} adopting {poseName} pose.");
    }

    // sets character expression texture to {expressionName} texture
    [YarnCommand("expression")]
    public void SetExpression(string expressionName){
        // find the expression with the same name as we are looking for
        Expression expressionToUse = null;
        foreach (var expression in expressions) {
            if (expression.name.Equals(expressionName, System.StringComparison.InvariantCultureIgnoreCase)) {
                expressionToUse = expression;
                break;
            }
        }
        // get the faceRenderer to apply the expression texture to the Character's face
        SetFaceTexture(expressionToUse.texture);
    }

    private void SetFaceTexture(Texture2D texture) {
        faceRenderer.materials[faceMaterialIndex].mainTexture = texture;
    }
}