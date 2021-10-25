using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Console : MonoBehaviour
{
    [SerializeField] TMP_InputField console;
    [SerializeField] TMP_Text text;

    [SerializeField] string fieldInput = "";

    public static event Action<bool> ConsolePause;

    public UnityEvent InfiniteAmmo;
    public UnityEvent MaxDamage;
    public UnityEvent GodMode;

    bool godMode = false;
    bool infiniteAmmo = false;
    bool maxDamage = false;

    bool pause = false;

    void Start()
    {
        console.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!console.gameObject.activeSelf)
            {
                pause = !pause;
                console.gameObject.SetActive(true);
                ConsolePause?.Invoke(pause);
            }
            else
            {
                pause = !pause;
                console.gameObject.SetActive(false);
                ConsolePause?.Invoke(pause);
            }
        }
        if (console.gameObject.activeSelf)
        {
            console.ActivateInputField();
        }       
    }

    public void SetCheat(string cheat)
    {
        fieldInput = cheat;
    }

    void CheckCheat()
    {
        switch (fieldInput)
        {
            case "god":
                godMode = !godMode;
                GodMode?.Invoke();
                Debug.Log("God Mode " + godMode);
                fieldInput = "";
                break;
            case "infinite_ammo":
                infiniteAmmo = !infiniteAmmo;
                InfiniteAmmo?.Invoke();
                Debug.Log("InfiniteAmmo " + infiniteAmmo);
                fieldInput = "";
                break;
            case "max_damage":
                maxDamage = !maxDamage;
                MaxDamage?.Invoke();
                Debug.Log("Max Damage " + maxDamage);
                fieldInput = "";
                break;
        }
    }

    public void EndEdit()
    {
        CheckCheat();
        text.text = "";
        pause = !pause;
        console.gameObject.SetActive(false);
        ConsolePause?.Invoke(pause);
    }


}
