using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadeable : MonoBehaviour {
    Color color;
    float defaultA = 1f, fadedA = 0.5f;
    float fadeSpeed = 10f, fadeValue, currentA, nextA;
    bool fadeIn, fadeOut;

    void Start () {
        color = GetComponent<SpriteRenderer>().color;
        fadeIn = false;
        fadeOut = false;
	}

    void Update()
    {
        if (fadeOut)
        {
            currentA = defaultA;
            nextA = fadedA;
        }
        else if (fadeIn)
        {
            currentA = fadedA;
            nextA = defaultA;
        }

        if (Mathf.Abs(GetComponent<SpriteRenderer>().color.a - nextA) > 0.005 && (fadeIn || fadeOut))
        {
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, Mathf.Lerp(currentA, nextA, fadeValue));
            fadeValue += fadeSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fadeOut = true;
            fadeIn = false;
            fadeValue = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fadeOut = false;
            fadeIn = true;
            fadeValue = 0;
        }
    }
}
