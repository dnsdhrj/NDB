using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndInterRound : MonoBehaviour {

    public void OnClick()
    {
        SceneManager.LoadScene(GameManager.gm.CurrentStageScene);
    }
}
