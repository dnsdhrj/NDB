using UnityEngine;
using System.Collections;

public class ButtonEvents : MonoBehaviour {

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameStart()
    {
        Application.LoadLevel("Game");
    }

    public void GameCredit()
    {
        Application.LoadLevel("Credit");
    }
}
