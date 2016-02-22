using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BacktoMenu : MonoBehaviour {

	public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
