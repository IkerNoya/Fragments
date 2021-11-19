using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameInstance : MonoBehaviour
{
    private float m_horizontalSensitivity = 100;
    private float m_verticalSensitivity = 100;
    private float m_sfxVolume = 10;
    private float m_musicVolume = 10;
    [SerializeField] AudioMixer sfxMixer;
    [SerializeField] AudioMixer musicMixer;

    [HideInInspector]
    public float horitontalSensitivity { get { return m_horizontalSensitivity; } set { m_horizontalSensitivity = value; } }
    [HideInInspector]
    public float verticalSensitivity { get { return m_verticalSensitivity; } set { m_verticalSensitivity = value; } }
    [HideInInspector]
    public float sfxVolume { get { return m_sfxVolume; } set { 
            m_sfxVolume = value; 
            sfxMixer.SetFloat("MyExposedParam", -20 + (m_sfxVolume*2)); 
            if(m_sfxVolume < 0.1f)
                sfxMixer.SetFloat("MyExposedParam", -80);
        }
    }
    [HideInInspector]
    public float musicVolume { get { return m_musicVolume; } set {
            m_musicVolume = value; 
            musicMixer.SetFloat("MyExposedParam", -13 + (m_musicVolume * 2)); 
            if(m_musicVolume < 0.1f)
            musicMixer.SetFloat("MyExposedParam", -80);

        }
    }

    static GameInstance instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    public static GameInstance Get()
    {
        return instance;
    }
    
}
