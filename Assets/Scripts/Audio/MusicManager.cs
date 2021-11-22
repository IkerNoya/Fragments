using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {
    static MusicManager instance;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    [SerializeField] string sceneGameplayName;
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        
        Destroy(gameObject);
    }

    public static MusicManager Get() {
        return instance;
    }
    public void ELEGANTEKELOKE() {
        source.clip = clip;
        source.Stop();
        source.Play();
    }

    private void OnLevelWasLoaded(int level) {
        if (SceneManager.GetActiveScene().name == sceneGameplayName)
            source.Stop();
        else {
            if (!source.isPlaying) {
                source.Stop();
                source.Play();
            }
        }
    }

}
