using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] MouseLook ml;
    [Header("Pause Panels")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject howToPlayPanel;
    [SerializeField] Slider verticalSensitivity;
    [SerializeField] Slider horizontalSensitivity;   
    [SerializeField] Slider SFXslider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Text verticalValue;
    [SerializeField] Text horizontalValue;
    [SerializeField] Text SFXValue;
    [SerializeField] Text MusicValue;

    GameInstance gameInstance;
    PauseController pause;
    void Start()
    {
        pause = FindObjectOfType<PauseController>();
        gameInstance = GameInstance.Get();
        if (gameInstance)
        {
            verticalSensitivity.value = gameInstance.verticalSensitivity;
            horizontalSensitivity.value = gameInstance.horitontalSensitivity;

            verticalValue.text = ((int)gameInstance.verticalSensitivity).ToString();
            horizontalValue.text = ((int)gameInstance.horitontalSensitivity).ToString();

            ml.SetHorizontalSensitivity(gameInstance.horitontalSensitivity);
            ml.SetVerticalSensitivity(gameInstance.verticalSensitivity);

            SFXslider.value = gameInstance.sfxVolume;
            MusicSlider.value = gameInstance.musicVolume;
        }
    }
    public void OnVerticalSliderValueChange()
    {
        gameInstance.verticalSensitivity = verticalSensitivity.value;
        ml.SetVerticalSensitivity(gameInstance.verticalSensitivity);
        verticalValue.text = ((int)ml.GetVerticalSensitivity()).ToString();
    }
    public void OnHorizontalSliderValueChange()
    {
        gameInstance.horitontalSensitivity = horizontalSensitivity.value;
        ml.SetHorizontalSensitivity(gameInstance.horitontalSensitivity);
        horizontalValue.text = ((int)ml.GetHorizontalSensitivity()).ToString();
    }
    public void OnSFXSliderValueChange()
    {
        gameInstance.sfxVolume = SFXslider.value;
        SFXValue.text = ((int)gameInstance.sfxVolume).ToString();
    }
    public void OnMusicSliderValueChange()
    {
        gameInstance.musicVolume = MusicSlider.value;
        MusicValue.text = ((int)gameInstance.musicVolume).ToString();
    }
    public void OnClickOptions()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
    }
    public void OnClickHowToPlay()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }
    public void OnClickBack()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    public void OnClickMenu()
    {
        
    }
    void Update()
    {
        if (!pause.GetPause())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf && !optionsPanel.activeSelf && !howToPlayPanel.activeSelf)
            {
                pause.ResumeGame();
            }
            else if (!pausePanel.activeSelf && optionsPanel.activeSelf && !howToPlayPanel.activeSelf)
            {
                OnClickBack();
            }
            else
            {
                OnClickBack();
            }
        }
    }
    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
