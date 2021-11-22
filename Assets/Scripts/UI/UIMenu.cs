using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIMenu : MonoBehaviour
{
    [Header("Exit Data")]
    [SerializeField] GameObject ExitPanel;

    [SerializeField] Image fade;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        fade.raycastTarget = true;
        Time.timeScale = 1;
        ExitPanel.SetActive(false);
    }

    void Update()
    {
        if (ExitPanel.activeSelf)
            return;
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
        ExitPanel.SetActive(false);
    }
}
