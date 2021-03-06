using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] bool isInGame = false;
    [SerializeField] bool shouldEscapeExit = false;
    [SerializeField] string backSceneName = "";


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

        else if(Input.GetKeyDown(KeyCode.Escape) && !shouldEscapeExit)
        {
            SceneManager.LoadScene(backSceneName);
        }
    }
}
