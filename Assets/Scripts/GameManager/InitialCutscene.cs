using System;
using UnityEngine;

public class InitialCutscene : MonoBehaviour
{

    public static Action<bool> initialCutscene;
    public static Action<bool> endInitialCutscene;
    bool hasPlayedInitialCutscene = false;
    float timer = 0;
    void Start()
    {
        if (!hasPlayedInitialCutscene)
        {
            initialCutscene?.Invoke(false);
            hasPlayedInitialCutscene = true;
        }
    }

    public void InitialCutsceneHasEnded()
    {
        endInitialCutscene?.Invoke(true);
    }
}
