using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] bool isInGame = false;

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (isInGame)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickQuit();
        }
    }
}
