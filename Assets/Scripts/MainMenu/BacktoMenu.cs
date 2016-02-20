using UnityEngine;
using System.Collections;

public class BacktoMenu : MonoBehaviour {

	public void BackToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
}
