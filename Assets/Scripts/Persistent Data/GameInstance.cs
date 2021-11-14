using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    private float m_horizontalSensitivity = 0;
    private float m_verticalSensitivity = 0;
    private float m_sfxVolume = 0;
    private float m_musicVolume = 0;


    [HideInInspector]
    public float horitontalSensitivity { get { return m_horizontalSensitivity; } set { m_horizontalSensitivity = value; } }
    [HideInInspector]
    public float verticalSensitivity { get { return m_verticalSensitivity; } set { m_verticalSensitivity = value; } }
    [HideInInspector]
    public float sfxVolume { get { return m_sfxVolume; } set { m_sfxVolume = value; } }
    [HideInInspector]
    public float musicVolume { get { return m_musicVolume; } set { m_musicVolume = value; } }


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
