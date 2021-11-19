using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{

    GameInstance gameInstance;

    float horizontalSensitivity = 0;
    float verticalSensitivity = 0;
    float sfxVolume = 0;
    float musicVolume = 0;

    [Header("Sliders")]
    [SerializeField] Slider h_sensitivity;
    [SerializeField] Slider v_sensitivity;
    [SerializeField] Slider sfx_volume;
    [SerializeField] Slider music_volume;
    [Header("Values")]
    [SerializeField] Text hValue;
    [SerializeField] Text vValue;
    [SerializeField] Text sfxValue;
    [SerializeField] Text musicValue;


    void Start()
    {
        gameInstance = GameInstance.Get();
        if (gameInstance != null)
        {
            horizontalSensitivity = gameInstance.horitontalSensitivity;
            verticalSensitivity = gameInstance.verticalSensitivity;
            sfxVolume = gameInstance.sfxVolume;
            musicVolume = gameInstance.musicVolume;
        }

        h_sensitivity.value = horizontalSensitivity;
        v_sensitivity.value = verticalSensitivity;
        sfx_volume.value = sfxVolume;
        music_volume.value = musicVolume;

        hValue.text = ((int)horizontalSensitivity).ToString();
        vValue.text = ((int)verticalSensitivity).ToString();
        sfxValue.text = ((int)sfxVolume).ToString();
        musicValue.text = ((int)musicVolume).ToString();
    }

    public void UpdateSensitivityH()
    {
        horizontalSensitivity = h_sensitivity.value;
        hValue.text = ((int)horizontalSensitivity).ToString();
        gameInstance.horitontalSensitivity = horizontalSensitivity;
    }
    public void UpdateSensitivityV()
    {
        verticalSensitivity = v_sensitivity.value;
        vValue.text = ((int)verticalSensitivity).ToString();
        gameInstance.verticalSensitivity = verticalSensitivity;
    }
    public void UpdateSFXVolume()
    {
        sfxVolume = sfx_volume.value;
        sfxValue.text = sfxVolume.ToString("F1");
        gameInstance.sfxVolume = sfxVolume;
    }
    public void UpdateMusicVolume()
    {
        musicVolume = music_volume.value;
        musicValue.text =musicVolume.ToString("F1");
        gameInstance.musicVolume = musicVolume;
    }
}
