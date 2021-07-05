using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script that controls the alpha (transparency) of a CanvasGroup, and
// can animate a fade in and fade out.
[RequireComponent(typeof(CanvasGroup))]
public class FadeOverlay : MonoBehaviour
{
    // Whether we start the scene fully faded out (i.e. 100% black) or not
    [SerializeField] bool startFadedOut = false;

    // The CanvasGroup we're controlling
    private CanvasGroup canvasGroup;

    // When we start, get the reference to the canvas group and set it up
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = startFadedOut ? 1 : 0;
    }

    // A coroutine that fades in from black to transparent over 'time'
    // seconds
    public IEnumerator FadeIn(float time) {
        yield return StartCoroutine(Fade(1, 0, time));
    }

    // A coroutine that fades out from transparent to black over 'time'
    // seconds
    public IEnumerator FadeOut(float time) {
        yield return StartCoroutine(Fade(0, 1, time));
    }

    // A coroutine that fades from one transparency level to another over
    // 'time' seconds
    public IEnumerator Fade(float from, float to, float time) {
        canvasGroup.alpha = from;

        float elapsed = 0f;

        while (elapsed < time) {
            var factor = elapsed / time;

            canvasGroup.alpha = Mathf.Lerp(from, to, factor);

            yield return null;
            elapsed += Time.deltaTime;
        }

        canvasGroup.alpha = to;
    }
    
}
