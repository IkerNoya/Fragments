using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Image EnterButton;

    bool canInteract = false;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canInteract)
            return;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            anim.SetTrigger("out");
        }
    } 
    public void SetCanInteract()
    {
        canInteract = !canInteract;
    }

    public void MoveToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
