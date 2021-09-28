using System;
using UnityEngine;
using UnityEngine.UI;

public class InitialCutscene : MonoBehaviour
{
    [SerializeField] Image blackScreen;
    [SerializeField] float screenFadeSpeed;
    [SerializeField] bool hasPlayedInitialCutscene = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip dialogue;

    public static Action<bool> initialCutscene;
    public static Action<bool> endInitialCutscene;

    Animator anim;
    float alpha = 0;

    void Start()
    {
        anim = GetComponent<Animator>();

        audioSource = GetComponentInParent<AudioSource>();

        if (!hasPlayedInitialCutscene)
        {
            initialCutscene?.Invoke(false);
            hasPlayedInitialCutscene = true;
            anim.SetTrigger("PlayInitialCutscene");
        }
        alpha = blackScreen.color.a;

    }

    void Update()
    {
        if (blackScreen && blackScreen.color.a > 0)
        {
            alpha -= Time.deltaTime * screenFadeSpeed;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
        }
    }

    public void PlayAudio()
    {
        audioSource.PlayOneShot(dialogue);
    }

    public void InitialCutsceneHasEnded()
    {
        endInitialCutscene?.Invoke(true);
    }

}
