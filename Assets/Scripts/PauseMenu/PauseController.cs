using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject playerHUD;
    [SerializeField] KeyCode pauseKey;
    bool gamePaused = false;
    void Start() {
        pauseMenu.SetActive(false);
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

    }
    public void ResumeGame() {
        Time.timeScale = 1.0f;
        playerHUD.SetActive(true);
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
    }
}
