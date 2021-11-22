using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIMenu : MonoBehaviour
{
    [Header("Exit Data")]
    [SerializeField] GameObject ExitPanel;
    [Header("Screens")]
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Options;
    [SerializeField] GameObject Credits;
    [SerializeField] GameObject Controls;
    [SerializeField] Image fade;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        fade.raycastTarget = true;
        Time.timeScale = 1;
        ExitPanel.SetActive(false);
        Options.SetActive(false);
        Credits.SetActive(false);
        Controls.SetActive(false);
    }

    void Update()
    {
        if (ExitPanel.activeSelf || Options.activeSelf || Credits.activeSelf || Controls.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                OnClickBack();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickExit();
        }
    }

    public void OnClickPlay()
    {
        anim.SetTrigger("out");
    }
    public void SetCanInteract()
    {
        fade.raycastTarget = !fade.raycastTarget;
    }
    public void MoveToGame()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void OnClickExit()
    {
        ExitPanel.SetActive(true);
    }
    public void OnClickNo()
    {
        Menu.SetActive(true);
        ExitPanel.SetActive(false);
    }
    public void OnClickBack()
    {
        Menu.SetActive(true);
        ExitPanel.SetActive(false);
        Options.SetActive(false);
        Credits.SetActive(false);
        Controls.SetActive(false);
    }
    public void OnClick(GameObject panel)
    {
        Menu.SetActive(false);
        panel.SetActive(true);
    }
}
