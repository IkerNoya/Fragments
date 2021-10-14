using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(LayoutElement))]
[RequireComponent(typeof(CanvasGroup))]
public class SingleObjectiveContainer : MonoBehaviour
{
    [SerializeField]
    private Text objective;

    [SerializeField]
    RectTransform rectTransform;

    [SerializeField]
    LayoutElement le;

    [SerializeField]
    CanvasGroup canvasGroup;


    public float animationDuration = 1.0f;

    Coroutine animationCoroutine = null;

    void Start()
    {
        le = GetComponent<LayoutElement>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetObjectiveTitle(string text)
    {
        objective.text = text;
        Canvas.ForceUpdateCanvases();

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(ObjectiveAnimation(true, animationDuration, null));
    }

    public void Hide(System.Action onComplete)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(ObjectiveAnimation(false, animationDuration*2, onComplete));
    }

    public IEnumerator ObjectiveAnimation(bool show, float duration, System.Action onComplete)
    {
        float startHeight = 0f;
        float endHeight = 0f;
        float startWidth = 0f;
        float endWidth = 0f;


        if (show)
        {
            startHeight = rectTransform.rect.height;
            endHeight = objective.rectTransform.rect.height;
            startWidth = rectTransform.rect.width;
            endWidth = objective.rectTransform.rect.width;
        }
        else
        {
            startHeight = rectTransform.rect.height;
            endHeight = 0f;
            startWidth = rectTransform.rect.width;
            endWidth = 0f;
        }

        float time = 0.0f;
        while (time < duration)
        {
            le.preferredHeight = EaseInOut(startHeight, endHeight, time, duration); // cambiar a lerp es lo mismo my nword
            le.preferredWidth = EaseInOut(startWidth, endWidth, time, duration);
            yield return null;
            time += Time.deltaTime;
        }

        le.preferredHeight = endHeight;
        le.preferredWidth = endWidth;

        onComplete?.Invoke();
    }

    float EaseInOut(float initial, float final, float time, float duration)
    {
        float change = final - initial;
        time /= duration / 2;
        if (time < 1f) return change / 2 * time * time + initial;
        time--;
        return -change / 2 * (time * (time - 2) - 1) + initial;
    }


}
