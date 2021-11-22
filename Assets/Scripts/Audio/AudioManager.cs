using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip Ambient;
    [SerializeField] AudioClip FightMusic;
    [SerializeField] AudioSource CameraSource;
    [SerializeField] AudioSource ClockSource;
    bool isPaused = false;
    


    void Start()
    {

        PauseController.SetPause += Pause;

        CameraSource.clip = Ambient;
        CameraSource.Play();
    }

    public void PlayClockSound()
    {
        ClockSource.Play();
    }

    void Pause(bool value)
    {
        isPaused = value;
        PauseSFX();
    }

    void PauseSFX()
    {
        if (isPaused)
            ClockSource.Pause();
        else if (!isPaused && !ClockSource.isPlaying)
            ClockSource.UnPause();
    }

    private void OnDestroy()
    {
        PauseController.SetPause -= Pause;
    }

   public void StartFightMusic() {
        if (CameraSource.clip != FightMusic) {
            CameraSource.clip = FightMusic;
            CameraSource.Play();
            CameraSource.loop = true;
        }
    }

    public void StartAmbientMusic() {
        if (CameraSource.clip != Ambient) {
            CameraSource.clip = Ambient;
            CameraSource.Play();
            CameraSource.loop = true;
        }
    }

}
