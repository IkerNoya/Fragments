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
    [SerializeField] Slider verticalSensitivity;
    [SerializeField] Slider horizontalSensitivity;
    [SerializeField] TMPro.TextMeshProUGUI verticalValue;
    [SerializeField] TMPro.TextMeshProUGUI horizontalValue;

    GameInstance gameInstance;
    void Start()
    {
        gameInstance = GameInstance.Get();
        if (gameInstance)
        {
            verticalSensitivity.value = gameInstance.verticalSensitivity;
            horizontalSensitivity.value = gameInstance.horitontalSensitivity;

            verticalValue.text = ((int)gameInstance.verticalSensitivity).ToString();
            horizontalValue.text = ((int)gameInstance.horitontalSensitivity).ToString();

            ml.SetHorizontalSensitivity(gameInstance.horitontalSensitivity);
            ml.SetVerticalSensitivity(gameInstance.verticalSensitivity);
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
    public void OnClickOptions()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void OnClickBack()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OnClickMenu()
    {
        
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
