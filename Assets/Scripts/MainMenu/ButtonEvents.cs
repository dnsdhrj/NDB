using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonEvents : MonoBehaviour {

    public GameObject SetGameNameBox;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameStart()
    {
        SetGameNameBox.SetActive(true);
    }

    public void GameCredit()
    {
        SceneManager.LoadScene("Credit");
    }
}
