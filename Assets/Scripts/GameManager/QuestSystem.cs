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
    [Header("Font")]
    [SerializeField] Font font;
    [SerializeField] int titleFontSize;
    [SerializeField] int descriptionFontSize;
    Text title;
    Text description;

    float textWidth = 600;
    float textHeight = 92;

    public void ShowActiveMission()
    {
        missionsWindow.SetActive(true);
        title.text = mission.title;
        description.text = mission.description;
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
        GameObject title = new GameObject("Title");
        title.AddComponent<Text>();
        title.transform.SetParent(missionsWindow.transform);
        title.transform.position = Vector3.zero;
        title.transform.localScale = Vector3.one;
        this.title = title.GetComponent<Text>();
        this.title.rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
        this.title.fontSize = titleFontSize;
        this.title.font = new Font();
        this.title.font = font;
    }
    void CreateDescription()
    {
        GameObject description = new GameObject("Description");
        description.AddComponent<Text>();
        description.transform.SetParent(missionsWindow.transform);
        description.transform.position = Vector3.zero;
        description.transform.localScale = Vector3.one;
        this.description = description.GetComponent<Text>();
        this.description.rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
        this.description.fontSize = descriptionFontSize;
        this.description.font = new Font();
        this.description.font = font;
    }
}
