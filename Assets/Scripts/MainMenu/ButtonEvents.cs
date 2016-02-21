using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour {

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Test");
    }

    public void GameCredit()
    {
        SceneManager.LoadScene("Credit");
    }
}
