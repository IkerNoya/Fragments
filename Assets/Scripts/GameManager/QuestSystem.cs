using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class QuestSystem : MonoBehaviour
{
    [System.Serializable]
    public class Objective
    {
        public int id;
        public string name = "Objective Lorem Itsum";
        public bool isCompleted = false;

        public SingleObjectiveContainer objective = null;
    }

    int objectiveID = 0;
    public List<Mission> missions;

    public PlayerController player;
    [Header("Window")]
    [SerializeField] GameObject missionsWindow;
    [SerializeField] GameObject header;
    //[SerializeField] SingleObjectiveContainer objectiveContainer;
    //[SerializeField] List<Objective> objectives;
    [Header("Canvas Group")]
    [SerializeField] CanvasGroup cg;

    Text title;

    RectTransform headerRectTransform;

    Coroutine animationCoroutine;

    bool isMissionOnScreen = false;

    void Start()
    {
        PlayerController.ShowObjective += ShowMissionEvent;
    }

    public void ShowActiveMission()
    {
        isMissionOnScreen = true;
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
    public void ActivateMission(int id)
    {
        for (int i = 0; i < missions.Count; i++)
        {
            if (missions[i].id == id)
            {
                missions[i].isActive = true;
                player.AddMission(missions[i]);
                CreateMission(id);
            }
            else
            {
                missions[i].isActive = false;
            }
        }
        
        //AddStartingObjectives();

        ShowMissionEvent();
    }

    void CreateMission(int id)
    {
        GameObject header = GameObject.Instantiate(this.header, missionsWindow.transform, false);
        title = header.GetComponentInChildren<Text>();

        for (int i = 0; i < missions.Count; i++)
        {
            if (missions[i].id == id)
            {
                title.text = missions[i].title;
            }
        }

        headerRectTransform = title.transform as RectTransform;
        Canvas.ForceUpdateCanvases();
        
    }

    public void ChangeMission(int id) {
        for (int i = 0; i < missions.Count; i++)
        {
            if (missions[i].id == id)
            {
                title.text = missions[i].title;
                missions[id].isActive = true;
            }
            else
                missions[i].isActive = false;
        }
        ShowMissionEvent();

    }


    //public void AddSingleObjective(string name)
    //{
    //    Objective objective = new Objective();
    //    objective.objective = CreateObjective();
    //    objectives.Add(objective);
    //    objective.objective.SetObjectiveTitle(name, 0.5f);
    //    objective.id = objectiveID;
    //    objectiveID++;
    //    ShowMissionEvent();
    //}

    //void AddStartingObjectives()
    //{
    //    if(objectives.Count > 0)
    //    {
    //        for(int i = 0; i < objectives.Count; i++)
    //        {
    //            if (objectives[i] != null)
    //            {
    //                objectives[i].objective = CreateObjective();
    //                objectives[i].objective.SetObjectiveTitle(mission.objective[i], 1.0f);
    //                objectives[i].id = objectiveID;
    //                objectiveID++;
    //            }
    //        }
    //    }
    //}

    //public SingleObjectiveContainer CreateObjective()
    //{
    //    SingleObjectiveContainer objective = Instantiate<SingleObjectiveContainer>(objectiveContainer, missionsWindow.transform, false);
    //    return objective;
    //}

    //public void RemoveObjective(Objective objective)
    //{
    //    ShowMissionEvent();
    //    if(objective != null)
    //    {
    //        objective.objective.Hide(() =>
    //        {
    //            objective.objective = null;
    //            objectives.Remove(objective);
    //        });
    //    }
    //}

    IEnumerator MissionAnimation(bool show, float duration, System.Action onComplete)
    {
        float initialAlpha = 0.0f;
        float endAlpha = 0.0f;

        if (show)
        {
            endAlpha = 1.0f;
        }
        else
        {
            initialAlpha = 1.0f;
        }
        cg.alpha = initialAlpha;

        float time = 0.0f;
        while (time < duration)
        {
            cg.alpha = EaseInOut(initialAlpha, endAlpha, time, duration);
            yield return null;
            time += Time.deltaTime;
        }

        cg.alpha = endAlpha;

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

    IEnumerator ShowAndHideMission()
    {
        ShowActiveMission();
        yield return new WaitForSeconds(5);
        HideActiveMission(() =>
        {
            isMissionOnScreen = false;
        });
    }

    void ShowMissionEvent()
    {
        if (!isMissionOnScreen)
        {
            StartCoroutine(ShowAndHideMission());
        }
    }


    //La ID se encuentra en el inspector o dependiendo de su lugar en la lista de objectivos
    //TODO: Encontrar una manera de automatizarlo
    //public void CompleteObjective(int id)
    //{
    //    for (int i = 0; i < objectives.Count; i++)
    //    {
    //        if (objectives[i].id == id)
    //        {
    //            objectives[i].isCompleted = true;
    //            RemoveObjective(objectives[i]);
    //            break;
    //        }
    //    }
    //}

    private void OnDestroy()
    {
        PlayerController.ShowObjective -= ShowMissionEvent;
    }
    private void OnDisable()
    {
        isMissionOnScreen = false;
    }
}
