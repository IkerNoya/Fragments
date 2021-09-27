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


    void Start()
    {
        verticalSensitivity.value = ml.GetVerticalSensitivity();
        horizontalSensitivity.value = ml.GetHorizontalSensitivity();

        verticalValue.text = ((int)ml.GetVerticalSensitivity()).ToString();
        horizontalValue.text = ((int)ml.GetHorizontalSensitivity()).ToString();
    }
    public void OnVerticalSliderValueChange()
    {
        ml.SetVerticalSensitivity(verticalSensitivity.value);
        verticalValue.text = ((int)ml.GetVerticalSensitivity()).ToString();
    }
    public void OnHorizontalSliderValueChange()
    {
        ml.SetHorizontalSensitivity(horizontalSensitivity.value);
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

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
