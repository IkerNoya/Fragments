using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    public Mission mission;

    public PlayerController player;
    [Header("Window")]
    [SerializeField] GameObject missionsWindow;
    [SerializeField] GameObject header;
    [SerializeField] GameObject objectiveContainer;
    [Header("Font")]
    [SerializeField] Font font;
    [SerializeField] int titleFontSize;
    [SerializeField] int descriptionFontSize;
    

    LayoutElement le;
    CanvasGroup cg;

    Text title;
    Text description;

    RectTransform headerRectTransform;

    Coroutine animationCoroutine;

    public void ShowActiveMission()
    {
        missionsWindow.SetActive(true);
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(MissionAnimation(true, 1, null));
    }
    public void HideActiveMission(System.Action onComplete)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(MissionAnimation(false, 1, onComplete));
    }
    public void ActivateMission()
    {
        mission.isActive = true;
        player.AddMission(mission);
        CreateTitle();
        CreateDescription();
        ShowActiveMission();
    }

    void CreateTitle()
    {
        GameObject header = GameObject.Instantiate(this.header, missionsWindow.transform, false);
        title = header.GetComponent<Text>();
        title.text = mission.title;
        headerRectTransform = title.transform as RectTransform;
        Canvas.ForceUpdateCanvases();
        
    }

    void CreateDescription()
    {
        GameObject objective = GameObject.Instantiate(objectiveContainer, missionsWindow.transform, false);
        description = objective.GetComponentInChildren<Text>();
        le = objective.GetComponent<LayoutElement>();
        cg = objective.GetComponent<CanvasGroup>();
        description.text = mission.objective[0];
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(MissionAnimation(true, 2, null));
    }

    IEnumerator MissionAnimation(bool show, float duration, System.Action onComplete)
    {
        float startHeight = 0f;
        float endHeight = 0f;
        float startWidth = 0f;
        float endWidth = 0f;
        if (show)
        {
            startHeight = description.rectTransform.rect.height;
            endHeight = headerRectTransform.rect.height;
            startWidth = description.rectTransform.rect.width; 
            endWidth = headerRectTransform.rect.width;
        }
        else
        {
            startHeight = description.rectTransform.rect.height;
            endHeight = 0f;
            startWidth = description.rectTransform.rect.width;
            endWidth = 0f;
        }

        float time = 0.0f;
        while (time < duration)
        {
            le.preferredHeight = EaseInOut(startHeight, endHeight, time, duration);
            le.preferredWidth = EaseInOut(startWidth, endWidth, time, duration);
            Debug.Log(le.preferredWidth);
            yield return null;
            time += Time.deltaTime;
        }
        if (!show) missionsWindow.SetActive(false);

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
