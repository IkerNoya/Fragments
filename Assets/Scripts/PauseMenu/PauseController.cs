using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PauseController : MonoBehaviour {

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject playerHUD;
    [SerializeField] KeyCode pauseKey;
    bool gamePaused = false;

    public static Action<bool> SetPause;

    void Start() {
        ResumeGame();
    }
    void Update() {
        if (Input.GetKeyDown(pauseKey)) {
            if (!gamePaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0.0f;
        playerHUD.SetActive(false);
        gamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        SetPause?.Invoke(true);
    }
    public void ResumeGame() {
        Time.timeScale = 1.0f;
        playerHUD.SetActive(true);
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        SetPause?.Invoke(false);
    }
}
