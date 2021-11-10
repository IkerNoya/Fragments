using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField] Image fade;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        fade.raycastTarget = true;
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
}
