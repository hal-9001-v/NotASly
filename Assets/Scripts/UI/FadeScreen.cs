using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();

    [SerializeField] private float duration = 1;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn(duration));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(duration));
    }

    public IEnumerator FadeIn(float duration)
    {
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;

        float time = 0;
        while (time < duration)
        {
            CanvasGroup.alpha = time / duration;
            time += Time.deltaTime;
            yield return null;
        }
        CanvasGroup.alpha = 1;
    }

    public IEnumerator FadeOut(float duration)
    {
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;

        float time = 0;
        while (time < duration)
        {
            CanvasGroup.alpha = 1 - time / duration;
            time += Time.deltaTime;
            yield return null;
        }
        CanvasGroup.alpha = 0;
        CanvasGroup.interactable = false;
    }

}
